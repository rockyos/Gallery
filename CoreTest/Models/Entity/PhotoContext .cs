using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreTest.Models.Entity
{
    public sealed class PhotoContext : IdentityDbContext
    {
        public PhotoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
    }

}
