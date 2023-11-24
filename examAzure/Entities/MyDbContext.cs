using Microsoft.EntityFrameworkCore;

namespace examAzure.Entities
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options): base(options) { 
        
        }
        
        public DbSet<OrderTbl> OrderTbl { get; set; }

    }
}
