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
    }

    public class PhotoRepository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhotoRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> photolist = _unitOfWork.Context.Set<T>().AsEnumerable();
            return Task.FromResult(photolist);
        }


        public Task<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            T photolist = _unitOfWork.Context.Set<T>().Find(predicate);
            return Task.FromResult(photolist);
        }


        public void Add(T entity)
        {
            _unitOfWork.Context.Set<T>().Add(entity);
        }

        public void Remove(T entity)
        {
            T existing = _unitOfWork.Context.Set<T>().Find(entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove(existing);
        }
    }
}
