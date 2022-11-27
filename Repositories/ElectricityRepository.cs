using CsvHelper.Configuration;
using CsvHelper;
using Enitites;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Enitites.Data;
using Contracts;

namespace Repositories
{
    public class ElectricityRepository : IElectricityRepository
    {
        #region Injection
        private readonly AppDbContext _context;
        private readonly ILoggerManager _logger;
        public ElectricityRepository(
            AppDbContext context,
            ILoggerManager logger)
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
    
        public async Task<bool> Add(List<AggregatedData> data)
        {
            string methodName = nameof(Add);
            try
            {
                await _context.AggregatedData.AddRangeAsync(data);
                await _context.SaveChangesAsync();

                _logger.LogInfo($"{methodName} => Successfully added to DB");
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"{methodName} => Exception: {ex.Message}");
                return false;
            }
        }
    }
}
