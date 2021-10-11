using Array.Test.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Array.Test.Data 
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) 
        {
        }

        public DbSet<Account> Account { get; set; }
    }
}