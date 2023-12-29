using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Demo.EFCoreExtensions
{
    public class CustomSqlServerQuerySqlGenerator : SqlServerQuerySqlGenerator
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public CustomSqlServerQuerySqlGenerator([NotNull] QuerySqlGeneratorDependencies dependencies) : base(dependencies)
        {
        }

        protected override Expression VisitTable(TableExpression tableExpression)
        {
            var result = base.VisitTable(tableExpression);
            Sql.Append(" WITH (NOLOCK)");
            return result;
        }
    }
}
