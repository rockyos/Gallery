using Microsoft.EntityFrameworkCore;


namespace CoreTest.Models
{
    public class PhotoContext : DbContext
    {
        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
    }

}
