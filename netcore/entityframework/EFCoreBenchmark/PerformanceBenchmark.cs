using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using EFCore.BulkExtensions;
using EFCoreBenchmark.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EFCoreBenchmark
{
    [SimpleJob(RuntimeMoniker.Net80, warmupCount: 5, iterationCount: 1)]
    public class PerformanceBenchmark
    {
        public PerformanceBenchmark()
        {
        }

        private SqliteConnection connection;
        private BloggingContext context;

        [GlobalSetup]
        public void Setup()
        {
            connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var builder = new DbContextOptionsBuilder(new DbContextOptions<BloggingContext>());
            builder.UseSqlite(connection);
            context = new BloggingContext(builder.Options as DbContextOptions<BloggingContext>);
            context.Database.EnsureCreated();

            DataSeed();
        }

        private void DataSeed()
        {
            var random = new Random();

            var blogs = new List<Blog>();
            var posts = new List<Post>();
            var postTags = new List<PostTag>();
            var tags = new List<Tag>();

            for (int i = 1; i <= 50000; i++)
            {
                blogs.Add(new Blog()
                {
                    BlogId = i,
                    Url = @"https://devblogs.microsoft.com/dotnet",
                    Rating = 5,
                    OwnerId = 1,
                    ThemeId = 1
                });
            }

            for (int i = 1; i <= 1000000; i++)
            {
                posts.Add(new Post()
                {
                    PostId = i,
                    BlogId = random.Next(1, 50000),
                    Title = "What's new",
                    Content = "Lorem ipsum dolor sit amet",
                    Rating = 5,
                    AuthorId = 1
                });
            }

            for (int i = 1; i <= 10000; i++)
            {
                tags.Add(new Tag()
                {
                    TagId = i.ToString(),
                });
            }

            int postTagId = 1;
            foreach (var post in posts)
            {
                var tag1 = random.Next(1, 100).ToString();
                var tag2 = random.Next(200, 400).ToString();
                var tag3 = random.Next(600, 800).ToString();
                var tag4 = random.Next(800, 1000).ToString();

                postTags.Add(new PostTag()
                {
                    PostTagId = postTagId++,
                    PostId = post.PostId,
                    TagId = tag1,
                });

                postTags.Add(new PostTag()
                {
                    PostTagId = postTagId++,
                    PostId = post.PostId,
                    TagId = tag2,
                });

                postTags.Add(new PostTag()
                {
                    PostTagId = postTagId++,
                    PostId = post.PostId,
                    TagId = tag3,
                });

                postTags.Add(new PostTag()
                {
                    PostTagId = postTagId++,
                    PostId = post.PostId,
                    TagId = tag4,
                });
            }

            context.BulkInsert(blogs);
            context.BulkInsert(posts);
            context.BulkInsert(tags);
            context.BulkInsert(postTags);

        }

        //[Params(1, 3)] public int N;

        [Params(1)]
        public int N;

        [Benchmark]
        public void WithoutOrderBy()
        {
            var query1 = context.Blogs.AsQueryable();

            var query2 = context.Posts.Include(a => a.Tags).ThenInclude(b => b.Tag);

            var query = from a in query1
                        join b in query2 on a.BlogId equals b.BlogId into go
                        from b in go.DefaultIfEmpty()
                        select new
                        {
                            a,
                            b,
                            b.Tags,
                        };

            var list = query.Skip(2100).Take(1000).ToList();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(list.FirstOrDefault()));
        }

        [Benchmark]
        public void WithOrderBy()
        {
            var query1 = context.Blogs.AsQueryable();

            var query2 = context.Posts.Include(a => a.Tags).ThenInclude(b => b.Tag);

            var query = from a in query1
                        join b in query2 on a.BlogId equals b.BlogId into go
                        from b in go.DefaultIfEmpty()
                        select new
                        {
                            a,
                            b,
                            b.Tags,
                        };

            var list = query.OrderBy(a => a.a.BlogId).Skip(2100).Take(1000).ToList();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(list.FirstOrDefault()));
        }
    }
}