using Magicodes.ExporterAndImporter.Excel.Utility;

namespace Utils.Exporting.Internal
{
    public class InternalExcelExportHelper<T> : ExportHelper<T> where T : class, new()
    {
        //todo 测试一下如果存在复杂类型时是否会有问题
        // protected override List<PropertyInfo> SortedProperties
        // {
        //     get
        //     {
        //         var type = typeof(T);
        //         var objProperties = type.GetProperties()
        //             .OrderBy(p => p.GetAttribute<ExporterHeaderAttribute>()?.ColumnIndex ?? 10000).ToList();
        //
        //         //exclude complex type
        //         objProperties = objProperties
        //             .Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string))
        //             .ToList();
        //
        //         return objProperties;
        //     }
        // }
    }
}