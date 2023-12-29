using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hugin.Infrastructure.Exporting;
using Hugin.Infrastructure.Exporting.Magicodes;
using Magicodes.ExporterAndImporter.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("Excel")]
    public class ExcelAppService : BaseAppService
    {
        private readonly IExcelExporting _excelExport;

        public ExcelAppService(IExcelExporting excelExport)
        {
            _excelExport = excelExport;
        }

        [HttpGet]
        public async Task<FileContentResult> Export()
        {
            var list = new List<ExportExcelData>
            {
                new ExportExcelData
                {
                     Name = "John Wick",
                     Age = 20,
                     Gender = Gender.Male,
                     Birthday = DateTime.Parse("2020/01/03")
                },
                new ExportExcelData
                {
                    Name = "Sakura",
                    Age = 16,
                    Gender = Gender.Female
                }
            };

            var buffer = await _excelExport.ExportAsync(list);
            return new FileContentResult(buffer, System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = "export_sample.xlsx"
            };
        }

        [HttpGet]
        public async Task<FileContentResult> ImportTemplate()
        {
            var buffer = await _excelExport.ExportTemplateAsync<ExportExcelData>();

            return new FileContentResult(buffer, System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = "import_sample.xlsx"
            };
        }

        [HttpPost]
        public async Task<object> Import(IFormFile formFile)
        {
            var list = await _excelExport.ImportTemplateAsync<ExportExcelData>(formFile.OpenReadStream());
            return list;
        }
    }

    /*
     *  1.display -- DisplayName Enum.Description (本地化?) 
     *  2.table style -- （1）标注可行 （2）在通常情景不要蔓延较好 （3）重复，可以在导出时默认增加
     *  3.validate -- （1）支持
     */
    public class ExportExcelData : DefaultExcelStyle
    {
        [DisplayName("姓名")]
        [MaxLength(10)]
        [Required]
        public string Name { get; set; }

        [DisplayName("年龄")]
        [Range(10, 80)]
        public int Age { get; set; }

        [DisplayName("性别")]
        public Gender Gender { get; set; }

        [ExporterHeader(Format = "yyyy-MM-dd HH:mm:ss")]
        public DateTime Birthday { get; set; } = DateTime.Now;

        [ExporterHeader(Format = "yyyy-MM-dd")]
        public DateTime Time1 { get; set; }

        [ExporterHeader(Format = "yyyy-MM-dd HH:mm:ss")]
        public DateTime? Time2 { get; set; }

        [ExporterHeader(Width = 100)]
        public DateTime Time3 { get; set; }

        [DataType(DataType.Date)]   //无效
        public DateTime Time4 { get; set; }

        public List<ExportExcelData> List { get; set; } = new List<ExportExcelData>();
    }

    /*
     * 枚举的Description 可以在excel体现
     */
    public enum Gender
    {
        [Description("女")]
        Female,
        [Description("男")]
        Male
    }
}
