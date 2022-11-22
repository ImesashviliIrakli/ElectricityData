using CsvHelper.Configuration;
using CsvHelper;
using Enitites;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;

namespace Repositories
{
    public class ElectricityRepository : IElectricityRepository
    {
        #region Injection
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly ILogger<ElectricityRepository> _logger;
        public ElectricityRepository(
            HttpClient httpClient,
            AppDbContext context,
            ILogger<ElectricityRepository> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
        }
        #endregion


        public async Task<Stream> GetStream(string month)
        {
            var methodName = nameof(GetStream);

            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(200);
                var url = $"https://data.gov.lt/dataset/1975/download/" + month + ".csv";

                _logger.LogInformation($"{methodName} => Getting stream from url");

                var response = await _httpClient.GetAsync(url);
                var readData = await response.Content.ReadAsStreamAsync();

                _logger.LogInformation($"{methodName} => Successfully got stream for may");

                return readData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<GroupedTinklasModel>> Add(Stream stream)
        {
            var methodName = nameof(Add);

            try
            {
                var streamReader = new StreamReader(stream);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                //get records rom stream
                var csv = new CsvReader(streamReader, config);
                var records = csv.GetRecords<ElectricityModel>().Where(x => x.OBT_PAVADINIMAS.Equals("Butas"));

                //Get grouped data using LINQ
                var groupedData = (from record in records
                                   group record by record.TINKLAS into groupResult
                                   select (new GroupedTinklasModel
                                   {
                                       Tinklas = groupResult.Key,
                                       PPlusSum = groupResult.Sum(x => x.PPlus),
                                       PMinusSum = groupResult.Sum(x => x.PMINUS),
                                       Month = groupResult.Max(x => x.PL_T)
                                   })).ToList();

                //check if the data already exists
                var month = groupedData.Select(x => x.Month.Month).FirstOrDefault();
                List<GroupedTinklasModel> groupedRecords = await _context.GroupedTinklas.Where(x => x.Month.Month == month).ToListAsync();

                if (groupedRecords.Count == 0)
                {
                    _logger.LogInformation($"{methodName} => adding data to database");
                    await _context.GroupedTinklas.AddRangeAsync(groupedData);
                    await _context.SaveChangesAsync();
                }

                //return success
                _logger.LogInformation($"{methodName} => Successfully gathered grouped data");
                return groupedData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<GroupedTinklasModel>> GetFourMonthesSumData()
        {
            var methodName = nameof(GetFourMonthesSumData);

            try
            {
                var result = await (from record in _context.GroupedTinklas
                                    group record by record.Tinklas into groupResult
                                    select (new GroupedTinklasModel
                                    {
                                        Tinklas = groupResult.Key,
                                        PPlusSum = groupResult.Sum(x => x.PPlusSum),
                                        PMinusSum = groupResult.Sum(x => x.PMinusSum)
                                    })).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return null;
            }
        }
    }
}
