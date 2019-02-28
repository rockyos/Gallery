using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync();
        Task InsertAsync(T entity);
        Task SubmitChangesAsync();
        Task RemoveAsync(T entity);
    }
}