using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EFCore.BulkExtensions;
using EFCoreBenchmark.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EFCoreBenchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        //Setup();
        BenchmarkRunner.Run<NavigatorBenchmark>();
        //BenchmarkRunner.Run<PerformanceBenchmark>();
        Console.ReadLine();
    }

    //static SqliteConnection connection;
    //static BloggingContext context;

    //public static void Setup()
    //{
    //    connection = new SqliteConnection("Data Source=:memory:");
    //    connection.Open();

    //    var builder = new DbContextOptionsBuilder(new DbContextOptions<BloggingContext>());
    //    builder.UseSqlite(connection);
    //    context = new BloggingContext(builder.Options as DbContextOptions<BloggingContext>);
    //    context.Database.EnsureCreated();

    //    DataSeed();
    //    LeftOuterJoin();
    //    LeftOuterJoinWithLinQ();
    //}

    //private static void DataSeed()
    //{
    //    var random = new Random();

    //    var blogs = new List<Blog>();
    //    var posts = new List<Post>();
    //    var postTags = new List<PostTag>();
    //    var tags = new List<Tag>();

    //    for (int i = 1; i <= 50000; i++)
    //    {
    //        blogs.Add(new Blog()
    //        {
    //            BlogId = i,
    //            Url = @"https://devblogs.microsoft.com/dotnet",
    //            Rating = 5,
    //            OwnerId = 1,
    //            ThemeId = 1
    //        });
    //    }

    //    for (int i = 1; i <= 10000; i++)
    //    {
    //        posts.Add(new Post()
    //        {
    //            PostId = i,
    //            BlogId = random.Next(1, 50000),
    //            Title = "What's new",
    //            Content = "Lorem ipsum dolor sit amet",
    //            Rating = 5,
    //            AuthorId = 1
    //        });
    //    }

    //    for (int i = 1; i <= 10000; i++)
    //    {
    //        tags.Add(new Tag()
    //        {
    //            TagId = i.ToString(),
    //        });
    //    }

    //    int postTagId = 1;
    //    foreach (var post in posts)
    //    {
    //        var tag1 = random.Next(1, 1000).ToString();
    //        var tag2 = random.Next(2000, 4000).ToString();
    //        var tag3 = random.Next(6000, 8000).ToString();
    //        var tag4 = random.Next(8000, 10000).ToString();

    //        postTags.Add(new PostTag()
    //        {
    //            PostTagId = postTagId++,
    //            PostId = post.PostId,
    //            TagId = tag1,
    //        });

    //        postTags.Add(new PostTag()
    //        {
    //            PostTagId = postTagId++,
    //            PostId = post.PostId,
    //            TagId = tag2,
    //        });

    //        postTags.Add(new PostTag()
    //        {
    //            PostTagId = postTagId++,
    //            PostId = post.PostId,
    //            TagId = tag3,
    //        });

    //        postTags.Add(new PostTag()
    //        {
    //            PostTagId = postTagId++,
    //            PostId = post.PostId,
    //            TagId = tag4,
    //        });
    //    }

    //    context.BulkInsert(blogs);
    //    context.BulkInsert(posts);
    //    context.BulkInsert(tags);
    //    context.BulkInsert(postTags);

    //}

    //public static void LeftOuterJoin()
    //{
    //    var blogs = context.Blogs
    //        .Include(blog => blog.Posts)
    //        .ThenInclude(post => post.Tags)
    //        .ThenInclude(tags => tags.Tag)
    //        .ToList();

    //    var list = blogs.ToList();
    //}

    //public static void LeftOuterJoinWithLinQ()
    //{
    //    var query = from blog in context.Blogs
    //                join post in context.Posts on blog.BlogId equals post.BlogId into g
    //                from post in g.DefaultIfEmpty()
    //                join postTag in context.PostTags on post.PostId equals postTag.PostId into h
    //                from postTag in h.DefaultIfEmpty()
    //                join tag in context.Tags on postTag.TagId equals tag.TagId into m
    //                from tag in m.DefaultIfEmpty()
    //                select new
    //                {
    //                    blog,
    //                    post,
    //                    postTag,
    //                    tag
    //                };

    //    var blogs = query.ToList();

    //    var distinct = blogs.Select(p => p.blog.BlogId).Distinct().ToList();
    //}
}