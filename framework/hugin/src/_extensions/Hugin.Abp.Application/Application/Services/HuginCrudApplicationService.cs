using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hugin.Application.Dtos;
using Hugin.Application.Filters;
using Hugin.Domain.Entities;
using Hugin.Infrastructure.Extensions;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Hugin.Application.Services
{
    /*
     *  ConfigureAwait(false)
     *    讨论了下，虽然ConfigureAwait会有提升性能的作用，但实际上很难界定这个值不值得去大量的应用
     *    也是很难保证全部地方都同步应用，也就不考虑在接下来的模板中再继续了
     *    
     *  RemoteService(IsEnabled = false, IsMetadataEnabled = false)
     *    限定引起数据变化的操作不再默认开放
     *    派生类中可进行重写开放和权限
     */

    /// <summary>
    /// crud的application基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TEntityDto">实体输出类型</typeparam>
    /// <typeparam name="TGetListInput">实体查询输入</typeparam>
    /// <typeparam name="TEntityEditDto">实体编辑类型</typeparam>
    /// <typeparam name="TGetEditOutputDto">实体编辑输出类型</typeparam>
    /// <typeparam name="TCreateOrUpdateInput">实体新建或更新输入</typeparam>

    public abstract class HuginCrudApplicationService<TEntity, TKey, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput>
           : HuginCrudApplicationService<TEntity, TKey, TEntityDto, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput, TCreateOrUpdateInput>
    where TKey : struct
    where TEntity : class, IEntity<TKey>
    where TEntityEditDto : new()
    where TGetEditOutputDto : new()
    {
        protected HuginCrudApplicationService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// crud的application基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TGetOutputDto">实体输出类型</typeparam>
    /// <typeparam name="TGetListOutputDto">实体列表输出类型</typeparam>
    /// <typeparam name="TGetListInput">实体查询输入</typeparam>
    /// <typeparam name="TEntityEditDto">实体编辑类型</typeparam>
    /// <typeparam name="TGetEditOutputDto">实体编辑输出类型</typeparam>
    /// <typeparam name="TCreateInput">实体新建输入</typeparam>
    /// <typeparam name="TUpdateInput">实体更新输入</typeparam>

    public abstract class HuginCrudApplicationService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateInput, TUpdateInput>
        : HuginApplicationService,
            ICrudAppService<TEntity, TGetOutputDto, TGetListOutputDto, TGetEditOutputDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
            where TKey : struct
            where TEntity : class, IEntity<TKey>
            where TEntityEditDto : new()
            where TGetEditOutputDto : new()
    {
        /// <summary>
        /// 实体仓储
        /// </summary>
        protected readonly IRepository<TEntity, TKey> Repository;

        protected HuginCrudApplicationService(IRepository<TEntity, TKey> repository)
        {
            Repository = repository;
        }

        #region Query

        /// <summary>
        /// Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TGetOutputDto> GetAsync(TKey id)
        {
            if (typeof(TGetOutputDto).IsAssignableTo<IEntity<TKey>>())
            {
                var query = MapperAccessor.Mapper.ProjectTo<TGetOutputDto>(CreateDefaultQuery());
                return await AsyncExecuter.FirstOrDefaultAsync(query, e => ((IEntity<TKey>)e).Id.Equals(id));
            }
            if (typeof(TGetOutputDto).IsAssignableTo<IEntityDto<TKey>>())
            {
                var query = MapperAccessor.Mapper.ProjectTo<TGetOutputDto>(CreateDefaultQuery());
                return await AsyncExecuter.FirstOrDefaultAsync(query, e => ((IEntityDto<TKey>)e).Id.Equals(id));
            }

            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input)
        {
            #region Obsolete

            //var query = (IQueryable<TEntity>)Repository;
            //query = ApplyFiltering(query, input);

            //var totalCount = await AsyncExecuter.CountAsync(query);

            //query = ApplySorting(query, input);
            //query = ApplyPaging(query, input);

            //var entities = await AsyncExecuter.ToListAsync(query);
            //var entityDtos = ObjectMapper.Map<List<TEntity>, List<TGetListOutputDto>>(entities);

            //return new PagedResultDto<TGetListOutputDto>(
            //    totalCount,
            //    entityDtos
            //);

            #endregion

            var query = MapperAccessor.Mapper.ProjectTo<TGetListOutputDto>(CreateDefaultQuery());

            query = ApplyFiltering(query, input);

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entityDtos = await AsyncExecuter.ToListAsync(query);

            return new PagedResultDto<TGetListOutputDto>(
                totalCount,
                entityDtos
            );
        }

        /// <summary>
        /// Default Query to override
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<object> CreateDefaultQuery()
        {
            return ApplyDefaultSorting(Repository);
        }

        /// <summary>
        /// Expression Filter With QueryFilterField Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="AbpException"></exception>
        protected virtual IQueryable<T> ApplyFiltering<T>(IQueryable<T> query, TGetListInput input)
        {
            if (!(input is FilterPagedAndSortedResultRequestDto requestDto))
            {
                return query;
            }

            if (requestDto.Filter.IsNullOrWhiteSpace())
            {
                return query;
            }

            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public |
                                                System.Reflection.BindingFlags.Instance |
                                                System.Reflection.BindingFlags.DeclaredOnly)
                .Where(p => p.PropertyType == typeof(string))
                .Where(p => p.IsDefined(typeof(QueryFilterFieldAttribute), true))
                .ToArray();

            if (props.IsEmpty())
            {
                return query;
            }

            Expression filterExpression = null;

            ParameterExpression param = Expression.Parameter(typeof(T));
            ConstantExpression constant = Expression.Constant(requestDto.Filter, typeof(string));
            foreach (var prop in props)
            {
                MemberExpression body = Expression.Property(param, prop);
                MethodCallExpression methodCall = Expression.Call(body,
                    typeof(string).GetMethod("Contains", new Type[] { typeof(string) }) ?? throw new AbpException(),
                    constant);

                filterExpression = filterExpression == null
                    ? (Expression)methodCall
                    : Expression.Or(filterExpression, methodCall);
            }

            if (filterExpression == null)
            {
                return query;
            }

            return query.Where(Expression.Lambda<Func<T, bool>>(filterExpression, param));
        }

        protected virtual IQueryable<T> ApplySorting<T>(IQueryable<T> query, TGetListInput input)
        {
            if (input is ISortedResultRequest sortInput)
            {
                if (!sortInput.Sorting.IsNullOrWhiteSpace())
                {
                    return query.OrderBy(sortInput.Sorting);
                }
            }

            if (input is ILimitedResultRequest)
            {
                return ApplyDefaultSorting(query);
            }

            return query;
        }

        protected virtual IQueryable<T> ApplyDefaultSorting<T>(IQueryable<T> query)
        {
            if (typeof(T).IsAssignableTo<IHasCreationTime>())
            {
                return query.OrderByDescending(e => ((IHasCreationTime)e).CreationTime);
            }

            if (typeof(T).IsAssignableTo<IEntity<TKey>>() && typeof(TKey).IsNumeric())
            {
                return query.OrderByDescending(e => ((IEntity<TKey>)e).Id);
            }

            if (typeof(T).IsAssignableTo<IEntityDto<TKey>>() && typeof(TKey).IsNumeric())
            {
                return query.OrderByDescending(e => ((IEntityDto<TKey>)e).Id);
            }

            return query;
        }

        protected virtual IQueryable<T> ApplyPaging<T>(IQueryable<T> query, TGetListInput input)
        {
            if (input is IPagedResultRequest pagedInput)
            {
                return query.PageBy(pagedInput);
            }

            if (input is ILimitedResultRequest limitedInput)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            return query;
        }

        #endregion

        #region Edit

        /// <summary>
        /// 编辑查询
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public virtual async Task<TGetEditOutputDto> GetForEdit(TKey? key)
        {
            var output = new TGetEditOutputDto();

            if (key.HasValue)
            {
                var entity = await Repository.GetAsync(key.Value);
                if (output is Dtos.IHoused<TEntityEditDto> housedOutput)
                {
                    housedOutput.Item = ObjectMapper.Map<TEntity, TEntityEditDto>(entity);
                }
                else
                {
                    output = ObjectMapper.Map<TEntity, TGetEditOutputDto>(entity);
                }
            }
            else
            {
                if (output is Dtos.IHoused<TEntityEditDto> housedOutput)
                {
                    housedOutput.Item = new TEntityEditDto();
                }
            }

            return output;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public virtual async Task<TGetOutputDto> CreateAsync(TCreateInput input)
        {
            TEntity entity;
            if (input is Dtos.IHoused<TEntityEditDto> housedInput)
            {
                entity = ObjectMapper.Map<TEntityEditDto, TEntity>(housedInput.Item);
            }
            else
            {
                entity = ObjectMapper.Map<TCreateInput, TEntity>(input);
            }
            SetIdForGuids(entity);
            TryToSetTenantId(entity);
            TryToSetUserId(entity);
            await Repository.InsertAsync(entity, autoSave: true);
            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public virtual async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input)
        {
            var entity = await Repository.GetAsync(id);
            if (input is Dtos.IHoused<TEntityEditDto> housedInput)
            {
                ObjectMapper.Map(housedInput.Item, entity);
            }
            else
            {
                ObjectMapper.Map(input, entity);
            }

            await Repository.UpdateAsync(entity, autoSave: true);
            return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public virtual async Task DeleteAsync(TKey id)
        {
            await Repository.DeleteAsync(id);
        }

        protected virtual void SetIdForGuids(TEntity entity)
        {
            if (entity is IEntity<Guid> entityWithGuidId && entityWithGuidId.Id == Guid.Empty)
            {
                EntityHelper.TrySetId(entityWithGuidId, () => GuidGenerator.Create(), true);
            }
        }

        protected virtual void TryToSetTenantId(TEntity entity)
        {
            if (entity is IMultiTenant multiTenant && HasTenantIdProperty(entity) && multiTenant.TenantId == null)
            {
                var tenantId = CurrentTenant.Id;

                if (!tenantId.HasValue)
                {
                    return;
                }

                var propertyInfo = entity.GetType().GetProperty(nameof(IMultiTenant.TenantId));

                if (propertyInfo == null || propertyInfo.GetSetMethod(true) == null)
                {
                    return;
                }

                propertyInfo.SetValue(entity, tenantId);
            }
        }

        protected virtual bool HasTenantIdProperty(TEntity entity)
        {
            return entity.GetType().GetProperty(nameof(IMultiTenant.TenantId)) != null;
        }

        protected virtual void TryToSetUserId(TEntity entity)
        {
            if (entity is IMultiUser user && HasUserIdProperty(entity) && user.UserId == Guid.Empty)
            {
                var userId = CurrentUser.Id;

                if (userId == Guid.Empty)
                {
                    return;
                }

                var propertyInfo = entity.GetType().GetProperty(nameof(IMultiUser.UserId));

                if (propertyInfo == null || propertyInfo.GetSetMethod(true) == null)
                {
                    return;
                }

                propertyInfo.SetValue(entity, userId);
            }
        }

        protected virtual bool HasUserIdProperty(TEntity entity)
        {
            return entity.GetType().GetProperty(nameof(IMultiUser.UserId)) != null;
        }

        #endregion
    }
}
