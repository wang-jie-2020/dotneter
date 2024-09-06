using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Abp.Infra.Rbac.Dtos.Dept;
using Yi.Abp.Infra.Rbac.Entities;
using Yi.Abp.Infra.Rbac.IServices;
using Yi.Abp.Infra.Rbac.Repositories;
using Yi.Framework.Ddd.Application;

namespace Yi.Abp.Infra.Rbac.Services.System
{
    /// <summary>
    /// Dept服务实现
    /// </summary>
    public class DeptService : YiCrudAppService<DeptAggregateRoot, DeptGetOutputDto, DeptGetListOutputDto, Guid, DeptGetListInputVo, DeptCreateInputVo, DeptUpdateInputVo>, IDeptService
    {
        private IDeptRepository _deptRepository;
        public DeptService(IDeptRepository deptRepository) : base(deptRepository)
        { _deptRepository = deptRepository; }

        [RemoteService(false)]
        public async Task<List<Guid>> GetChildListAsync(Guid deptId)
        {
            return await _deptRepository.GetChildListAsync(deptId);
        }

        /// <summary>
        /// 通过角色id查询该角色全部部门
        /// </summary>
        /// <returns></returns>
        //[Route("{roleId}")]
        public async Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId)
        {
            var entities = await _deptRepository.GetListRoleIdAsync(roleId);
            return await MapToGetListOutputDtosAsync(entities);
        }

        /// <summary>
        /// 多查
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override async Task<PagedResultDto<DeptGetListOutputDto>> GetListAsync(DeptGetListInputVo input)
        {
            RefAsync<int> total = 0;
            var entities = await _deptRepository._DbQueryable
                           .WhereIF(!string.IsNullOrEmpty(input.DeptName), u => u.DeptName.Contains(input.DeptName!))
                            .WhereIF(input.State is not null, u => u.State == input.State)
                           .OrderBy(u => u.OrderNum, OrderByType.Asc)
                          .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
            return new PagedResultDto<DeptGetListOutputDto>
            {
                Items = await MapToGetListOutputDtosAsync(entities),
                TotalCount = total
            };
        }
    }
}
