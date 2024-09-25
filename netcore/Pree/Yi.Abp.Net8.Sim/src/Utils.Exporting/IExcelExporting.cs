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



// using MiniExcelLibs;
//
// namespace Yi.AspNetCore.Helpers
// {
//     public static class ExporterHelper
//     {
//         public static string ExportExcel<TEntity>(IEnumerable<TEntity> entities)
//         {
//             throw new NotImplementedException();
//             // var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "temp");
//             //
//             // var fileName = $"{typeof(TEntity).Name}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_{Guid.NewGuid()}";
//             // var filePath = $"{dir}/{fileName}.xlsx";
//             // if (!Directory.Exists(dir))
//             // {
//             //     Directory.CreateDirectory(dir);
//             // }
//             //
//             // MiniExcel.SaveAs(filePath, entities);
//             //
//             // return filePath;
//         }
//
//
//         // public static string templatePath = $@"{App.HostEnvironment.ContentRootPath}Template";
//         //
//         // /// <summary>
//         // /// Excel模板导出
//         // /// </summary>
//         // /// <param name="fileName">导出目标路径</param>
//         // /// <param name="templateName">导出模板路径</param>
//         // /// <param name="exportData">导出数据</param>
//         // /// <returns></returns>
//         // public static async Task<FileStreamResult> ExportByTemplate(string fileName, string templateName, dynamic exportData)
//         // {
//         //     var goalPath = $@"{templatePath}\Export\{fileName}.xlsx";
//         //     await MiniExcel.SaveAsByTemplateAsync(goalPath, $@"{templatePath}\{templateName}.xlsx", exportData);
//         //     return new FileStreamResult(new MemoryStream(await File.ReadAllBytesAsync(goalPath)), "application/ms-excel");
//         // }
//         //
//         // /// <summary>
//         // /// Excel导出
//         // /// </summary>
//         // /// <param name="fileName">导出目标路径</param>
//         // /// <param name="exportData">导出数据</param>
//         // /// <returns></returns>
//         // public static async Task<FileStreamResult> Export(string fileName, dynamic exportData)
//         // {
//         //     var goalPath = $@"{templatePath}\Export\{fileName}.xlsx";
//         //     await MiniExcel.SaveAsAsync(goalPath, exportData, overwriteFile: true);
//         //     return new FileStreamResult(new MemoryStream(await File.ReadAllBytesAsync(goalPath)), "application/ms-excel");
//         // }
//         //
//         //
//         // /// <summary>
//         // /// Excel导出
//         // /// </summary>
//         // /// <param name="fileName">导出目标路径</param>
//         // /// <param name="exportData">导出数据</param>
//         // /// <param name="sheetName">导出数据</param>
//         // /// <returns></returns>
//         // public static async Task<FileStreamResult> Export(string fileName, dynamic exportData, string sheetName = "Sheet1")
//         // {
//         //     var goalPath = $@"{templatePath}\Export\{fileName}.xlsx";
//         //     await MiniExcel.SaveAsAsync(goalPath, exportData, true, sheetName, overwriteFile: true);
//         //     return new FileStreamResult(new MemoryStream(await File.ReadAllBytesAsync(goalPath)), "application/ms-excel");
//         // }
//         //
//         // /// <summary>
//         // /// 下载模板
//         // /// </summary>
//         // /// <typeparam name="T"></typeparam>
//         // /// <param name="fileName"></param>
//         // /// <param name="t"></param>
//         // /// <returns></returns>
//         // public static async Task<FileStreamResult> ExportTemplate<T>(string fileName, T t) where T : class
//         // {
//         //     var memoryStream = new MemoryStream();
//         //     await memoryStream.SaveAsAsync(t);
//         //     memoryStream.Seek(0, SeekOrigin.Begin);
//         //     return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
//         //     {
//         //         FileDownloadName = "demo.xlsx"
//         //     };
//         // }
//         //
//         // /// <summary>
//         // /// Excel导入
//         // /// </summary>
//         // /// <typeparam name="T"></typeparam>
//         // /// <param name="fileInput"></param>
//         // /// <param name="RemoveEmpty">是否删除空白行(默认删除)</param>
//         // /// <returns></returns>
//         // public static async Task<List<T>> Import<T>(List<IFormFile> fileInput, bool RemoveEmpty = true) where T : class, new()
//         // {
//         //     if (!fileInput.Any()) throw Oops.Oh(ErrorCodeEnum.Excel1001);
//         //     IFormFile formFile = fileInput[0];
//         //     string goalPath = $@"{templatePath}\Import\{DateTime.Now.ToString("yyyyMMdd")}\";
//         //
//         //     if (!Directory.Exists(goalPath))
//         //     {
//         //         Directory.CreateDirectory(goalPath);
//         //     }
//         //
//         //     string importName = $"{DateTime.Now.ToString("HHmmssfff")}_{formFile.FileName}";
//         //     goalPath = $"{goalPath}{importName}";
//         //
//         //     using (var stream = new FileStream(goalPath, FileMode.Create))
//         //     {
//         //         formFile.CopyTo(stream);
//         //     }
//         //
//         //     var rows = await MiniExcel.QueryAsync<T>(goalPath);
//         //     var datas = rows.ToList();
//         //     if (RemoveEmpty)
//         //     {
//         //         var propers = typeof(T).GetProperties();
//         //         for (int i = datas.Count - 1; i >= 0; i--)
//         //         {
//         //             var obj = datas[i];
//         //             bool isBlank = true;
//         //             foreach (var property in propers)
//         //             {
//         //                 var value = property.GetValue(obj);
//         //                 if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value.ToString() != "0")
//         //                 {
//         //                     isBlank = false;
//         //                     break;
//         //                 }
//         //             }
//         //
//         //             if (isBlank)
//         //             {
//         //                 datas.RemoveAt(i);
//         //             }
//         //         }
//         //     }
//         //
//         //     return datas;
//         // }
//         //
//         // public static async Task<FileStreamResult> Export(string fileName, byte[] data)
//         // {
//         //     var memoryStream = new MemoryStream();
//         //     await memoryStream.WriteAsync(data, 0, data.Length);
//         //     memoryStream.Seek(0, SeekOrigin.Begin);
//         //     return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
//         //     {
//         //         FileDownloadName = "demo.xlsx"
//         //     };
//         // }
//     }
// }
//
//
// //
// // var output = await GetListAsync(input);
// // var list = output.Items.ToList();
// // var buffer = await ExcelExport.ExportAsync(list);
// //
// // //await FileCacheApp.SetFile(new FileCto("123", System.Net.Mime.MediaTypeNames.Application.Octet, buffer));
// //
// // return new FileContentResult(buffer, System.Net.Mime.MediaTypeNames.Application.Octet)
// // {
// //     FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"
// // };
// // }