using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using Dapper;
using System.Transactions;
using Demo.Data;
using Demo.Models;
using IsolationLevel = System.Data.IsolationLevel;

namespace Demo.Controllers
{
    [ApiController]
    [Route("snapshot")]
    public class SnapshotController : DefaultController
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public SnapshotController(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration) : base(serviceScopeFactory, configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        [HttpGet("allow")]
        public void ALLOW_SNAPSHOT_ISOLATION()
        {
            IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
            connection.Execute("ALTER DATABASE test SET ALLOW_SNAPSHOT_ISOLATION ON");
        }

        [HttpGet("disallow")]
        public void DISALLOW_SNAPSHOT_ISOLATION()
        {
            IDbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(_configuration.GetConnectionString("MSSQL"));
            connection.Execute("ALTER DATABASE test SET ALLOW_SNAPSHOT_ISOLATION OFF");
        }

        [HttpGet("test")]
        public void Test()
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Snapshot }))
            {
                DefaultFail();
            }
        }
    }
}






