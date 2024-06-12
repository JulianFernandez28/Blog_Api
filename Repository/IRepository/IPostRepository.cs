using API_BLOG.Models;

namespace API_BLOG.Repository.IRepository
{
    public interface IPostRepository: IRepository<Post>
    {
        Task<Post> Update(Post entity);
    }
}
