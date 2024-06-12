namespace API_BLOG.Models.Dtos.Register
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Nombres { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}
