using API_BLOG.Models.Dtos.Comment;
using API_BLOG.Models.Entitys;
using System.Linq.Expressions;

namespace API_BLOG.Repository.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<Comment> Update(Comment comment);

        Task<Comment> Delete(Comment comment);

        Expression<Func<Comment, bool>> GetBy(CommentSearchDto comment);

    }
}
