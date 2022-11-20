﻿using CsvHelper;
using CsvHelper.Configuration;
using ElectricityData.Data;
using ElectricityData.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;

namespace ElectricityData.Repositories
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

        public async Task<Stream> GetStreamMay()
        {
            var methodName = nameof(GetStreamMay);
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(200);
                string url = "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv";

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

        public async Task<Stream> GetStreamApril()
        {
            var methodName = nameof(GetStreamApril);
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(200);
                string url = "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv";

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

        public async Task<Stream> GetStreamMarch()
        {
            var methodName = nameof(GetStreamMarch);
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(200);
                string url = "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv";

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

        public async Task<Stream> GetStreamFebruary()
        {
            var methodName = nameof(GetStreamFebruary);

            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(200);
                string url = "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv";

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

                if (groupedRecords == null)
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
    }
}
