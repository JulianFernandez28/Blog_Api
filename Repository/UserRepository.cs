using API_BLOG.Data;
using API_BLOG.Models.Dtos.Login;
using API_BLOG.Models.Dtos.Register;
using API_BLOG.Models.Dtos.Usuario;
using API_BLOG.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API_BLOG.Repository.IRepository;

namespace API_BLOG.Repository
{
    public class UserRepository:IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;


        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<Usuario> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {

            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public bool isUserUnique(string userName)
        {
            var usuario = _db.Usuarios.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());

            if (usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginResquestDto)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower() == loginResquestDto.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(usuario, loginResquestDto.Password);


            if (usuario == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //si usuario existe generamos el JW TOKEN
            var roles = await _userManager.GetRolesAsync(usuario);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Email),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDTO = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDto>(usuario),
                Rol = roles.FirstOrDefault()
              
            };
            return loginResponseDTO;

        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto)
        {
            RegisterResponseDto registro = new RegisterResponseDto();
            Usuario usuario = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Nombres = registerRequestDto.Nombres,
            };
            try
            {
                var resultado = await _userManager.CreateAsync(usuario, registerRequestDto.Password);
                if (resultado.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                    }

                    await _userManager.AddToRoleAsync(usuario, registerRequestDto.Rol);
                    var usuarioAp = _db.Usuarios.FirstOrDefault(u => u.UserName == registerRequestDto.Email);

                    registro.Usuario = _mapper.Map<UsuarioDto>(usuarioAp);
                    return registro;
                }
                registro.Usuario = new UsuarioDto();

                foreach (var error in resultado.Errors)
                {
                    registro.ErrorMessages.Add(error.Description);
                }

            }
            catch (Exception)
            {

                throw;
            }
            return registro;
        }
    }
}
