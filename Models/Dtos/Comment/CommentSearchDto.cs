using API_BLOG.Models.Entitys;
using System.ComponentModel.DataAnnotations;

namespace API_BLOG.Models.Dtos.Comment
{
    public class CommentSearchDto
    {
        public int Id { get; set; }


        public string Content { get; set; }

        public DateTime CreateOn { get; set; }


        public string UsuarioId { get; set; }

        public int PostId { get; set; }
    }
}
