using Microsoft.AspNetCore.Identity;

namespace API_BLOG.Models.Entitys
{
    public class Usuario : IdentityUser
    {
        public string Nombres { get; set; }
        public bool Status { get; set; } = true;
    }
}
