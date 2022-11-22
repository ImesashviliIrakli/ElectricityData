using Enitites;

namespace Repositories
{
    public interface IElectricityRepository
    {
        public Task<Stream> GetStream(string month);
        public Task<IEnumerable<GroupedTinklasModel>> Add(Stream stream);
        public Task<List<GroupedTinklasModel>> GetFourMonthesSumData();
    }
}
