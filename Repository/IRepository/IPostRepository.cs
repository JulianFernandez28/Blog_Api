using API_BLOG.Models.Dtos.Post;
using API_BLOG.Models.Entitys;
using CloudinaryDotNet.Actions;

namespace API_BLOG.Repository.IRepository
{
    public interface IPostRepository: IRepository<Post>
    {
        Task<Post> Update(Post entity);

        Task<Post> Delete(Post entity);

        Task<ImageUploadResult> UploadImage(PostCreateDto postCreate);
    }
}
