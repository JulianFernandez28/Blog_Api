using API_BLOG.Models.Dtos.Login;
using API_BLOG.Models.Dtos.Register;

namespace API_BLOG.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        bool isUserUnique(string username);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

        Task<RegisterResponseDto> Register(RegisterRequestDto registerRequestDto); 
    }
}
