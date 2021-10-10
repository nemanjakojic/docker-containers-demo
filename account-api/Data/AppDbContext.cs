using code.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace code.Data 
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