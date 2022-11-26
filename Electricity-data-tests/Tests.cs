using Enitites;
using Enitites.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories;
using System;

namespace Electricity_data_tests
{
    public class Tests
    {
        private static DbContextOptions<AppDbContext> s_dbContextOptions { get; set; }
        private static string s_connectionString = @"Server=(localdb)\mssqllocaldb;Database=ElectricityProject;Trusted_Connection=True;MultipleActiveResultSets=true";

        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = true)]
        [TestCase(4, ExpectedResult = true)]
        [TestCase(5, ExpectedResult = true)]

        public async Task<bool> GetByMonthTests(int month)
        {
            try
            {
                ILogger<ElectricityRepository> logger = Mock.Of<ILogger<ElectricityRepository>>();

                s_dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(s_connectionString)
                    .Options;

                var context = new AppDbContext(s_dbContextOptions);

                var repo = new ElectricityRepository(context, logger);

                List<AggregatedData> result = await repo.GetByMonth(month);

                Assert.That(result, Is.Not.Null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [TestCase(ExpectedResult = true)]
        public async Task<bool> GetAllTest()
        {
            try
            {
                ILogger<ElectricityRepository> logger = Mock.Of<ILogger<ElectricityRepository>>();

                s_dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(s_connectionString)
                    .Options;

                var context = new AppDbContext(s_dbContextOptions);

                var repo = new ElectricityRepository(context, logger);
                List<AggregatedData> result = await repo.GetAll();

                Assert.That(result, Is.Not.Null);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
