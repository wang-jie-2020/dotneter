using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Hugin.BookStore.Controllers
{
    [Route("api/uow")]
    public class UowController : AbpController
    {
        private readonly IRepository<Book, Guid> _bookRepository;

        public UowController(IRepository<Book, Guid> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        [Route("get-auto-uow")]
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task ManualUow()
        {
            Debug.Assert(UnitOfWorkManager.Current == null);

            using (var uow1 = UnitOfWorkManager.Begin())
            {
                using (var uow2 = UnitOfWorkManager.Begin())
                {
                    using (var uow3 = UnitOfWorkManager.Begin())
                    {

                    }
                }
            }

            await Task.CompletedTask;
        }

        [HttpGet]
        [Route("get-manual-uow")]
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task Uow()
        {
            Debug.Assert(UnitOfWorkManager.Current == null);

            using (var uow1 = UnitOfWorkManager.Begin())
            {
                using (var uow2 = UnitOfWorkManager.Begin(requiresNew: true))
                {
                    using (var uow3 = UnitOfWorkManager.Begin(requiresNew: true))
                    {

                    }
                }
            }

            await Task.CompletedTask;
        }

        [HttpGet]
        [Route("error-uow")]
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task ErrorUow()
        {
            Debug.Assert(UnitOfWorkManager.Current == null);

            using (var uow1 = UnitOfWorkManager.Begin())
            {
                await _bookRepository.InsertAsync(new Book()
                {
                    Name = "阳阳阳"
                });

                throw new BusinessException("抛出错误");
            }

            await Task.CompletedTask;
        }
    }
}
