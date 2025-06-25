using System.Linq.Expressions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SqlSugar;

namespace Yi.Framework.SqlSugarCore.Repositories;

public class SqlSugarRepository<TEntity> : SimpleClient<TEntity>, ISqlSugarRepository<TEntity>
    where TEntity : class, new()
{
    private readonly ISugarDbContextProvider<ISqlSugarDbContext> _sugarDbContextProvider;

    public SqlSugarRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider)
    {
        _sugarDbContextProvider = sugarDbContextProvider;
        Context = _sugarDbContextProvider.GetDbContextAsync().Result.SqlSugarClient;
    }

    public override bool Delete(Expression<Func<TEntity, bool>> whereExpression)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return AsUpdateable()
                .SetColumns(nameof(ISoftDelete), true)
                .Where(whereExpression)
                .ExecuteCommand() > 0;
        return base.Delete(whereExpression);
    }

    public override bool Delete(TEntity deleteObj)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            ObjectHelper.TrySetProperty(((ISoftDelete)deleteObj), x => x.IsDeleted, () => true);
            return Update(deleteObj);
        }

        return base.Delete(deleteObj);
    }

    public override bool Delete(List<TEntity> deleteObjs)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            deleteObjs.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return UpdateRange(deleteObjs);
        }

        return base.Delete(deleteObjs);
    }

    public override bool DeleteById(dynamic id)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = GetById(id);
            ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
            return Update(entity);
        }

        return Context.Deleteable<T>().In(id).ExecuteCommand() > 0;
    }

    public override bool DeleteByIds(dynamic[] ids)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = AsQueryable().In(ids).ToList();
            if (entities.Count == 0) return false;
            entities.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return UpdateRange(entities);
        }

        return base.DeleteByIds(ids);
    }

    public override async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            return await AsUpdateable()
                .SetColumns(nameof(ISoftDelete), true)
                .Where(whereExpression)
                .ExecuteCommandAsync() > 0;
        return await base.DeleteAsync(whereExpression);
    }

    public override async Task<bool> DeleteAsync(TEntity deleteObj)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            ObjectHelper.TrySetProperty(((ISoftDelete)deleteObj), x => x.IsDeleted, () => true);
            return await UpdateAsync(deleteObj);
        }

        return await base.DeleteAsync(deleteObj);
    }

    public override async Task<bool> DeleteAsync(List<TEntity> deleteObjs)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            deleteObjs.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return await UpdateRangeAsync(deleteObjs);
        }

        return await base.DeleteAsync(deleteObjs);
    }
    
    public override async Task<bool> DeleteByIdAsync(dynamic id)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entity = await GetByIdAsync(id);
            ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
            return await UpdateAsync(entity);
        }

        return await Context.Deleteable<T>().In(id).ExecuteCommand() > 0;
    }

    public override async Task<bool> DeleteByIdsAsync(dynamic[] ids)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            var entities = await AsQueryable().In(ids).ToListAsync();
            if (entities.Count == 0) return false;
            entities.ForEach(e => ObjectHelper.TrySetProperty(((ISoftDelete)e), x => x.IsDeleted, () => true));
            return await UpdateRangeAsync(entities);
        }

        return await base.DeleteByIdsAsync(ids);
    }
}

public class SqlSugarRepository<TEntity, TKey> : SqlSugarRepository<TEntity>, ISqlSugarRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>, new()
{
    public SqlSugarRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider) : base(
        sugarDbContextProvider)
    {
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        await DeleteByIdsAsync(ids.Select(x => (object)x).ToArray());
    }
}