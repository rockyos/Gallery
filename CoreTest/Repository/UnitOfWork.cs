using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        PhotoContext Context { get; }
        void SaveToDB();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public PhotoContext Context { get; }
        public UnitOfWork(PhotoContext context)
        {
            Context = context;
        }
        public void SaveToDB()
        {
            Context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Context.Dispose();
        }

    }
}
