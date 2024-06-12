using API_BLOG.Models;
using API_BLOG.Models.Dtos.Post;
using API_BLOG.Models.Dtos.Usuario;
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
        }
    }
}
