using API_BLOG.Models.Dtos.Usuario;

namespace API_BLOG.Models.Dtos.Register
{
    public class RegisterResponseDto
    {
        public RegisterResponseDto()
        {
            ErrorMessages = new List<string>();
        }
        public UsuarioDto Usuario { get; set; }
        public List<String> ErrorMessages { get; set; }
    }
}
