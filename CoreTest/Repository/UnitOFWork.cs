using CoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Repository
{
    

    public class UnitOFWork : IDisposable
    {
        private PhotoContext _context;
        private PhotoRepository<Photo> _repository;

        public UnitOFWork(PhotoContext сontext)
        {
            _context = сontext;
        }

        public PhotoRepository<Photo> PhotoRepository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new PhotoRepository<Photo>(_context);
                }
                return _repository;
            }
        }

        public void SaveAll()
        {
            _context.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
