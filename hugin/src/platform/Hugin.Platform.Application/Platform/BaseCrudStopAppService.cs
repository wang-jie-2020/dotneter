using System;
using System.Linq;
using System.Threading.Tasks;
using Hugin.Application.Services;
using Hugin.Domain.Entities;
using Hugin.Infrastructure.Exporting;
using Hugin.Platform.FileObjects;
using Hugin.Platform.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Hugin.Platform
{
    public abstract class BaseCrudStopAppService<TEntity, TKey, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput>
        : BaseCrudStopAppService<TEntity, TKey, TEntityDto, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput, TCreateOrUpdateInput>
    where TKey : struct
    where TEntity : class, IEntity<TKey>, IStopAudited
    where TEntityDto : class, new()
    where TEntityEditDto : new()
    where TGetEditOutputDto : new()
    {
        protected BaseCrudStopAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    [ApiExplorerSettings(GroupName = ApiGroups.Platform)]
    public abstract class BaseCrudStopAppService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateInput, TUpdateInput>
        : HuginCrudStopApplicationService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateInput, TUpdateInput>
    where TKey : struct
    where TEntity : class, IEntity<TKey>, IStopAudited
    where TGetListOutputDto : class, new()
    where TEntityEditDto : new()
    where TGetEditOutputDto : new()
    {
        protected BaseCrudStopAppService(IRepository<TEntity, TKey> repository) : base(repository)
        {
            LocalizationResource = typeof(PlatformResource);
            ObjectMapperContext = typeof(PlatformApplicationModule);
        }

        private IExcelExporting _excelExport;
        protected IExcelExporting ExcelExport => this.LazyGetRequiredService<IExcelExporting>(ref this._excelExport);

        private IFileCacheAppService _fileCacheAppService;
        protected IFileCacheAppService FileCacheApp => this.LazyGetRequiredService<IFileCacheAppService>(ref this._fileCacheAppService);

        public virtual async Task<FileContentResult> GetExcel(TGetListInput input)
        {
            var output = await GetListAsync(input);
            var list = output.Items.ToList();
            var buffer = await ExcelExport.ExportAsync(list);

            //await FileCacheApp.SetFile(new FileCto("123", System.Net.Mime.MediaTypeNames.Application.Octet, buffer));

            return new FileContentResult(buffer, System.Net.Mime.MediaTypeNames.Application.Octet)
            {
                FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"

            };
        }

        //public virtual async Task<FileContentResult> GetImportTemplate()
        //{
        //    /*
        //     * object 替换
        //     */
        //    var buffer = await ExcelExport.ExportTemplateAsync<object>();

        //    return new FileContentResult(buffer, System.Net.Mime.MediaTypeNames.Application.Octet)
        //    {
        //        FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"
        //    };
        //}

        //public virtual async Task ImportExcel(IFormFile formFile)
        //{
        //    /*
        //     * object 替换
        //     */
        //    var items = await ExcelExport.ImportTemplateAsync<object>(formFile.OpenReadStream());
        //}
    }
}