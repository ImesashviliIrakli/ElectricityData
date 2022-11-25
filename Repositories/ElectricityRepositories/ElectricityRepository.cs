﻿using CsvHelper.Configuration;
using CsvHelper;
using Enitites;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Enitites.Data;

namespace Repositories.ElectricityRepositories
{
    public class ElectricityRepository : IElectricityRepository
    {
        #region Injection
        private readonly AppDbContext _context;
        private readonly ILogger<ElectricityRepository> _logger;
        public ElectricityRepository(
            AppDbContext context,
            ILogger<ElectricityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        #endregion

        public async Task<List<AggregatedData>> GetByMonth(int month)
        {
            string methodName = nameof(GetByMonth);

            try
            {
                List<AggregatedData> result = await _context.AggregatedData.Where(x => x.Date.Month == month).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<AggregatedData>> GetAll()
        {
            string methodName = nameof(GetAll);

            try
            {
                List<AggregatedData> result = await (from record in _context.AggregatedData
                                                          group record by record.Tinklas into groupResult
                                                          select (new AggregatedData
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
