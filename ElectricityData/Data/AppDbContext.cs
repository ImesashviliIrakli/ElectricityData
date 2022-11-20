using ElectricityData.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricityData.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ElectricityModel> ElectricityData { get; set; }
        public DbSet<GroupedTinklasModel> GroupedTinklas { get; set; }

    }
}
