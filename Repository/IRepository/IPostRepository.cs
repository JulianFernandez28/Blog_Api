using API_BLOG.Models.Entitys;

namespace API_BLOG.Repository.IRepository
{
    public interface IPostRepository: IRepository<Post>
    {
        Task<Post> Update(Post entity);

        Task<Post> Delete(Post entity);
    }
}
