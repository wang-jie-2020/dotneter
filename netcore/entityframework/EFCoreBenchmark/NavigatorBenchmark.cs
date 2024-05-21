using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using EFCore.BulkExtensions;
using EFCoreBenchmark.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCoreBenchmark
{
    [SimpleJob(RuntimeMoniker.Net80, warmupCount: 5, iterationCount: 1)]
    public class NavigatorBenchmark
    {
        public NavigatorBenchmark()
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

            for (int i = 1; i <= 100000; i++)
            {
                tags.Add(new Tag()
                {
                    TagId = i.ToString(),
                });
            }

            int postTagId = 1;
            foreach (var post in posts)
            {
                var tag1 = random.Next(1, 1000).ToString();
                var tag2 = random.Next(20000, 40000).ToString();
                var tag3 = random.Next(60000, 80000).ToString();
                var tag4 = random.Next(80000, 100000).ToString();

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

        //[Benchmark]
        //public void SingleInclude()
        //{
        //    var blogs = context.Blogs.Include(blog => blog.Posts).ToList();
        //}

        //[Benchmark]
        //public void SingleLINQ()
        //{
        //    var query = from a in context.Blogs
        //                join b in context.Posts on a.BlogId equals b.BlogId into g
        //                from b in g.DefaultIfEmpty()
        //                select new
        //                {
        //                    a,
        //                    b
        //                };

        //    var blogs = query.ToList();

        //}

        //[Benchmark]
        //public void MultipleInclude()
        //{
        //    var blogs = context.Blogs
        //        .Include(blog => blog.Posts)
        //        .Include(blog => blog.Owner)
        //        .ToList();
        //}

        //[Benchmark]
        //public void MultipleLINQ()
        //{
        //    var query = from a in context.Blogs
        //                join b in context.Posts on a.BlogId equals b.BlogId into g
        //                from b in g.DefaultIfEmpty()
        //                join c in context.Person on a.OwnerId equals c.PersonId into m
        //                from c in m.DefaultIfEmpty()
        //                select new
        //                {
        //                    a,
        //                    b,
        //                    c
        //                };
        //    var blogs = query.ToList();

        //}

        //[Benchmark]
        //public void SingleChildInclude()
        //{
        //    var blogs = context.Blogs
        //        .Include(blog => blog.Posts)
        //        .ThenInclude(post => post.Author)
        //        .ToList();
        //}

        //[Benchmark]
        //public void SingleChildLINQ()
        //{
        //    var query0 =
        //        from post in context.Posts
        //        join person in context.Person on post.AuthorId equals person.PersonId into g
        //        from person in g.DefaultIfEmpty()
        //        select new
        //        {
        //            post,
        //            person
        //        };

        //    var query1 =
        //        from a in context.Posts
        //        join b in query0 on a.AuthorId equals b.post.BlogId into g
        //        from b in g.DefaultIfEmpty()
        //        select new
        //        {
        //            a,
        //            b.post,
        //            b.person
        //        };

        //    var blogs = query1.ToList();
        //}


        //[Benchmark]
        //public void MultipleThenIncludes()
        //{
        //    var blogs = context.Blogs
        //        .Include(blog => blog.Posts)
        //        .ThenInclude(post => post.Author)
        //        .ThenInclude(author => author.Photo)
        //        .ToList();
        //}

        // [Benchmark]
        // public void IncludeTree()
        // {
        //     using var context = new BloggingContext();
        //     var blogs = context.Blogs
        //         .Include(blog => blog.Posts)
        //         .ThenInclude(post => post.Author)
        //         .ThenInclude(author => author.Photo)
        //         .Include(blog => blog.Owner)
        //         .ThenInclude(owner => owner.Photo)
        //         .ToList();
        // }
        //
        // [Benchmark]
        // public void MultipleLeafIncludes()
        // {
        //     using var context = new BloggingContext();
        //     var blogs = context.Blogs
        //         .Include(blog => blog.Posts)
        //         .ThenInclude(post => post.Author)
        //         .Include(blog => blog.Posts)
        //         .ThenInclude(post => post.Tags)
        //         .ToList();
        // }
        //
        // [Benchmark]
        // public void IncludeMultipleNavigationsWithSingleInclude()
        // {
        //     using var context = new BloggingContext();
        //     var blogs = context.Blogs
        //         .Include(blog => blog.Owner.AuthoredPosts)
        //         .ThenInclude(post => post.Blog.Owner.Photo)
        //         .ToList();
        // }

        //[Benchmark]
        //public void MultipleLeafIncludesFiltered2()
        //{
        //    using var context = new BloggingContext();
        //    var filteredBlogs = context.Blogs
        //        .Include(blog => blog.Posts.Where(post => post.BlogId == 1))
        //        .ThenInclude(post => post.Author)
        //        .Include(blog => blog.Posts.Where(post => post.BlogId == 1))
        //        .ThenInclude(post => post.Tags.OrderBy(postTag => postTag.TagId).Skip(3))
        //        .ToList();
        //}

        [Benchmark]
        public void LeftOuterJoin()
        {
            var blogs = context.Blogs
                .Include(blog => blog.Posts)
                .ThenInclude(post => post.Tags)
                .ThenInclude(tags => tags.Tag)
                .ToList();
        }

        [Benchmark]
        public void LeftOuterJoinWithLinQ()
        {
            var query = from blog in context.Blogs
                        join post in context.Posts on blog.BlogId equals post.BlogId into g
                        from post in g.DefaultIfEmpty()
                        join postTag in context.PostTags on post.PostId equals postTag.PostId into h
                        from postTag in h.DefaultIfEmpty()
                        join tag in context.Tags on postTag.TagId equals tag.TagId into m
                        from tag in m.DefaultIfEmpty()
                        select new
                        {
                            blog,
                            post,
                            postTag,
                            tag
                        };

            var blogs = query.ToList();

        }
    }
}