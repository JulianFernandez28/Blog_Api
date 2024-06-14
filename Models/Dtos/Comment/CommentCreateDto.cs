using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_BLOG.Models.Dtos.Comment
{
    public class CommentCreateDto
    {

        [Required]
        public string Content { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [Required]
        public int PostId { get; set; }

    }
}
