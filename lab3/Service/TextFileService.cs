using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace lab3.Service
{
    class TextFileService: IFileService
    {
        public async Task<string> OpenAsync(string filename)
        {
            string arr = "";
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                arr = await sr.ReadToEndAsync();
            }
            return arr;
        }

        public async Task SaveAsync(string filename, List<string> dataStringArrayOrigAndResult)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
            {
                for (int i =0; i < dataStringArrayOrigAndResult.Count; i++)
                {
                    await sw.WriteLineAsync(dataStringArrayOrigAndResult[i]);
                }
            }
            //await Task.Run(() => Thread.Sleep(30000));
        }
    }
}
