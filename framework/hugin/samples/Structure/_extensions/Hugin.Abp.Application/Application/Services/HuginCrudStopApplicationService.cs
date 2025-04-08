using System;
using System.Linq;
using System.Threading.Tasks;
using Hugin.Application.Dtos;
using Hugin.Domain.Entities;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Hugin.Application.Services
{
    /// <summary>
    ///  stop的application基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TEntityDto">实体输出类型</typeparam>
    /// <typeparam name="TGetListInput">实体查询输入</typeparam>
    /// <typeparam name="TEntityEditDto">实体编辑类型</typeparam>
    /// <typeparam name="TGetEditOutputDto">实体编辑输出类型</typeparam>
    /// <typeparam name="TCreateOrUpdateInput">实体新建或更新输入</typeparam>
    public abstract class HuginCrudStopApplicationService<TEntity, TKey, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput>
        : HuginCrudStopApplicationService<TEntity, TKey, TEntityDto, TEntityDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateOrUpdateInput, TCreateOrUpdateInput>
    where TKey : struct
    where TEntity : class, IEntity<TKey>, IStopAudited
    where TEntityEditDto : new()
    where TGetEditOutputDto : new()
    {
        protected HuginCrudStopApplicationService(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }
    }

    /// <summary>
    /// stop的application基类
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
    public abstract class HuginCrudStopApplicationService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateInput, TUpdateInput>
        : HuginCrudApplicationService<TEntity, TKey, TGetOutputDto, TGetListOutputDto, TGetListInput, TEntityEditDto, TGetEditOutputDto, TCreateInput, TUpdateInput>
            , IStopAppService<TKey>
    where TKey : struct
    where TEntity : class, IEntity<TKey>, IStopAudited
    where TEntityEditDto : new()
    where TGetEditOutputDto : new()
    {
        protected HuginCrudStopApplicationService(IRepository<TEntity, TKey> repository) : base(repository)
        {

        }

        /*
         * 不具备通用性
         */
        //protected override IQueryable<TEntity> CreateDefaultQuery()
        //{
        //    return base.CreateDefaultQuery().Where(p => p.IsStop == false);
        //}

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public virtual async Task Stop(TKey id)
        {
            var entity = await Repository.GetAsync(id);
            entity.IsStop = true;
            entity.StopUserId = CurrentUser?.Id;
            entity.StopTime = Clock.Now;
            await Repository.UpdateAsync(entity, autoSave: true);
        }

        /// <summary>
        /// 取消停用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
        public virtual async Task CancelStop(TKey id)
        {
            var entity = await Repository.GetAsync(id);
            entity.IsStop = false;
            entity.StopUserId = null;
            entity.StopTime = null;
            await Repository.UpdateAsync(entity, autoSave: true);
        }

        protected override IQueryable<T> ApplyFiltering<T>(IQueryable<T> query, TGetListInput input)
        {
            query = base.ApplyFiltering(query, input);

            if (input is StopFilterPagedAndSortedResultRequestDto requestDto)
            {
                if (requestDto.IsStop != null)
                {
                    if (typeof(T).IsAssignableTo<IStop>())
                    {
                        query = query.Where(p => ((IStop)p).IsStop.Equals(requestDto.IsStop.Value));
                    }
                }
            }

            return query;
        }
    }
}