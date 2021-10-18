using Docker.Test.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Docker.Test.Data 
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