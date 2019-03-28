using Microsoft.EntityFrameworkCore;


namespace CoreTest.Models
{
    public sealed class PhotoContext : DbContext
    {
        public PhotoContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
