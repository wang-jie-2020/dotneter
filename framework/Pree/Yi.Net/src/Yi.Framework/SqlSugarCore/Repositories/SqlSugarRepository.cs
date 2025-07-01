using System.Linq.Expressions;
using SqlSugar;
using Volo.Abp.Uow;

namespace Yi.Framework.SqlSugarCore.Repositories;

public class SqlSugarRepository<TEntity> : ISqlSugarRepository<TEntity>, IUnitOfWorkEnabled
    where TEntity : class, new()
{
    private readonly ISugarDbContextProvider<ISqlSugarDbContext> _sugarDbContextProvider;

    public SqlSugarRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider)
    {
        _sugarDbContextProvider = sugarDbContextProvider;
    }

    protected virtual async Task<ISqlSugarClient> GetDbContextAsync()
    {
        var db = (await _sugarDbContextProvider.GetDbContextAsync()).SqlSugarClient;
        return db;
    }

    public ISqlSugarClient Context
    {
        get => GetDbContextAsync().Result;
        set => throw new NotImplementedException();
    }

    protected virtual SimpleClient<TEntity> Client => new SimpleClient<TEntity>(Context);

    #region SimpleClient模块

    public virtual SimpleClient<TEntity> CopyNew()
    {
        return Client.CopyNew();
    }

    public virtual RepositoryType CopyNew<RepositoryType>(IServiceProvider serviceProvider) where RepositoryType : ISugarRepository
    {
        return Client.CopyNew<RepositoryType>(serviceProvider);
    }

    public virtual RepositoryType CopyNew<RepositoryType>() where RepositoryType : ISugarRepository
    {
        return Client.CopyNew<RepositoryType>();
    }

    public virtual SimpleClient<ChangeType> Change<ChangeType>() where ChangeType : class, new()
    {
        return Client.Change<ChangeType>();
    }

    public virtual RepositoryType ChangeRepository<RepositoryType>() where RepositoryType : ISugarRepository
    {
        return Client.ChangeRepository<RepositoryType>();
    }

    public virtual RepositoryType ChangeRepository<RepositoryType>(IServiceProvider serviceProvider) where RepositoryType : ISugarRepository
    {
        return Client.ChangeRepository<RepositoryType>(serviceProvider);
    }

    public virtual IDeleteable<TEntity> AsDeleteable()
    {
        return Client.AsDeleteable();
    }

    public virtual IInsertable<TEntity> AsInsertable(List<TEntity> insertObjs)
    {
        return Client.AsInsertable(insertObjs);
    }

    public virtual IInsertable<TEntity> AsInsertable(TEntity insertObj)
    {
        return Client.AsInsertable(insertObj);
    }

    public virtual IInsertable<TEntity> AsInsertable(TEntity[] insertObjs)
    {
        return Client.AsInsertable(insertObjs);
    }

    public virtual ISugarQueryable<TEntity> AsQueryable()
    {
        return Client.AsQueryable();
    }

    public virtual ISqlSugarClient AsSugarClient()
    {
        return Client.AsSugarClient();
    }

    public virtual ITenant AsTenant()
    {
        return Client.AsTenant();
    }

    public virtual IUpdateable<TEntity> AsUpdateable(List<TEntity> updateObjs)
    {
        return Client.AsUpdateable(updateObjs);
    }

    public virtual IUpdateable<TEntity> AsUpdateable(TEntity updateObj)
    {
        return Client.AsUpdateable(updateObj);
    }

    public virtual IUpdateable<TEntity> AsUpdateable()
    {
        return Client.AsUpdateable();
    }

    public virtual IUpdateable<TEntity> AsUpdateable(TEntity[] updateObjs)
    {
        return Client.AsUpdateable(updateObjs);
    }

    public virtual int Count(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.Count(whereExpression);
    }

    //public virtual bool Delete(Expression<Func<TEntity, bool>> whereExpression)
    //{
    //    return Client.Delete(whereExpression);
    //}

    //public virtual bool Delete(TEntity deleteObj)
    //{
    //    return Client.Delete(deleteObj);
    //}

    //public virtual bool Delete(List<TEntity> deleteObjs)
    //{
    //    return Client.Delete(deleteObjs);
    //}

    //public virtual bool DeleteById(dynamic id)
    //{
    //    return Client.DeleteById(id);
    //}

    //public virtual bool DeleteByIds(dynamic[] ids)
    //{
    //    return Client.DeleteByIds(ids);
    //}

    public virtual TEntity GetById(dynamic id)
    {
        return Client.GetById(id);
    }

    public virtual List<TEntity> GetList()
    {
        return Client.GetList();
    }

    public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.GetList(whereExpression);
    }

    public virtual List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page)
    {
        return Client.GetPageList(whereExpression, page);
    }

    public virtual List<TEntity> GetPageList(Expression<Func<TEntity, bool>> whereExpression, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return Client.GetPageList(whereExpression, page, orderByExpression, orderByType);
    }

    public virtual List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page)
    {
        return Client.GetPageList(conditionalList, page);
    }

    public virtual List<TEntity> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return Client.GetPageList(conditionalList, page, orderByExpression, orderByType);
    }

    public virtual TEntity GetSingle(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.GetSingle(whereExpression);
    }

    public virtual TEntity GetFirst(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.GetFirst(whereExpression);
    }

    public virtual bool Insert(TEntity insertObj)
    {
        return Client.Insert(insertObj);
    }

    public virtual bool InsertOrUpdate(TEntity data)
    {
        return Client.InsertOrUpdate(data);
    }

    public virtual bool InsertOrUpdate(List<TEntity> datas)
    {
        return Client.InsertOrUpdate(datas);
    }

    public virtual bool InsertRange(List<TEntity> insertObjs)
    {
        return Client.InsertRange(insertObjs);
    }

    public virtual bool InsertRange(TEntity[] insertObjs)
    {
        return Client.InsertRange(insertObjs);
    }

    public virtual int InsertReturnIdentity(TEntity insertObj)
    {
        return Client.InsertReturnIdentity(insertObj);
    }

    public virtual long InsertReturnBigIdentity(TEntity insertObj)
    {
        return Client.InsertReturnBigIdentity(insertObj);
    }

    public virtual long InsertReturnSnowflakeId(TEntity insertObj)
    {
        return Client.InsertReturnSnowflakeId(insertObj);
    }

    public virtual List<long> InsertReturnSnowflakeId(List<TEntity> insertObjs)
    {
        return Client.InsertReturnSnowflakeId(insertObjs);
    }

    public virtual TEntity InsertReturnEntity(TEntity insertObj)
    {
        return Client.InsertReturnEntity(insertObj);
    }

    public virtual bool IsAny(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.IsAny(whereExpression);
    }

    public virtual bool Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.Update(columns, whereExpression);
    }

    public virtual bool UpdateSetColumnsTrue(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return Client.UpdateSetColumnsTrue(columns, whereExpression);
    }

    public virtual bool Update(TEntity updateObj)
    {
        return Client.Update(updateObj);
    }

    public virtual bool UpdateRange(List<TEntity> updateObjs)
    {
        return Client.UpdateRange(updateObjs);
    }

    public virtual bool UpdateRange(TEntity[] updateObjs)
    {
        return Client.UpdateRange(updateObjs);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.CountAsync(whereExpression);
    }

    //public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
    //{
    //    return await Client.DeleteAsync(whereExpression);
    //}

    //public virtual async Task<bool> DeleteAsync(TEntity deleteObj)
    //{
    //    return await Client.DeleteAsync(deleteObj);
    //}

    //public virtual async Task<bool> DeleteAsync(List<TEntity> deleteObjs)
    //{
    //    return await Client.DeleteAsync(deleteObjs);
    //}

    //public virtual async Task<bool> DeleteByIdAsync(dynamic id)
    //{
    //    return await Client.DeleteByIdAsync(id);
    //}

    //public virtual async Task<bool> DeleteByIdsAsync(dynamic[] ids)
    //{
    //    return await Client.DeleteByIdsAsync(ids);
    //}

    public virtual async Task<TEntity> GetByIdAsync(dynamic id)
    {
        return await Client.GetByIdAsync(id);
    }

    public virtual async Task<List<TEntity>> GetListAsync()
    {
        return await Client.GetListAsync();
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.GetListAsync(whereExpression);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> whereExpression, PageModel page)
    {
        return await Client.GetPageListAsync(whereExpression, page);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> whereExpression, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return await Client.GetPageListAsync(whereExpression, page, orderByExpression, orderByType);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page)
    {
        return await Client.GetPageListAsync(conditionalList, page);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return await Client.GetPageListAsync(conditionalList, page, orderByExpression, orderByType);
    }

    public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.GetSingleAsync(whereExpression);
    }

    public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.GetFirstAsync(whereExpression);
    }

    public virtual async Task<bool> InsertAsync(TEntity insertObj)
    {
        return await Client.InsertAsync(insertObj);
    }

    public virtual async Task<bool> InsertOrUpdateAsync(TEntity data)
    {
        return await Client.InsertOrUpdateAsync(data);
    }

    public virtual async Task<bool> InsertOrUpdateAsync(List<TEntity> datas)
    {
        return await Client.InsertOrUpdateAsync(datas);
    }

    public virtual async Task<bool> InsertRangeAsync(List<TEntity> insertObjs)
    {
        return await Client.InsertRangeAsync(insertObjs);
    }

    public virtual async Task<bool> InsertRangeAsync(TEntity[] insertObjs)
    {
        return await Client.InsertRangeAsync(insertObjs);
    }

    public virtual async Task<int> InsertReturnIdentityAsync(TEntity insertObj)
    {
        return await Client.InsertReturnIdentityAsync(insertObj);
    }

    public virtual async Task<long> InsertReturnBigIdentityAsync(TEntity insertObj)
    {
        return await Client.InsertReturnBigIdentityAsync(insertObj);
    }

    public virtual async Task<long> InsertReturnSnowflakeIdAsync(TEntity insertObj)
    {
        return await Client.InsertReturnSnowflakeIdAsync(insertObj);
    }

    public virtual async Task<List<long>> InsertReturnSnowflakeIdAsync(List<TEntity> insertObjs)
    {
        return await Client.InsertReturnSnowflakeIdAsync(insertObjs);
    }

    public virtual async Task<TEntity> InsertReturnEntityAsync(TEntity insertObj)
    {
        return await Client.InsertReturnEntityAsync(insertObj);
    }

    public virtual async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.IsAnyAsync(whereExpression);
    }

    public virtual async Task<bool> UpdateSetColumnsTrueAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.UpdateSetColumnsTrueAsync(columns, whereExpression);
    }

    public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Client.UpdateAsync(columns, whereExpression);
    }

    public virtual async Task<bool> UpdateAsync(TEntity updateObj)
    {
        return await Client.UpdateAsync(updateObj);
    }

    public virtual async Task<bool> UpdateRangeAsync(List<TEntity> updateObjs)
    {
        return await Client.UpdateRangeAsync(updateObjs);
    }

    public virtual async Task<bool> UpdateRangeAsync(TEntity[] updateObjs)
    {
        return await Client.UpdateRangeAsync(updateObjs);
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.CountAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.DeleteAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(TEntity deleteObj, CancellationToken cancellationToken)
    {
        return await Client.DeleteAsync(deleteObj, cancellationToken);
    }

    public virtual async Task<bool> DeleteAsync(List<TEntity> deleteObjs, CancellationToken cancellationToken)
    {
        return await Client.DeleteAsync(deleteObjs, cancellationToken);
    }

    public virtual async Task<bool> DeleteByIdAsync(dynamic id, CancellationToken cancellationToken)
    {
        return await Client.DeleteByIdAsync(id, cancellationToken);
    }

    public virtual async Task<bool> DeleteByIdsAsync(dynamic[] ids, CancellationToken cancellationToken)
    {
        return await Client.DeleteByIdsAsync(ids, cancellationToken);
    }

    public virtual async Task<TEntity> GetByIdAsync(dynamic id, CancellationToken cancellationToken)
    {
        return await Client.GetByIdAsync(id, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken)
    {
        return await Client.GetListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.GetListAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> whereExpression, PageModel page, CancellationToken cancellationToken)
    {
        return await Client.GetPageListAsync(whereExpression, page, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> whereExpression, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc, CancellationToken cancellationToken = new CancellationToken())
    {
        return await Client.GetPageListAsync(whereExpression, page, orderByExpression, orderByType, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, CancellationToken cancellationToken)
    {
        return await Client.GetPageListAsync(conditionalList, page, cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetPageListAsync(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc, CancellationToken cancellationToken = new CancellationToken())
    {
        return await Client.GetPageListAsync(conditionalList, page, orderByExpression, orderByType, cancellationToken);
    }

    public virtual async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.GetSingleAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.GetFirstAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<bool> InsertAsync(TEntity insertObj, CancellationToken cancellationToken)
    {
        return await Client.InsertAsync(insertObj, cancellationToken);
    }

    public virtual async Task<bool> InsertOrUpdateAsync(TEntity data, CancellationToken cancellationToken)
    {
        return await Client.InsertOrUpdateAsync(data, cancellationToken);
    }

    public virtual async Task<bool> InsertOrUpdateAsync(List<TEntity> datas, CancellationToken cancellationToken)
    {
        return await Client.InsertOrUpdateAsync(datas, cancellationToken);
    }

    public virtual async Task<bool> InsertRangeAsync(List<TEntity> insertObjs, CancellationToken cancellationToken)
    {
        return await Client.InsertRangeAsync(insertObjs, cancellationToken);
    }

    public virtual async Task<bool> InsertRangeAsync(TEntity[] insertObjs, CancellationToken cancellationToken)
    {
        return await Client.InsertRangeAsync(insertObjs, cancellationToken);
    }

    public virtual async Task<int> InsertReturnIdentityAsync(TEntity insertObj, CancellationToken cancellationToken)
    {
        return await Client.InsertReturnIdentityAsync(insertObj, cancellationToken);
    }

    public virtual async Task<long> InsertReturnBigIdentityAsync(TEntity insertObj, CancellationToken cancellationToken)
    {
        return await Client.InsertReturnBigIdentityAsync(insertObj, cancellationToken);
    }

    public virtual async Task<long> InsertReturnSnowflakeIdAsync(TEntity insertObj, CancellationToken cancellationToken)
    {
        return await Client.InsertReturnSnowflakeIdAsync(insertObj, cancellationToken);
    }

    public virtual async Task<List<long>> InsertReturnSnowflakeIdAsync(List<TEntity> insertObjs, CancellationToken cancellationToken)
    {
        return await Client.InsertReturnSnowflakeIdAsync(insertObjs, cancellationToken);
    }

    public virtual async Task<TEntity> InsertReturnEntityAsync(TEntity insertObj, CancellationToken cancellationToken)
    {
        return await Client.InsertReturnEntityAsync(insertObj, cancellationToken);
    }

    public virtual async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.IsAnyAsync(whereExpression, cancellationToken);
    }

    public virtual async Task<bool> UpdateSetColumnsTrueAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.UpdateSetColumnsTrueAsync(columns, whereExpression, cancellationToken);
    }

    public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken)
    {
        return await Client.UpdateAsync(columns, whereExpression, cancellationToken);
    }

    public virtual async Task<bool> UpdateAsync(TEntity updateObj, CancellationToken cancellationToken)
    {
        return await Client.UpdateAsync(updateObj, cancellationToken);
    }

    public virtual async Task<bool> UpdateRangeAsync(List<TEntity> updateObjs, CancellationToken cancellationToken)
    {
        return await Client.UpdateRangeAsync(updateObjs, cancellationToken);
    }

    public virtual async Task<bool> UpdateRangeAsync(TEntity[] updateObjs, CancellationToken cancellationToken)
    {
        return await Client.UpdateRangeAsync(updateObjs, cancellationToken);
    }

    #endregion

    public virtual bool Delete(Expression<Func<TEntity, bool>> whereExpression)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return AsUpdateable()
                .SetColumns(nameof(ISoftDelete), true)
                .Where(whereExpression)
                .ExecuteCommand() > 0;
        return Client.Delete(whereExpression);
    }

    public virtual bool Delete(TEntity deleteObj)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            ObjectHelper.TrySetProperty(((ISoftDelete)deleteObj), x => x.IsDeleted, () => true);
            return Update(deleteObj);
        }

        return Client.Delete(deleteObj);
    }

    public virtual bool Delete(List<TEntity> deleteObjs)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            deleteObjs.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return UpdateRange(deleteObjs);
        }

        return Client.Delete(deleteObjs);
    }

    public virtual bool DeleteById(dynamic id)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = GetById(id);
            ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
            return Update(entity);
        }

        return Client.DeleteById(id);
    }

    public virtual bool DeleteByIds(dynamic[] ids)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = AsQueryable().In(ids).ToList();
            if (entities.Count == 0) return false;
            entities.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return UpdateRange(entities);
        }

        return Client.DeleteByIds(ids);
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return await AsUpdateable()
                .SetColumns(nameof(ISoftDelete), true)
                .Where(whereExpression)
                .ExecuteCommandAsync() > 0;
        return await Client.DeleteAsync(whereExpression);
    }

    public virtual async Task<bool> DeleteAsync(TEntity deleteObj)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            ObjectHelper.TrySetProperty(((ISoftDelete)deleteObj), x => x.IsDeleted, () => true);
            return await UpdateAsync(deleteObj);
        }

        return await Client.DeleteAsync(deleteObj);
    }

    public virtual async Task<bool> DeleteAsync(List<TEntity> deleteObjs)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            deleteObjs.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return await UpdateRangeAsync(deleteObjs);
        }

        return await Client.DeleteAsync(deleteObjs);
    }

    public virtual async Task<bool> DeleteByIdAsync(dynamic id)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = await GetByIdAsync(id);
            ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
            return await UpdateAsync(entity);
        }

        return await Client.DeleteByIdAsync(id);
    }

    public virtual async Task<bool> DeleteByIdsAsync(dynamic[] ids)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = await AsQueryable().In(ids).ToListAsync();
            if (entities.Count == 0) return false;
            entities.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return await UpdateRangeAsync(entities);
        }

        return await Client.DeleteByIdsAsync(ids);
    }
}