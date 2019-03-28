using Microsoft.EntityFrameworkCore;

namespace CoreTest.Models.Entity
{
    public sealed class PhotoContext : DbContext
    {
        public PhotoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
