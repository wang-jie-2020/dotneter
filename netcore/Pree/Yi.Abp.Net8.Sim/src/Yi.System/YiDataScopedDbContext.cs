﻿using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Permissions;
using Yi.System.Domains.Rbac;
using Yi.System.Domains.Rbac.Entities;
using Yi.System.Services.Rbac;

namespace Yi.System;

public class YiDataScopedDbContext : SqlSugarDbContext
{
    public YiDataScopedDbContext(IAbpLazyServiceProvider lazyServiceProvider) : base(lazyServiceProvider)
    {
    }

    protected override void CustomDataFilter(ISqlSugarClient sqlSugarClient)
    {
        if (DataFilter.IsEnabled<IDataPermission>()) DataPermissionFilter(sqlSugarClient);


        base.CustomDataFilter(sqlSugarClient);
    }


    /// <summary>
    ///     数据权限过滤
    /// </summary>
    protected void DataPermissionFilter(ISqlSugarClient sqlSugarClient)
    {
        //获取当前用户的信息
        if (CurrentUser.Id == null || CurrentUser.IsRefreshToken())
        {
            return;
        }
        
        //管理员不过滤
        if (CurrentUser.UserName.Equals(UserConst.Admin) || CurrentUser.Roles.Any(f => f.Equals(UserConst.AdminRolesCode)))
        {
            return;
        }
        
        var expUser = Expressionable.Create<UserEntity>();
        var expRole = Expressionable.Create<RoleEntity>();

        var roleInfo = CurrentUser.GetRoleInfo();

        //如果无岗位，或者无角色，只能看自己的数据
        if ( /*CurrentUser.GetDeptId() is null ||*/ roleInfo is null)
        {
            expUser.Or(it => it.Id == CurrentUser.Id);
            expRole.Or(it => roleInfo.Select(x => x.Id).Contains(it.Id));
        }
        else
        {
            foreach (var role in roleInfo.OrderBy(f => f.DataScope))
            {
                var dataScope = role.DataScope;
                if (DataScopeEnum.ALL.Equals(dataScope)) //所有权限
                {
                    break;
                }

                if (DataScopeEnum.CUSTOM.Equals(dataScope)) //自定数据权限
                {
                    //" OR {}.dept_id IN ( SELECT dept_id FROM sys_role_dept WHERE role_id = {} ) ", deptAlias, role.getRoleId()));

                    expUser.Or(it =>
                        SqlFunc.Subqueryable<RoleDeptEntity>().Where(f => f.DeptId == it.DeptId && f.RoleId == role.Id)
                            .Any());
                }
                else if (DataScopeEnum.DEPT.Equals(dataScope)) //本部门数据
                {
                    expUser.Or(it => it.DeptId == CurrentUser.GetDeptId());
                }
                else if (DataScopeEnum.DEPT_FOLLOW.Equals(dataScope)) //本部门及以下数据
                {
                    //SQl  OR {}.dept_id IN ( SELECT dept_id FROM sys_dept WHERE dept_id = {} or find_in_set( {} , ancestors ) )
                    var allChildDepts = sqlSugarClient.Queryable<DeptEntity>()
                        .ToChildList(it => it.ParentId, CurrentUser.GetDeptId());

                    expUser.Or(it => allChildDepts.Select(f => f.Id).ToList().Contains(it.DeptId ?? Guid.Empty));
                }
                else if (DataScopeEnum.USER.Equals(dataScope)) //仅本人数据
                {
                    expUser.Or(it => it.Id == CurrentUser.Id);
                    expRole.Or(it => roleInfo.Select(x => x.Id).Contains(it.Id));
                }
            }
        }
        
        sqlSugarClient.QueryFilter.AddTableFilter(expUser.ToExpression());
        sqlSugarClient.QueryFilter.AddTableFilter(expRole.ToExpression());
    }
}