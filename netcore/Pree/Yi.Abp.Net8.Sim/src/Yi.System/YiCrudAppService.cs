using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Yi.System;

public abstract class YiCrudAppService<TEntity, TEntityDto, TKey>
    : YiCrudAppService<TEntity, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
    protected YiCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {
    }
}

public abstract class YiCrudAppService<TEntity, TEntityDto, TKey, TGetListInput>
    : YiCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TEntityDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
    protected YiCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {
    }
}

public abstract class YiCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput>
    : YiCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
    protected YiCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {
    }
}

public abstract class YiCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : YiCrudAppService<TEntity, TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IEntityDto<TKey>
{
    protected YiCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {
    }
}

public abstract class YiCrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput,
        TUpdateInput>
    : CrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IEntityDto<TKey>
    where TGetListOutputDto : IEntityDto<TKey>
{
    protected YiCrudAppService(IRepository<TEntity, TKey> repository) : base(repository)
    {
    }

    /// <summary>
    ///     多查
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
    {
        List<TEntity>? entites = null;
        
        if (input is IPagedResultRequest pagedInput)
            entites = await Repository.GetPagedListAsync(pagedInput.SkipCount, pagedInput.MaxResultCount, string.Empty);
        else
            entites = await Repository.GetListAsync();
        
        var total = await Repository.GetCountAsync();
        var output = await MapToGetListOutputDtosAsync(entites);
        return new PagedResultDto<TGetListOutputDto>(total, output);
    }

    /// <summary>
    ///     多删
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [RemoteService(true)]
    public virtual async Task DeleteAsync(IEnumerable<TKey> id)
    {
        await Repository.DeleteManyAsync(id);
    }

    /// <summary>
    ///     偷梁换柱
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [RemoteService(false)]
    public override Task DeleteAsync(TKey id)
    {
        return base.DeleteAsync(id);
    }
    
    /// <summary>
    ///     导出excel
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<IActionResult> GetExportExcelAsync(TGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        var dirPath = "/wwwroot/temp";

        var fileName = $"{typeof(TEntity).Name}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_{Guid.NewGuid()}";
        var filePath = $"{dirPath}/{fileName}.xlsx";
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        MiniExcel.SaveAs(filePath, output.Items);
        return new PhysicalFileResult(filePath, "application/vnd.ms-excel");
    }

    /// <summary>
    ///     导入excel
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task PostImportExcelAsync(List<TCreateInput> input)
    {
        throw new NotImplementedException();
    }
}