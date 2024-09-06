using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Abp.Infra.Rbac.Dtos.DictionaryType;
using Yi.Abp.Infra.Rbac.Entities;
using Yi.Abp.Infra.Rbac.IServices;
using Yi.Framework.Ddd.Application;
using Yi.Framework.SqlSugarCore.Abstractions;

namespace Yi.Abp.Infra.Rbac.Services
{
    /// <summary>
    /// DictionaryType服务实现
    /// </summary>
    public class DictionaryTypeService : YiCrudAppService<DictionaryTypeAggregateRoot, DictionaryTypeGetOutputDto, DictionaryTypeGetListOutputDto, Guid, DictionaryTypeGetListInputVo, DictionaryTypeCreateInputVo, DictionaryTypeUpdateInputVo>,
       IDictionaryTypeService
    {
        private ISqlSugarRepository<DictionaryTypeAggregateRoot, Guid> _repository;
        public DictionaryTypeService(ISqlSugarRepository<DictionaryTypeAggregateRoot, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public async override Task<PagedResultDto<DictionaryTypeGetListOutputDto>> GetListAsync(DictionaryTypeGetListInputVo input)
        {

            RefAsync<int> total = 0;
            var entities = await _repository._DbQueryable.WhereIF(input.DictName is not null, x => x.DictName.Contains(input.DictName!))
                      .WhereIF(input.DictType is not null, x => x.DictType!.Contains(input.DictType!))
                      .WhereIF(input.State is not null, x => x.State == input.State)
                      .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
                      .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

            return new PagedResultDto<DictionaryTypeGetListOutputDto>
            {
                TotalCount = total,
                Items = await MapToGetListOutputDtosAsync(entities)
            };
        }
    }
}
