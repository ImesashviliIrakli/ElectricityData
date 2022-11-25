using Enitites;

namespace Repositories.ElectricityRepositories
{
    public interface IElectricityRepository
    {
        public Task<List<AggregatedData>> GetAll();
        public Task<List<AggregatedData>> GetByMonth(int month);
    }
}
