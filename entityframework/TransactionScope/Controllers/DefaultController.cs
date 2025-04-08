using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IsolationLevel = System.Data.IsolationLevel;

namespace Demo.Controllers
{
    /// <summary>
    ///     测试一些限定环境:
    ///         1.SQLSERVER的隔离级别read-committed下,不能在read-committed-snapshot下
    ///         2.Dapper 1.x
    ///         3.Dapper依赖的System.Data.SqlClient要>4.4.0,否则不支持环境事务---2.x不用考虑
    /// </summary>
    [ApiController]
    [Route("default")]
    public class DefaultController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public DefaultController(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        protected void Blocking()
        {
            IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            var trans = connection.BeginTransaction(IsolationLevel.Serializable);
            connection.Execute("UPDATE Authors SET Profile = GETDATE() where id > 30", null, trans);
        }

        protected object QueryThroughEf()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var query = context.Authors.AsQueryable();
            var list = query.ToList();

            return list;
        }

        protected object QueryThroughDapper1x()
        {
            IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
            return connection.Query<Author>("select * from authors", commandTimeout: 10);
        }

        protected object QueryThroughDapper2x()
        {
            IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
            return connection.Query<Author>("select * from authors", commandTimeout: 10);
        }

        [HttpGet("default-fail")]
        public async Task<object> DefaultFail()
        {
            _ = Task.Run(() =>
            {
                Blocking();
            });

            Thread.Sleep(2000);

            try
            {
                QueryThroughEf();
            }
            catch (Exception e)
            {
                Console.WriteLine("ef fail");
            }

            try
            {
                QueryThroughDapper1x();
            }
            catch (Exception e)
            {
                Console.WriteLine("dapper1 fail");
            }

            try
            {
                QueryThroughDapper2x();
            }
            catch (Exception e)
            {
                Console.WriteLine("dapper2 fail");
            }

            await Task.CompletedTask;
            return "fail";
        }

        [HttpGet("read_uncommitted-transaction-success")]
        public async Task<object> ReadUncommittedTransaction()
        {
            _ = Task.Run(() =>
            {
                Blocking();
            });

            Thread.Sleep(2000);

            QueryThroughEfTransaction();
            QueryThroughDapperTransaction();

            await Task.CompletedTask;
            return "success";

            object QueryThroughEfTransaction()
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                using var trans = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
                var query = context.Authors.AsQueryable();
                var list = query.ToList();

                trans.Commit();
                return list;
            }

            object QueryThroughDapperTransaction()
            {
                IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                try
                {
                    using (var trans = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        var list = connection.Query<Author>("select * from authors", null, trans);
                        trans.Commit();
                        return list;
                    }
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [HttpGet("read_uncommitted-transaction_scope-success")]
        public async Task<object> ReadUncommittedTransactionScope()
        {
            _ = Task.Run(() =>
            {
                Blocking();
            });

            Thread.Sleep(2000);

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                QueryThroughEf();
                transactionScope.Complete();
            }

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                QueryThroughDapper1x();
                transactionScope.Complete();
            }


            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                QueryThroughDapper2x();
                transactionScope.Complete();
            }

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                QueryThroughEf();
                QueryThroughDapper2x();

                try
                {
                    var list = QueryThroughDapper1x();
                }
                catch (Exception e)
                {
                    Console.WriteLine("dapper1 fail");
                }

                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    QueryThroughDapper1x();
                    transactionScope2.Complete();
                }

                using (var transactionScope3 = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
                    var list = connection.Query<Author>("select * from authors with(nolock)");
                    transactionScope3.Complete();
                }

                transactionScope.Complete();

            };

            await Task.CompletedTask;
            return "success";
        }

        [HttpGet("success-but-why")]
        public async Task<object> MixedWithTwoTransactionScope()
        {
            _ = Task.Run(() =>
            {
                Blocking();
            });

            Thread.Sleep(2000);

            await Task.CompletedTask;
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var list1 = QueryThroughEf();

                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    var list = QueryThroughDapper1x();
                    transactionScope2.Complete();
                }

                //why ???????
                using (var transactionScope3 = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var list = QueryThroughDapper1x();
                    transactionScope3.Complete();
                }

                transactionScope.Complete();
            };

            return " why success";
        }
    }
}






