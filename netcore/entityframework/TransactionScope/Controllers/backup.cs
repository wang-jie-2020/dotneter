//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Transactions;
//using Dapper;
//using Demo.Data;
//using Demo.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using IsolationLevel = System.Data.IsolationLevel;

//namespace Demo.Controllers
//{
//    /// <summary>
//    ///     测试一些限定环境:
//    ///         1.SQLSERVER的隔离级别read-committed下,不能在read-committed-snapshot下
//    ///         2.Dapper 1.x
//    ///         3.Dapper依赖的System.Data.SqlClient要>4.4.0,否则不支持环境事务---2.x不用考虑
//    /// </summary>
//    [ApiController]
//    [Route("mssql")]
//    public class MSSQLController : ControllerBase
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;
//        private readonly IConfiguration _configuration;


//        public MSSQLController(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _configuration = configuration;
//        }

//        private void LongTimeBlock()
//        {
//            using var scope = _serviceScopeFactory.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            context.Database.SetCommandTimeout(80);

//            var sql = @"BEGIN TRAN
//                        UPDATE Authors SET Profile = GETDATE() where id > 30
//                        WAITFOR DELAY '00:01:00'
//                        COMMIT";

//            context.Database.ExecuteSqlRaw(sql);
//        }

//        private object QueryThroughEf()
//        {
//            using var scope = _serviceScopeFactory.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//            var query = context.Authors.AsQueryable();
//            var list = query.ToList();

//            return list;
//        }

//        private object QueryThroughEfTransaction()
//        {
//            using var scope = _serviceScopeFactory.CreateScope();
//            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

//            using var trans = context.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
//            //using var trans = context.Database.BeginTransaction(IsolationLevel.Snapshot);
//            var query = context.Authors.AsQueryable();
//            var list = query.ToList();

//            trans.Commit();
//            return list;
//        }

//        private object QueryThroughDapper()
//        {
//            IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
//            return connection.Query<Author>("select * from authors");
//        }

//        private object QueryThroughDapperTransaction()
//        {
//            IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
//            if (connection.State == ConnectionState.Closed)
//            {
//                connection.Open();
//            }

//            try
//            {
//                using (var trans = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
//                //using (var trans = connection.BeginTransaction(IsolationLevel.Snapshot))
//                {
//                    var list = connection.Query<Author>("select * from authors", null, trans);
//                    trans.Commit();
//                    return list;
//                }
//            }
//            finally
//            {
//                if (connection.State == ConnectionState.Open)
//                {
//                    connection.Close();
//                }
//            }
//        }

//        [HttpGet("without-transaction-ef-fail")]
//        public async Task<object> WithoutTransactionOfEf()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            var list = QueryThroughEf();

//            await Task.CompletedTask;
//            return list;
//        }

//        [HttpGet("without-transaction-dapper-fail")]
//        public async Task<object> WithoutTransactionOfDapper()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            var list = QueryThroughDapper();

//            await Task.CompletedTask;
//            return list;
//        }

//        [HttpGet("with-transaction-success")]
//        public async Task<object> WithTransaction()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            var list1 = QueryThroughEfTransaction();
//            var list2 = QueryThroughDapperTransaction();

//            await Task.CompletedTask;
//            return list2;
//        }

//        [HttpGet("mixed-oneScope-fail")]
//        public async Task<object> MixedWithOneTransactionScope()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);
//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();
//                var list2 = QueryThroughDapper();
//                transactionScope.Complete();
//                return list2;
//            };
//        }

//        [HttpGet("mixed-oneScope-success-same-connectionType")]
//        public async Task<object> MixedWithOneTransactionScopeInSameConnectionType()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();

//                IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
//                var list2 = connection.Query<Author>("select * from authors");

//                transactionScope.Complete();
//                return list2;
//            };
//        }

//        [HttpGet("mixed-twoScope-fail")]
//        public async Task<object> MixedWithTwoTransactionScope1()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();

//                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    var list2 = QueryThroughDapper();
//                    transactionScope2.Complete();
//                }

//                transactionScope.Complete();
//                return list1;
//            };
//        }

//        [HttpGet("mixed-twoScope-success-1")]
//        public async Task<object> MixedWithTwoTransactionScope2()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();

//                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//                {
//                    var list2 = QueryThroughDapper();
//                    transactionScope2.Complete();
//                }

//                transactionScope.Complete();
//                return list1;
//            };
//        }

//        [HttpGet("mixed-twoScope-success-2")]
//        public async Task<object> MixedWithTwoTransactionScope3()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();

//                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    IDbConnection connection = new System.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
//                    var list3 = connection.Query<Author>("select * from authors with(nolock)");
//                    transactionScope2.Complete();
//                }

//                transactionScope.Complete();
//                return list1;
//            };
//        }

//        [HttpGet("success-but-why")]
//        public async Task<object> MixedWithTwoTransactionScope()
//        {
//            _ = Task.Run(() =>
//            {
//                LongTimeBlock();
//            });

//            Thread.Sleep(2000);

//            await Task.CompletedTask;
//            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//            {
//                var list1 = QueryThroughEf();

//                //当且仅当有这部分,下一部分才正常?不能理解原理
//                using (var transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
//                {
//                    var list2 = QueryThroughDapper();
//                    transactionScope2.Complete();
//                }

//                using (var transactionScope3 = new TransactionScope(TransactionScopeOption.Suppress))
//                {
//                    var list3 = QueryThroughDapper();
//                    transactionScope3.Complete();
//                }

//                transactionScope.Complete();
//                return list1;
//            };
//        }
//    }
//}






