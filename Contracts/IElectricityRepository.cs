using Enitites;

namespace Contracts
{
    public interface IElectricityRepository
    {
        public Task<List<AggregatedData>> GetAll();
        public Task<List<AggregatedData>> GetByMonth(int month);
        public Task<bool> Add(List<AggregatedData> data);
    }
}
