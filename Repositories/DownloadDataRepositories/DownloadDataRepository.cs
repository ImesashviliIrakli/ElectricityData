using CsvHelper.Configuration;
using CsvHelper;
using Enitites;
using Enitites.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Repositories.DownloadDataRepositories
{
    public class DownloadDataRepository : IDownloadDataRepository
    {
        #region Injection
        private readonly AppDbContext _context;
        private readonly ILogger<DownloadDataRepository> _logger;
        public DownloadDataRepository(
            AppDbContext context,
            ILogger<DownloadDataRepository> logger)
        {
            //_client = client;
            _context = context;
            _logger = logger;
        }
        #endregion

        //This method downloads the electricity data and adds it to the DB.
        public async Task DownloadAll()
        {
            var methodName = nameof(DownloadLastFourMonths);
            try
            {
                var taskList = new List<Task> {
                DownloadData("10763/2022-02"),
                DownloadData("10764/2022-03"),
                DownloadData("10765/2022-04"),
                DownloadData("10766/2022-05")
                };

                await Task.WhenAll(taskList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
            }
        }

        private async Task DownloadData(string month)
        {
            var methodName = nameof(DownloadData);
            try
            {
                _logger.LogInformation($"{methodName} => Started downloading data");
                var stream = await GetStream(month);

                if (stream == null)
                {
                    _logger.LogError($"{methodName} => Could not get stream");
                    return;
                }

                var add = await AddToDB(stream);

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

        #region HelperMethods
        private async Task<Stream> GetStream(string month)
        {
            var methodName = nameof(GetStream);

            try
            {
                var _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(200);
                var url = $"https://data.gov.lt/dataset/1975/download/" + month + ".csv";

                _logger.LogInformation($"{methodName} => Getting stream from url");

                var response = await _client.GetAsync(url);
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

        private async Task<bool> AddToDB(Stream stream)
        {
            var methodName = nameof(AddToDB);

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
                return true;
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
