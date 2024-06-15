using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_BLOG.Models.Dtos.Post
{
    public class PostCreateDto
    {
        
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string UsuarioId { get; set; }

        public IFormFile Image { get; set; }

    }
}
