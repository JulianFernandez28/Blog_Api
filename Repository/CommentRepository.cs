using API_BLOG.Data;
using API_BLOG.Models.Dtos.Comment;
using API_BLOG.Models.Entitys;
using API_BLOG.Repository.IRepository;
using System.Linq.Expressions;

namespace API_BLOG.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context ):base(context) 
        {
            _context = context;
        }
        public async Task<Comment> Delete(Comment comment)
        {
            comment.UpdateOn= DateTime.Now;
            comment.Status = false;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;

        }

        public async Task<Comment> Update(Comment comment)
        {
            comment.UpdateOn = DateTime.Now;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public Expression<Func<Comment, bool>> GetBy(CommentSearchDto comment)
        {

            if (comment.PostId != 0)
                return  x => x.PostId == comment.PostId && x.Status == true;

            if (!string.IsNullOrEmpty(comment.UsuarioId)) 
                return x => x.UsuarioId == comment.UsuarioId && x.Status == true;

            if (!string.IsNullOrEmpty(comment.Content))
                return x=> x.Content== comment.Content && x.Status == true;   


            return x=> x.Status == true;
        }

       
    }
}
