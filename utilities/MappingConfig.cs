using API_BLOG.Models.Dtos;
using API_BLOG.Models.Dtos.Comment;
using API_BLOG.Models.Dtos.Post;
using API_BLOG.Models.Entitys;
using AutoMapper;

namespace API_BLOG.utilities
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            //Usuarios
            CreateMap<Usuario, UsuarioDto>().ReverseMap();

            //Post
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Post, PostCreateDto>().ReverseMap();
            CreateMap<Post, PostUpdateDto>().ReverseMap();

            //comentarios
            CreateMap<Comment, CommentCreateDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
            CreateMap<Comment, CommentSearchDto>().ReverseMap();
        }
    }
}
