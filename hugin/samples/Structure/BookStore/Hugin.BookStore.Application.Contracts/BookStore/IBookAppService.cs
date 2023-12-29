using System;
using Hugin.BookStore.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Hugin.BookStore
{
    /*
     * 虽然abp提供了一些基础封装ICrudAppService，但这些封装不太适合直接使用
     * 在提供了抽象基类的情况下，没什么必要在接口中再重复定义crud了
     */
    public interface IBookAppService : ICrudAppService<BookDto, Guid, PagedAndSortedResultRequestDto, BookEditDto>
    {

    }
}
