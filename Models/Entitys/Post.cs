using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_BLOG.Models.Entitys
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public bool Status { get; set; } = true;
        [Required]
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }

    }
}
