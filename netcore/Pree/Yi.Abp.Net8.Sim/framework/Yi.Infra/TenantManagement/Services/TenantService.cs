using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MiniExcelLibs;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.TenantManagement.Dtos;
using Yi.Infra.TenantManagement.Entities;

namespace Yi.Infra.TenantManagement.Services;

[RemoteService(false)]
public class TenantService : ApplicationService, ITenantService
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ISqlSugarRepository<TenantAggregateRoot, Guid> _repository;

    public TenantService(ISqlSugarRepository<TenantAggregateRoot, Guid> repository, IDataSeeder dataSeeder)
    {
        _repository = repository;
        _dataSeeder = dataSeeder;
    }

    public async Task<TenantGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<TenantGetOutputDto>();
    }

    public async Task<PagedResultDto<TenantGetListOutputDto>> GetListAsync(TenantGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<TenantGetListOutputDto>(total, entities.Adapt<List<TenantGetListOutputDto>>());
    }

    public async Task<TenantGetOutputDto> CreateAsync(TenantCreateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name))
        {
            throw new UserFriendlyException("创建失败，当前租户已存在");
        }

        var entity = input.Adapt<TenantAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<TenantGetOutputDto>();
    }
    
    public async Task<TenantGetOutputDto> UpdateAsync(Guid id, TenantUpdateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name && x.Id != id))
            throw new UserFriendlyException("更新后租户名已经存在");

        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);
        
        return entity.Adapt<TenantGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public virtual async Task<IActionResult> GetExportExcelAsync(TenantGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        var dirPath = "/wwwroot/temp";

        var fileName = $"{nameof(TenantAggregateRoot)}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_{Guid.NewGuid()}";
        var filePath = $"{dirPath}/{fileName}.xlsx";
        if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

        MiniExcel.SaveAs(filePath, output.Items);
        return new PhysicalFileResult(filePath, "application/vnd.ms-excel");
    }
    
    public async Task<List<TenantSelectOutputDto>> GetSelectAsync()
    {
        var entities = await _repository._DbQueryable.ToListAsync();
        return entities.Select(x => new TenantSelectOutputDto { Id = x.Id, Name = x.Name }).ToList();
    }
    
    /// <summary>
    ///     初始化租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task InitAsync(Guid id)
    {
        await CurrentUnitOfWork.SaveChangesAsync();
        using (CurrentTenant.Change(id))
        {
            await CodeFirst(LazyServiceProvider);
            await _dataSeeder.SeedAsync(id);
        }
    }

    private async Task CodeFirst(IServiceProvider service)
    {
        var moduleContainer = service.GetRequiredService<IModuleContainer>();

        //没有数据库，不能创工作单元，创建库，先关闭
        using (var uow = UnitOfWorkManager.Begin(true, false))
        {
            var db = await _repository.GetDbContextAsync();
            //尝试创建数据库
            db.DbMaintenance.CreateDatabase();

            var types = new List<Type>();
            foreach (var module in moduleContainer.Modules)
                types.AddRange(module.Assembly.GetTypes()
                    .Where(x => x.GetCustomAttribute<IgnoreCodeFirstAttribute>() == null)
                    .Where(x => x.GetCustomAttribute<SugarTable>() != null)
                    .Where(x => x.GetCustomAttribute<DefaultTenantTableAttribute>() is null)
                    .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));

            if (types.Count > 0) db.CodeFirst.InitTables(types.ToArray());

            await uow.CompleteAsync();
        }
    }
}