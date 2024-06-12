using API_BLOG.Models.Specifications;
using System.Linq.Expressions;

namespace API_BLOG.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includProperties= null);

        PagedList<T> GetAllPaginado(Parameters parameters, Expression<Func<T, bool>>? filter = null, string? includProperties = null);


        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includProperties = null);

        Task Remove(T entity);

        Task Save();
             
    }
}
