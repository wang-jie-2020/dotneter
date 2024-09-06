﻿using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Abp.Infra.Rbac.Dtos.LoginLog;
using Yi.Abp.Infra.Rbac.Entities;
using Yi.Framework.Ddd.Application;
using Yi.Framework.SqlSugarCore.Abstractions;

namespace Yi.Abp.Infra.Rbac.Services.RecordLog
{
    public class LoginLogService : YiCrudAppService<LoginLogAggregateRoot, LoginLogGetListOutputDto, Guid, LoginLogGetListInputVo>
    {
        private readonly ISqlSugarRepository<LoginLogAggregateRoot> _repository;
        public LoginLogService(ISqlSugarRepository<LoginLogAggregateRoot, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public override async Task<PagedResultDto<LoginLogGetListOutputDto>> GetListAsync(LoginLogGetListInputVo input)
        {
            RefAsync<int> total = 0;

            var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.LoginIp), x => x.LoginIp.Contains(input.LoginIp!))
                          .WhereIF(!string.IsNullOrEmpty(input.LoginUser), x => x.LoginUser!.Contains(input.LoginUser!))
                          .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
                          .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
            return new PagedResultDto<LoginLogGetListOutputDto>(total, await MapToGetListOutputDtosAsync(entities));
        }

        [RemoteService(false)]
        public override Task<LoginLogGetListOutputDto> UpdateAsync(Guid id, LoginLogGetListOutputDto input)
        {
            return base.UpdateAsync(id, input);
        }
    }
}
