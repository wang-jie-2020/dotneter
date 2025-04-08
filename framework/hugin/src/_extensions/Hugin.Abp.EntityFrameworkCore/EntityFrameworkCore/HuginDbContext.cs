using System;
using System.Linq.Expressions;
using Hugin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiStadium;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace Hugin.EntityFrameworkCore
{
    public abstract class HuginDbContext<TDbContext> : AbpDbContext<TDbContext> where TDbContext : DbContext
    {
        public ICurrentUser CurrentUser { get; set; }

        public ICurrentStadium CurrentStadium { get; set; }

        protected virtual Guid? CurrentUserId => CurrentUser?.Id;

        protected virtual Guid? CurrentStadiumId => CurrentStadium?.Id;

        protected virtual bool IsUserFilterEnabled => DataFilter?.IsEnabled<IMultiUser>() ?? false;

        protected virtual bool IsStadiumFilterEnabled => DataFilter?.IsEnabled<IMultiStadium>() ?? false;

        protected HuginDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }

        protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMultiUser).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMultiStadium).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            var expression = base.CreateFilterExpression<TEntity>();

            if (typeof(IMultiUser).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> filter = e => !IsUserFilterEnabled || EF.Property<Guid>(e, "UserId") == CurrentUserId;
                expression = expression == null ? filter : CombineExpressions(expression, filter);
            }

            if (typeof(IMultiStadium).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> filter = e => !IsStadiumFilterEnabled || EF.Property<Guid>(e, "StadiumId") == CurrentStadiumId;
                expression = expression == null ? filter : CombineExpressions(expression, filter);
            }

            return expression;
        }
    }
}
