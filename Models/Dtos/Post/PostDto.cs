using API_BLOG.Models.Entitys;
using System.ComponentModel.DataAnnotations;
using System;

namespace API_BLOG.Models.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public string Image { get; set; }

        public string UsuarioId { get; set; }

        public Usuario Usuario { get; set; }

        public DateTime CreateOn { get; set; }

    }
}
