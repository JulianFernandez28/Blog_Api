using API_BLOG.Models.Dtos.Usuario;

namespace API_BLOG.Models.Dtos.Login
{
    public class LoginResponseDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
    }
}
