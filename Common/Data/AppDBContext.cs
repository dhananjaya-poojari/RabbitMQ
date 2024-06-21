using Microsoft.EntityFrameworkCore;
using Common.Models;

namespace Common.Data
{
    public class AppDBContext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("InMemoryDb");
        //}

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
