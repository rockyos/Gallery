 using CoreTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        void SaveToDB();
    }

    public class PhotoRepository<T> : IRepository<T> where T : class
    {
        private readonly PhotoContext context;

        public PhotoRepository(PhotoContext photoContext)
        {
            context = photoContext;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> photolist = context.Set<T>().AsEnumerable();
            return Task.FromResult(photolist);
        }


        public Task<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            T photolist = context.Set<T>().Find(predicate);
            return Task.FromResult(photolist);
        }


        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            T existing = context.Set<T>().Find(entity);
            if (existing != null) context.Set<T>().Remove(existing);
        }
        public void SaveToDB()
        {
            context.SaveChangesAsync();
        }
    }
}
