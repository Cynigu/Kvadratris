using System.Collections.Generic;
using System.Threading.Tasks;

namespace lab3.Service
{
    public interface IFileService
    {
        Task<string> OpenAsync(string filename);
        Task SaveAsync(string filename, List<string> dataStringArrayOrigAndResult);
    }
}
