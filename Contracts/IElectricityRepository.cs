using Enitites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IElectricityRepository
    {
        public Task<List<AggregatedData>> GetAll();
        public Task<List<AggregatedData>> GetByMonth(int month);
        public Task<bool> Add(List<AggregatedData> data);
    }
}
