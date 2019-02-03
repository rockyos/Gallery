﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTest.Models
{
    public class PhotoContext : DbContext
    {
        public PhotoContext()
        {
        }

        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Photo> Photos { get; set; }
    }

    public interface IRepository
    {
        Task<List<Photo>> GetAll();
        Task<Photo> GetOne(int id);
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
            _context.Photos.ToListAsync();

        public Task<Photo> GetOne(int id) =>
                _context.Photos.SingleOrDefaultAsync(m => m.Id == id);

        public void Add(Photo item) =>
            _context.Photos.Add(item);

        public void Remove(Photo item) =>
            _context.Photos.Remove(item);

        public Task SaveChanges() =>
            _context.SaveChangesAsync();
    }

    //public class UnitOfWork : IDisposable
    //{
    //    PhotoContext db = new PhotoContext();
    //    PhotoRepository repository;

    //    public PhotoRepository Photos
    //    {
    //        get
    //        {
    //            if (repository == null)
    //                repository = new PhotoRepository(db);
    //            return repository;
    //        }
    //    }

    //    public void Save()
    //    {
    //        db.SaveChanges();
    //    }

    //    bool disposed = false;

    //    public virtual void Dispose(bool disposing)
    //    {
    //        if (!disposed)
    //        {
    //            if (disposing)
    //            {
    //                db.Dispose();
    //            }
    //            disposed = true;
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }
    //}
}
