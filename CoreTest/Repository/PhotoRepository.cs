using CoreTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreTest.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetList();
        Task<T> GetOneAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entityList);
        void Remove(T entity);
        Task SaveToDBAsync();
    }

    public class PhotoRepository<T> : IRepository<T> where T : class
    {
        private readonly PhotoContext context;

        public PhotoRepository(PhotoContext photoContext)
        {
            context = photoContext;
        }

        public IQueryable<T> GetList()
        {
            return context.Set<T>();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }


        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entityList)
        {
            await context.Set<T>().AddRangeAsync(entityList);
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task SaveToDBAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
