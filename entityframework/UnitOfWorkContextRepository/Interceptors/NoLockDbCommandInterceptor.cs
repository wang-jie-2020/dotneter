using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace UnitOfWorkContextRepository.Interceptors
{
    /*
     * 考虑在拦截器中修改数据表,比如改成from A as a with(nolock)
     *  但正则表达式实在是太丑陋了!!!
     */
    public class NoLockDbCommandInterceptor : DbCommandInterceptor
    {
        private static readonly Regex TableAliasRegex =
            new Regex(@"(?<tableAlias>\[dbo\].\[\w+\] AS \[Extent\d+\](?! WITH\(NOLOCK\)))",
                RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        {
            RebuildCommandText(command);
            return base.NonQueryExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            RebuildCommandText(command);
            return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            RebuildCommandText(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            RebuildCommandText(command);
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
        {
            RebuildCommandText(command);
            return base.ScalarExecuting(command, eventData, result);
        }

        public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            RebuildCommandText(command);
            return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        protected virtual void RebuildCommandText(DbCommand command)
        {
            var text = TableAliasRegex.Replace(
                command.CommandText,
                "${tableAlias} WITH (NOLOCK)"
            );
        }
    }
}
