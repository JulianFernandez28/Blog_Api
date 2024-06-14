using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API_BLOG.Models.Entitys;

namespace API_BLOG.Models.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreateOn { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        [Required]
        public int PostId { get; set; }

    }
}
