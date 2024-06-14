using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_BLOG.Models.Entitys
{
    public class Comment
    {
        
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }

         [Required]
         public string Content { get; set; }

         public DateTime CreateOn { get; set; }

        public DateTime UpdateOn { get; set; }

        public bool Status { get; set; } = true;

        [Required]
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        
        [Required]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        

    }
}
