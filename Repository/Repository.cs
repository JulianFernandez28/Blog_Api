using API_BLOG.Data;
using API_BLOG.Models.Specifications;
using API_BLOG.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_BLOG.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet =  _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();

        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true, string includProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query =  query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (includProperties != null)
            {
                foreach (var incluirProp in includProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null, string includProperties = null)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if (includProperties != null)
            {
                foreach (var incluirProp in includProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return await query.ToListAsync();
        }

        public PagedList<T> GetAllPaginado(Parameters parameters, Expression<Func<T, bool>> filter = null, string includProperties = null)
        {
            IQueryable<T> query = dbSet;

            

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includProperties != null)
            {
                foreach (var incluirProp in includProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            query = query.OrderByDescending(n => n);

            
            
            return PagedList<T>.ToPagedList(query, parameters.PageNumber, parameters .PageSize);
        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
           await _context.SaveChangesAsync();
        }
    }
}
