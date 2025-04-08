using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Excel;

namespace Hugin.Infrastructure.Exporting.Magicodes
{
    public class ExcelExporter : global::Magicodes.ExporterAndImporter.Excel.ExcelExporter
    {
        public Task<byte[]> ExportAsBytes<T>(ICollection<T> dataItems) where T : class, new()
        {
            var helper = new ExcelExportHelper<T>();

            var type = typeof(T);
            var exportAttribute = type.GetCustomAttributes(true).SingleOrDefault(p => p is ExcelExporterAttribute);
            if (exportAttribute == null)
            {
                helper.ExcelExporterSettings = new ExcelExporterAttribute()
                {
                    TableStyle = OfficeOpenXml.Table.TableStyles.Light10,
                    AutoCenter = true
                };
            }

            #region base

            if (helper.ExcelExporterSettings.MaxRowNumberOnASheet > 0 &&
                dataItems.Count > helper.ExcelExporterSettings.MaxRowNumberOnASheet)
            {
                using (helper.CurrentExcelPackage)
                {
                    var sheetCount = dataItems.Count / helper.ExcelExporterSettings.MaxRowNumberOnASheet +
                                     (dataItems.Count % helper.ExcelExporterSettings.MaxRowNumberOnASheet > 0 ? 1 : 0);
                    for (int i = 0; i < sheetCount; i++)
                    {
                        var sheetDataItems = dataItems
                            .Skip(i * helper.ExcelExporterSettings.MaxRowNumberOnASheet)
                            .Take(helper.ExcelExporterSettings.MaxRowNumberOnASheet).ToList();
                        helper.AddExcelWorksheet();
                        helper.Export(sheetDataItems);
                    }

                    return Task.FromResult(helper.CurrentExcelPackage.GetAsByteArray());
                }
            }

            using (var ep = helper.Export(dataItems))
            {
                return Task.FromResult(ep.GetAsByteArray());
            }

            #endregion
        }
    }
}
