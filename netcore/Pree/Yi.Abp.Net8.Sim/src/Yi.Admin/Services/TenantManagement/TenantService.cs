using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Uow;
using Yi.Admin.Domains.TenantManagement.Entities;
using Yi.Admin.Services.TenantManagement.Dtos;
using Yi.AspNetCore.Helpers;

namespace Yi.Admin.Services.TenantManagement;

public class TenantService : ApplicationService, ITenantService
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ISqlSugarRepository<TenantAggregateRoot, Guid> _repository;

    public TenantService(ISqlSugarRepository<TenantAggregateRoot, Guid> repository, IDataSeeder dataSeeder)
    {
        _repository = repository;
        _dataSeeder = dataSeeder;
    }

    public async Task<TenantDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<TenantDto>();
    }

    public async Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<TenantDto>(total, entities.Adapt<List<TenantDto>>());
    }

    public async Task<TenantDto> CreateAsync(TenantCreateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name))
        {
            throw new UserFriendlyException("创建失败，当前租户已存在");
        }

        var entity = input.Adapt<TenantAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<TenantDto>();
    }

    public async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name && x.Id != id))
            throw new UserFriendlyException("更新后租户名已经存在");

        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<TenantDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(TenantGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }

    public async Task<List<TenantSelectDto>> GetSelectAsync()
    {
        var entities = await _repository.DbQueryable.ToListAsync();
        return entities.Select(x => new TenantSelectDto { Id = x.Id, Name = x.Name }).ToList();
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