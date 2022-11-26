using Contracts;
using CsvHelper.Configuration;
using CsvHelper;
using Enitites.Data;
using Enitites;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DownloadService
{
    public class DownloadManager : IDownloadManager
    {
        #region Injection
        private readonly AppDbContext _context;
        private readonly ILogger<DownloadManager> _logger;
        private readonly IElectricityRepository _repository;
        public DownloadManager(
            AppDbContext context,
            ILogger<DownloadManager> logger,
            IElectricityRepository repository)
        {
            //_client = client;
            _context = context;
            _logger = logger;
            _repository = repository;
        }
        #endregion

        // This method executes the DownloadData() method to download all four month's of data.
        public async Task DownloadAll()
        {
            string methodName = nameof(DownloadAll);
            try
            {
                await DownloadData("10763/2022-02");
                await DownloadData("10764/2022-03");
                await DownloadData("10765/2022-04");
                await DownloadData("10766/2022-05");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
            }
        }

        #region HelperMethods
        // 1) First checks if a selected month's of data already exists in the DB.
        // 2) Secondly executes GetStream() method to download electricity data.
        // 3) Thirdly executes AddToDB() method which adds the data to DB.
        private async Task DownloadData(string month)
        {
            string methodName = nameof(DownloadData);
            try
            {
                // Check if the data already exists in DB.
                char getMonthNumber = month[month.Length - 1];
                int monthNumber = int.Parse(getMonthNumber.ToString());

                List<AggregatedData> groupedRecords = await _context.AggregatedData.Where(x => x.Date.Month == monthNumber).ToListAsync();

                if (groupedRecords.Count != 0)
                {
                    _logger.LogInformation($"{methodName} => Data already exists in the DB");
                    return;
                }

                _logger.LogInformation($"{methodName} => Started downloading data");
                var stream = await GetStream(month);

                if (stream == null)
                {
                    _logger.LogError($"{methodName} => Could not get stream");
                    return;
                }

                bool add = await AddToDB(stream);

                if (!add)
                {
                    _logger.LogError($"{methodName} => Could not add to DB");
                    return;
                }

                _logger.LogInformation($"{methodName} => Data was added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
            }
        }

        // Gets the stream that contains the electricity data.
        private async Task<Stream> GetStream(string month)
        {
            string methodName = nameof(GetStream);
            try
            {
                var _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(200);

                var url = $"https://data.gov.lt/dataset/1975/download/{month}.csv";

                _logger.LogInformation($"{methodName} => Getting stream from url");

                // Download and read the data as a stream.
                HttpResponseMessage response = await _client.GetAsync(url);
                Stream readData = await response.Content.ReadAsStreamAsync();

                _logger.LogInformation($"{methodName} => Successfully got stream for may");

                return readData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return null;
            }
        }

        // Reads the data that was provided by GetStream() method and adds it to DB.
        private async Task<bool> AddToDB(Stream stream)
        {
            string methodName = nameof(AddToDB);

            try
            {
                var streamReader = new StreamReader(stream);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                // Get records from stream.
                var csv = new CsvReader(streamReader, config);
                List<ElectricityData> records = csv.GetRecords<ElectricityData>().Where(x => x.OBT_PAVADINIMAS.Equals("Butas")).ToList();

                // Get grouped data using LINQ.
                List<AggregatedData> groupedData = (from record in records
                                                    group record by record.TINKLAS into groupResult
                                                    select (new AggregatedData
                                                    {
                                                        Tinklas = groupResult.Key,
                                                        PPlusSum = groupResult.Sum(x => x.PPlus),
                                                        PMinusSum = groupResult.Sum(x => x.PMINUS),
                                                        Date = groupResult.Max(x => x.PL_T)
                                                    })).ToList();

                // Add data to DB.
                bool add = await _repository.Add(groupedData);

                return add;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
