using Microsoft.EntityFrameworkCore;
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

}
