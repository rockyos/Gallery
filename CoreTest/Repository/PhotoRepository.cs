using CoreTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTest.Repository
{
    public interface IRepository
    {
        Task<List<Photo>> GetAll();
        Task<Photo> GetOne(string guid);
        void Add(Photo item);
        void Remove(Photo item);
        Task SaveChanges();
    }

    public class PhotoRepository : IRepository
    {
        private readonly PhotoContext _context;

        public PhotoRepository(PhotoContext context)
        {
            _context = context;
        }

        public Task<List<Photo>> GetAll() =>
            _context.Photos.Select(x => new Photo()
            {
                Guid = x.Guid,
                PhotoName = x.PhotoName,
                Id = x.Id
            }).ToListAsync();

        public Task<Photo> GetOne(string guid) =>
                _context.Photos.SingleOrDefaultAsync(m => m.Guid == guid);

        public void Add(Photo item) =>
            _context.Photos.Add(item);

        public void Remove(Photo item) =>
            _context.Photos.Remove(item);

        public Task SaveChanges() =>
            _context.SaveChangesAsync();
    }
}
