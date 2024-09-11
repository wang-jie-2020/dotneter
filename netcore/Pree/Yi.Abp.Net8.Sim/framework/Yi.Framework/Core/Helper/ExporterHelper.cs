using MiniExcelLibs;

namespace Yi.Framework.Core.Helper;

public static class ExporterHelper
{
    public static string ExportExcel<TEntity>(IEnumerable<TEntity> entities)
    {
        var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "temp");

        var fileName = $"{typeof(TEntity).Name}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_{Guid.NewGuid()}";
        var filePath = $"{dir}/{fileName}.xlsx";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        MiniExcel.SaveAs(filePath, entities);

        return filePath;
    }
}