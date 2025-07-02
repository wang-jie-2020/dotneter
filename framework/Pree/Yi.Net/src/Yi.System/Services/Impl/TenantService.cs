using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MiniExcelLibs;
using Volo.Abp.Uow;
using Yi.AspNetCore;
using Yi.AspNetCore.Data.Seeding;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class TenantService : BaseService, ITenantService
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ISqlSugarRepository<TenantEntity> _repository;

    public TenantService(ISqlSugarRepository<TenantEntity> repository, IDataSeeder dataSeeder)
    {
        _repository = repository;
        _dataSeeder = dataSeeder;
    }

    public async Task<TenantDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<TenantDto>();
    }

    public async Task<PagedResult<TenantDto>> GetListAsync(TenantGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResult<TenantDto>(total, entities.Adapt<List<TenantDto>>());
    }

    public async Task<TenantDto> CreateAsync(TenantCreateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name))
        {
            throw Oops.Oh(SystemErrorCodes.Tenant_Exist);
        }

        var entity = input.Adapt<TenantEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<TenantDto>();
    }

    public async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateInput input)
    {
        if (await _repository.IsAnyAsync(x => x.Name == input.Name && x.Id != id))
            throw Oops.Oh(SystemErrorCodes.Tenant_Repeat);

        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<TenantDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }

    public async Task<IActionResult> GetExportExcelAsync(TenantGetListInput input)
    {
        if (input is PagedInput paged)
        {
            paged.PageNum = 0;
            paged.PageSize = int.MaxValue;
        }

        var output = await GetListAsync(input);

        var stream = new MemoryStream();
        await MiniExcel.SaveAsAsync(stream, output.Items);
        stream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(stream, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{L[nameof(TenantEntity)]}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }

    public async Task<List<TenantSelectDto>> GetSelectAsync()
    {
        var entities = await _repository.AsQueryable().ToListAsync();
        return entities.Select(x => new TenantSelectDto { Id = x.Id, Name = x.Name }).ToList();
    }

    /// <summary>
    ///     初始化租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task InitAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    private async Task CodeFirst(IServiceProvider service)
    {
        var moduleContainer = service.GetRequiredService<IModuleContainer>();

        //没有数据库，不能创工作单元，创建库，先关闭
        using (var uow = UnitOfWorkManager.Begin(true, false))
        {
            //尝试创建数据库
            _repository.Context.DbMaintenance.CreateDatabase();

            var types = new List<Type>();
            foreach (var module in moduleContainer.Modules)
                types.AddRange(module.Assembly.GetTypes()
                    .Where(x => x.GetCustomAttribute<IgnoreCodeFirstAttribute>() == null)
                    .Where(x => x.GetCustomAttribute<SugarTable>() != null)
                    .Where(x => x.GetCustomAttribute<DefaultTenantTableAttribute>() is null)
                    .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));

            if (types.Count > 0) _repository.Context.CodeFirst.InitTables(types.ToArray());

            await uow.CompleteAsync();
        }
    }
}