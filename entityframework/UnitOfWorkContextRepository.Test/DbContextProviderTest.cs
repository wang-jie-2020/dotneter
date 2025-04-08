using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Test.Fixtures;
using UnitOfWorkContextRepository.Transaction;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UnitOfWorkContextRepository.Test
{
    [Collection("uow-collection")]
    public class DbContextProviderTest
    {
        private readonly UowFixture _baseFixture;
        private readonly IEfCoreRepository _repository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DbContextProviderTest(UowFixture baseFixture)
        {
            _baseFixture = baseFixture;
            _repository = baseFixture.ServiceProvider.GetRequiredService<IEfCoreRepository>();
            _unitOfWorkManager = baseFixture.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task DefaultDbContextProviderTest()
        {
            var context = await _repository.GetDbContextAsync();
            context.ShouldNotBeNull();

            //默认的DbContext注册是Scope周期的
            var context1 = await _repository.GetDbContextAsync();
            var context2 = await _repository.GetDbContextAsync();
            context1.ShouldBeSameAs(context2);
        }

        [Fact]
        public async Task UnitOfWorkDbContextProviderTest()
        {
            var context1 = await _repository.GetDbContextAsync();
            using (var uow = _unitOfWorkManager.Begin())
            {
                var context2 = await _repository.GetDbContextAsync();
                var context3 = await _repository.GetDbContextAsync();

                context1.ShouldNotBeSameAs(context2);
                context2.ShouldBeSameAs(context3);
            }
        }
    }
}
