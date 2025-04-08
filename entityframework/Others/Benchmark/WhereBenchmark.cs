using System.Linq;
using Benchmark.Data;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Benchmark
{
    public class WhereBenchmark
    {
        public WhereBenchmark()
        {

        }

        [Params(1)]
        public int N;

        [Benchmark]
        public void OuterWhere()
        {
            for (int i = 0; i < N; i++)
            {
                using var context = new BloggingContext();

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

                var blogs = query.Where(p => p.blog.BlogId < 20000).ToList();
            }
        }

        [Benchmark]
        public void InnerWhere()
        {
            for (int i = 0; i < N; i++)
            {
                using var context = new BloggingContext();

                var query = from blog in context.Blogs.Where(p => p.BlogId < 20000)
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
}
