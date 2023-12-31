﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Hugin.BookStore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Hugin.BookStore.Repository
{
    public class EfCoreAuthorRepository : EfCoreRepository<BookStoreDbContext, Author, Guid>, IAuthorRepository
    {
        public EfCoreAuthorRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Author> FindByNameAsync(string name)
        {
            return await DbSet.FirstOrDefaultAsync(author => author.Name == name);
        }

        public async Task<List<Author>> GetListAsync(int skipCount, int maxResultCount, string sorting, string filter = null)
        {
            return await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(), author => author.Name.Contains(filter))
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
