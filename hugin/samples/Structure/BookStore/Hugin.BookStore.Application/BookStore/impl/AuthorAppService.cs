using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hugin.BookStore.Dtos;
using Hugin.BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace Hugin.BookStore.impl
{
    [SwaggerTag("NoBased Api")]
    [Authorize(BookStorePermissions.Author.Default)]
    public class AuthorAppService
        : BaseAppService, IAuthorAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorManager _authorManager;

        public AuthorAppService(IAuthorRepository authorRepository, IAuthorManager authorManager)
        {
            _authorRepository = authorRepository;
            _authorManager = authorManager;
        }

        public async Task<AuthorDto> GetAsync(Guid id)
        {
            var author = await _authorRepository.GetAsync(id);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        public async Task<PagedResultDto<AuthorDto>> GetListAsync(AuthorQueryInput input)
        {
            if (input.Sorting.IsNullOrWhiteSpace())
            {
                input.Sorting = nameof(Author.Name);
            }

            //abp示例就这么写的
            var authors = await _authorRepository.GetListAsync(input.SkipCount, input.MaxResultCount, input.Sorting, input.Filter);
            var totalCount = await AsyncExecuter.CountAsync<Author>(
                    _authorRepository.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    author => author.Name.Contains(input.Filter)));

            return new PagedResultDto<AuthorDto>(totalCount, ObjectMapper.Map<List<Author>, List<AuthorDto>>(authors));
        }

        [Authorize(BookStorePermissions.Author.Create)]
        public async Task<AuthorDto> CreateAsync(AuthorEditDto input)
        {
            var author = await _authorManager.CreateAsync(input.Name, input.BirthDate, input.Profile);
            await _authorRepository.InsertAsync(author);
            return ObjectMapper.Map<Author, AuthorDto>(author);
        }

        [Authorize(BookStorePermissions.Author.Edit)]
        public async Task UpdateAsync(Guid id, AuthorEditDto input)
        {
            var author = await _authorRepository.GetAsync(id);
            if (author.Name != input.Name)
            {
                await _authorManager.ChangeNameAsync(author, input.Name);
            }

            author.BirthDate = input.BirthDate;
            author.Profile = input.Profile;
            await _authorRepository.UpdateAsync(author);
        }

        [Authorize(BookStorePermissions.Author.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _authorRepository.DeleteAsync(id);
        }
    }
}
