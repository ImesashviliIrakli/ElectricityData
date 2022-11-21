using ElectricityData.Models;
using Microsoft.AspNetCore.Components.Web;

namespace ElectricityData.Repositories
{
    public interface IElectricityRepository
    {
        public Task<Stream> GetStreamMay();
        public Task<Stream> GetStreamApril();
        public Task<Stream> GetStreamMarch();
        public Task<Stream> GetStreamFebruary();
        public Task<IEnumerable<GroupedTinklasModel>> Add(Stream stream);
        public Task<List<GroupedTinklasModel>> GetFourMonthesSumData();

    }
}
