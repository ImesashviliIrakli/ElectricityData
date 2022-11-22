using Enitites;
using Microsoft.EntityFrameworkCore;
namespace Repositories.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<GroupedTinklasModel> GroupedTinklas { get; set; }
    }
}
