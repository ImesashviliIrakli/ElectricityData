using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Repositories;
using Repositories.Data;

namespace Electricity_data_tests
{
    public class Tests
    {
        public static DbContextOptions<AppDbContext> dbContextOptions { get; set; }
        public static string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ElectricityProject;Trusted_Connection=True;MultipleActiveResultSets=true";

        [TestCase("10763/2022-02", ExpectedResult = true)]
        [TestCase("10764/2022-03", ExpectedResult = true)]
        [TestCase("10765/2022-04", ExpectedResult = true)]
        [TestCase("10766/2022-05", ExpectedResult = true)]

        public async Task<bool> GetDataTests(string month)
        {
            ILogger<ElectricityRepository> logger = Mock.Of<ILogger<ElectricityRepository>>();

            var httpClient = new HttpClient();

            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new AppDbContext(dbContextOptions);

            var repo = new ElectricityRepository(httpClient, context, logger);

            try
            {
                var stream = await repo.GetStream(month);

                Assert.That(stream, Is.Not.Null);

                var result = await repo.Add(stream);

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
