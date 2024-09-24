using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Excel;
using Utils.Exporting.Internal;

namespace Utils.Exporting
{
    public class ExcelExporting : IExcelExporting
    {
        public async Task<byte[]> ExportAsync<T>(List<T> data) where T : class, new()
        {
            var exporter = new InternalExcelExporter();

            var buffer = await exporter.ExportAsBytes(data);
            return buffer;
        }

        public async Task<byte[]> ExportTemplateAsync<T>() where T : class, new()
        {
            var importer = new InternalExcelImporter();

            byte[] buffer = await importer.GenerateTemplateBytes<T>();
            return buffer;
        }

        public async Task<List<T>> Import<T>(Stream stream) where T : class, new()
        {
            var importer = new ExcelImporter();

            var result = await importer.Import<T>(stream);
            if (result.HasError)
            {
                var sb = new StringBuilder();

                if (result.RowErrors != null && result.RowErrors.Any())
                {
                    foreach (var err in result.RowErrors)
                    {
                        sb.AppendLine($"行:{err.RowIndex}错误---");
                        foreach (var row in err.FieldErrors)
                        {
                            sb.AppendLine($" {row.Key}:{row.Value}");
                        }
                    }
                }

                if (result.Exception != null)
                {
                    sb.AppendLine(result.Exception.Message);
                }

                throw new Exception(sb.ToString());
            }

            return result.Data.ToList();
        }
    }
}