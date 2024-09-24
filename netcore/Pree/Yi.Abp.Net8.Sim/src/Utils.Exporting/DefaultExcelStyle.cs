using Magicodes.ExporterAndImporter.Excel;

namespace Utils.Exporting
{
    [ExcelExporter(TableStyle = OfficeOpenXml.Table.TableStyles.Light10, AutoCenter = true, AutoFitAllColumn = true)]
    [ExcelImporter(IsLabelingError = true)]
    public abstract class DefaultExcelStyle
    {
    }
}
