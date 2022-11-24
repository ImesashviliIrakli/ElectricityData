using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DownloadDataRepositories
{
    public interface IDownloadDataRepository
    {
        public Task<bool> DownloadData(string month);
    }
}
