using Enitites;

namespace Repositories.ElectricityRepositories
{
    public interface IElectricityRepository
    {
        public Task<List<GroupedTinklasModel>> GetAll();
        public Task<List<GroupedTinklasModel>> GetByMonth(int month);
    }
}
