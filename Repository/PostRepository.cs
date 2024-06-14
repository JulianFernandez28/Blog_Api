using API_BLOG.Data;
using API_BLOG.Models.Entitys;
using API_BLOG.Repository.IRepository;

namespace API_BLOG.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<Post> Update(Post entity)
        {
            entity.UpdateOn =  DateTime.Now;
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<Post> Delete(Post entity)
        {
            entity.UpdateOn = DateTime.Now;
            entity.Status = false;
            _context.Posts.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
    }
}
