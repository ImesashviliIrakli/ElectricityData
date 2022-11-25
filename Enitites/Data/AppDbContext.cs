using Enitites;
using Microsoft.EntityFrameworkCore;
namespace Enitites.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AggregatedData> AggregatedData { get; set; }
    }
}
