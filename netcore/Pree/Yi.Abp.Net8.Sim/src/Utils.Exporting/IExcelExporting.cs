using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Utils.Exporting
{
    public interface IExcelExporting
    {
        Task<byte[]> ExportAsync<T>(List<T> data) where T : class, new();

        Task<byte[]> ExportTemplateAsync<T>() where T : class, new();

        Task<List<T>> Import<T>(Stream stream) where T : class, new();
    }
}
