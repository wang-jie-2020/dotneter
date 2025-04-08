using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Demo.Controllers
{
    [ApiController]
    [Route("snapshot_demo")]
    public class SnapshotDemoController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConfiguration _configuration;

        public SnapshotDemoController(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("DEMO");
        }

        [HttpGet("demo1")]
        public void Demo1()
        {
            // Assumes GetConnectionString returns a valid connection string
            // where pooling is turned off by setting Pooling=False;.
            string connectionString = GetConnectionString();
            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                // Drop the TestSnapshot table if it exists
                connection1.Open();
                SqlCommand command1 = connection1.CreateCommand();
                command1.CommandText = "IF EXISTS "
                    + "(SELECT * FROM sys.tables WHERE name=N'TestSnapshot') "
                    + "DROP TABLE TestSnapshot";
                try
                {
                    command1.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // Enable Snapshot isolation
                command1.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION ON";
                command1.ExecuteNonQuery();

                // Create a table named TestSnapshot and insert one row of data
                command1.CommandText =
                    "CREATE TABLE TestSnapshot (ID int primary key, valueCol int)";
                command1.ExecuteNonQuery();
                command1.CommandText =
                    "INSERT INTO TestSnapshot VALUES (1,1)";
                command1.ExecuteNonQuery();

                // Begin, but do not complete, a transaction to update the data
                // with the Serializable isolation level, which locks the table
                // pending the commit or rollback of the update. The original
                // value in valueCol was 1, the proposed new value is 22.
                SqlTransaction transaction1 =
                    connection1.BeginTransaction(IsolationLevel.Serializable);
                command1.Transaction = transaction1;
                command1.CommandText =
                    "UPDATE TestSnapshot SET valueCol=22 WHERE ID=1";
                command1.ExecuteNonQuery();

                // Open a second connection to AdventureWorks
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    connection2.Open();
                    // Initiate a second transaction to read from TestSnapshot
                    // using Snapshot isolation. This will read the original
                    // value of 1 since transaction1 has not yet committed.
                    SqlCommand command2 = connection2.CreateCommand();
                    SqlTransaction transaction2 =
                        connection2.BeginTransaction(IsolationLevel.Snapshot);
                    command2.Transaction = transaction2;
                    command2.CommandText =
                        "SELECT ID, valueCol FROM TestSnapshot";
                    SqlDataReader reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        Console.WriteLine("Expected 1,1 Actual "
                            + reader2.GetValue(0).ToString()
                            + "," + reader2.GetValue(1).ToString());
                    }
                    reader2.Close();
                    transaction2.Commit();
                }

                // Open a third connection to AdventureWorks and
                // initiate a third transaction to read from TestSnapshot
                // using ReadCommitted isolation level. This transaction
                // will not be able to view the data because of
                // the locks placed on the table in transaction1
                // and will time out after 4 seconds.
                // You would see the same behavior with the
                // RepeatableRead or Serializable isolation levels.
                using (SqlConnection connection3 = new SqlConnection(connectionString))
                {
                    connection3.Open();
                    SqlCommand command3 = connection3.CreateCommand();
                    SqlTransaction transaction3 =
                        connection3.BeginTransaction(IsolationLevel.ReadCommitted);
                    command3.Transaction = transaction3;
                    command3.CommandText =
                        "SELECT ID, valueCol FROM TestSnapshot";
                    command3.CommandTimeout = 10;
                    try
                    {
                        SqlDataReader sqldatareader3 = command3.ExecuteReader();
                        while (sqldatareader3.Read())
                        {
                            Console.WriteLine("You should never hit this.");
                        }
                        transaction3.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Expected timeout expired exception: "
                            + ex.Message);
                        transaction3.Rollback();
                    }
                }

                // Open a fourth connection to AdventureWorks and
                // initiate a fourth transaction to read from TestSnapshot
                // using the ReadUncommitted isolation level. ReadUncommitted
                // will not hit the table lock, and will allow a dirty read
                // of the proposed new value 22 for valueCol. If the first
                // transaction rolls back, this value will never actually have
                // existed in the database.
                using (SqlConnection connection4 = new SqlConnection(connectionString))
                {
                    connection4.Open();
                    SqlCommand command4 = connection4.CreateCommand();
                    SqlTransaction transaction4 =
                        connection4.BeginTransaction(IsolationLevel.ReadUncommitted);
                    command4.Transaction = transaction4;
                    command4.CommandText =
                        "SELECT ID, valueCol FROM TestSnapshot";
                    SqlDataReader reader4 = command4.ExecuteReader();
                    while (reader4.Read())
                    {
                        Console.WriteLine("Expected 1,22 Actual "
                            + reader4.GetValue(0).ToString()
                            + "," + reader4.GetValue(1).ToString());
                    }
                    reader4.Close();
                    transaction4.Commit();
                }

                // Roll back the first transaction
                transaction1.Rollback();
            }

            // CLEANUP
            // Delete the TestSnapshot table and set
            // ALLOW_SNAPSHOT_ISOLATION OFF
            using (SqlConnection connection5 = new SqlConnection(connectionString))
            {
                connection5.Open();
                SqlCommand command5 = connection5.CreateCommand();
                command5.CommandText = "DROP TABLE TestSnapshot";
                SqlCommand command6 = connection5.CreateCommand();
                command6.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION OFF";
                try
                {
                    command5.ExecuteNonQuery();
                    command6.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Done!");
        }

        [HttpGet("demo2")]
        public void Demo2()
        {
            // Assumes GetConnectionString returns a valid connection string
            // where pooling is turned off by setting Pooling=False;.
            string connectionString = GetConnectionString();
            using (SqlConnection connection1 = new SqlConnection(connectionString))
            {
                connection1.Open();
                SqlCommand command1 = connection1.CreateCommand();

                // Enable Snapshot isolation in AdventureWorks
                command1.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION ON";
                try
                {
                    command1.ExecuteNonQuery();
                    Console.WriteLine(
                        "Snapshot Isolation turned on in AdventureWorks.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ALLOW_SNAPSHOT_ISOLATION ON failed: {0}", ex.Message);
                }
                // Create a table
                command1.CommandText =
                    "IF EXISTS "
                    + "(SELECT * FROM sys.tables "
                    + "WHERE name=N'TestSnapshotUpdate')"
                    + " DROP TABLE TestSnapshotUpdate";
                command1.ExecuteNonQuery();
                command1.CommandText =
                    "CREATE TABLE TestSnapshotUpdate "
                    + "(ID int primary key, CharCol nvarchar(100));";
                try
                {
                    command1.ExecuteNonQuery();
                    Console.WriteLine("TestSnapshotUpdate table created.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CREATE TABLE failed: {0}", ex.Message);
                }
                // Insert some data
                command1.CommandText =
                    "INSERT INTO TestSnapshotUpdate VALUES (1,N'abcdefg');"
                    + "INSERT INTO TestSnapshotUpdate VALUES (2,N'hijklmn');"
                    + "INSERT INTO TestSnapshotUpdate VALUES (3,N'opqrstuv');";
                try
                {
                    command1.ExecuteNonQuery();
                    Console.WriteLine("Data inserted TestSnapshotUpdate table.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                // Begin, but do not complete, a transaction
                // using the Snapshot isolation level.
                SqlTransaction transaction1 = null;
                try
                {
                    transaction1 = connection1.BeginTransaction(IsolationLevel.Snapshot);
                    command1.CommandText =
                        "SELECT * FROM TestSnapshotUpdate WHERE ID BETWEEN 1 AND 3";
                    command1.Transaction = transaction1;
                    command1.ExecuteNonQuery();
                    Console.WriteLine("Snapshot transaction1 started.");

                    // Open a second Connection/Transaction to update data
                    // using ReadCommitted. This transaction should succeed.
                    using (SqlConnection connection2 = new SqlConnection(connectionString))
                    {
                        connection2.Open();
                        SqlCommand command2 = connection2.CreateCommand();
                        command2.CommandText = "UPDATE TestSnapshotUpdate SET CharCol="
                            + "N'New value from Connection2' WHERE ID=1";
                        SqlTransaction transaction2 =
                            connection2.BeginTransaction(IsolationLevel.ReadCommitted);
                        command2.Transaction = transaction2;
                        try
                        {
                            command2.ExecuteNonQuery();
                            transaction2.Commit();
                            Console.WriteLine(
                                "transaction2 has modified data and committed.");
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction2.Rollback();
                        }
                        finally
                        {
                            transaction2.Dispose();
                        }
                    }

                    // Now try to update a row in Connection1/Transaction1.
                    // This transaction should fail because Transaction2
                    // succeeded in modifying the data.
                    command1.CommandText =
                        "UPDATE TestSnapshotUpdate SET CharCol="
                        + "N'New value from Connection1' WHERE ID=1";
                    command1.Transaction = transaction1;
                    command1.ExecuteNonQuery();
                    transaction1.Commit();
                    Console.WriteLine("You should never see this.");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Expected failure for transaction1:");
                    Console.WriteLine("  {0}: {1}", ex.Number, ex.Message);
                }
                finally
                {
                    transaction1.Dispose();
                }
            }

            // CLEANUP:
            // Turn off Snapshot isolation and delete the table
            using (SqlConnection connection3 = new SqlConnection(connectionString))
            {
                connection3.Open();
                SqlCommand command3 = connection3.CreateCommand();
                command3.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION OFF";
                try
                {
                    command3.ExecuteNonQuery();
                    Console.WriteLine(
                        "CLEANUP: Snapshot isolation turned off in AdventureWorks.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CLEANUP FAILED: {0}", ex.Message);
                }
                command3.CommandText = "DROP TABLE TestSnapshotUpdate";
                try
                {
                    command3.ExecuteNonQuery();
                    Console.WriteLine("CLEANUP: TestSnapshotUpdate table deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CLEANUP FAILED: {0}", ex.Message);
                }
            }
        }

        /*
         *  SET ALLOW_SNAPSHOT_ISOLATION ON 的有效范围是什么???
         */

        [HttpGet("seed")]
        public void Seed()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Drop the TestSnapshot table if it exists
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "IF EXISTS "
                                      + "(SELECT * FROM sys.tables WHERE name=N'TestSnapshot') "
                                      + "DROP TABLE TestSnapshot";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // Enable Snapshot isolation
                command.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION ON";
                command.ExecuteNonQuery();

                // Create a table named TestSnapshot and insert one row of data
                command.CommandText =
                    "CREATE TABLE TestSnapshot (ID int primary key, valueCol int)";
                command.ExecuteNonQuery();
                command.CommandText =
                    "INSERT INTO TestSnapshot VALUES (1,1)";
                command.ExecuteNonQuery();
            }
        }

        [HttpGet("cleanup")]
        public void Cleanup()
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection5 = new SqlConnection(connectionString))
            {
                connection5.Open();
                SqlCommand command5 = connection5.CreateCommand();
                command5.CommandText = "DROP TABLE TestSnapshot";
                SqlCommand command6 = connection5.CreateCommand();
                command6.CommandText =
                    "ALTER DATABASE AdventureWorks SET ALLOW_SNAPSHOT_ISOLATION OFF";
                try
                {
                    command5.ExecuteNonQuery();
                    command6.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        [HttpGet("query")]
        public void Query()
        {
            string connectionString = GetConnectionString();

            Task.Run(() =>
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "UPDATE TestSnapshot SET valueCol=22 WHERE ID=1";
                command.ExecuteNonQuery();
            });

            Thread.Sleep(1000);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Snapshot);
                command.Transaction = transaction;
                command.CommandText = "SELECT ID, valueCol FROM TestSnapshot";
                command.CommandTimeout = 10;
                SqlDataReader reader2 = command.ExecuteReader();
                while (reader2.Read())
                {
                    Console.WriteLine("Expected 1,1 Actual " + reader2.GetValue(0).ToString() + "," + reader2.GetValue(1).ToString());
                }
                reader2.Close();
                transaction.Commit();
            }

            Console.WriteLine("Done!");
        }
    }
}






