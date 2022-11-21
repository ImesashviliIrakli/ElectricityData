using ElectricityData.Data;
using ElectricityData.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electricity_data_tests
{
    public class Tests
    {
        public static DbContextOptions<AppDbContext> dbContextOptions { get; set; }
        public static string connectionString = @"Server=(localdb)\mssqllocaldb;Database=ElectricityProject;Trusted_Connection=True;MultipleActiveResultSets=true";

        [TestCase(ExpectedResult = true)]
        public async Task<bool> GetMayTest()
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
                var stream = await repo.GetStreamMay();

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

        [TestCase(ExpectedResult = true)]
        public async Task<bool> GetAprilTest()
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
                var stream = await repo.GetStreamApril();

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


        [TestCase(ExpectedResult = true)]
        public async Task<bool> GetMarchTest()
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
                var stream = await repo.GetStreamMarch();

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

        [TestCase(ExpectedResult = true)]
        public async Task<bool> GetFebruaryTest()
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
                var stream = await repo.GetStreamFebruary();

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
