using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EFCore.BulkExtensions;
using EFCoreBenchmark.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBenchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<NavigatorBenchmark>();
        //BenchmarkRunner.Run<PerformanceBenchmark>();
        Console.ReadLine();
    }

    //private static SqliteConnection connection;
    //private static BloggingContext context;

    //public static void Setup()
    //{
    //    connection = new SqliteConnection("Data Source=:memory:");
    //    connection.Open();

    //    var builder = new DbContextOptionsBuilder(new DbContextOptions<BloggingContext>());
    //    builder.UseSqlite(connection);

    //    context = new BloggingContext(builder.Options as DbContextOptions<BloggingContext>);

    //    context.Database.EnsureCreated();

    //    Console.WriteLine(context.Person.Count());
    //}
}