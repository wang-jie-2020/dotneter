using Microsoft.EntityFrameworkCore;
using Translate.Models;

namespace Translate.Data
{
    public class TranslateDbContext : DbContext
    {
        public TranslateDbContext(DbContextOptions<TranslateDbContext> options) : base(options)
        {

        }

        public DbSet<TranslateText> TranslateTexts { get; set; }

        public DbSet<CoderReviewText> CoderReviewTexts { get; set; }

    }
}
