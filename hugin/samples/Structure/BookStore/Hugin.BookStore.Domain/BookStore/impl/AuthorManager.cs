using System;
using System.Threading.Tasks;
using Hugin.BookStore.Errors;
using JetBrains.Annotations;
using Volo.Abp;

namespace Hugin.BookStore.impl
{
    public class AuthorManager : BaseDomainManager, IAuthorManager
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorManager(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        /*
         * 在仅针对crud操作时，这种封装不是特别实际
         * 但从设计上说这样做是应该的
         */
        public async Task<Author> CreateAsync([NotNull] string name, DateTime birthDate, [CanBeNull] string profile = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingAuthor = await _authorRepository.FindByNameAsync(name);
            if (existingAuthor != null)
            {
                throw new AuthorAlreadyExistsException(name);
            }

            return new Author(GuidGenerator.Create(), name, birthDate, profile);
        }

        public async Task ChangeNameAsync([NotNull] Author author, [NotNull] string newName)
        {
            Check.NotNull(author, nameof(author));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existingAuthor = await _authorRepository.FindByNameAsync(newName);
            if (existingAuthor != null && existingAuthor.Id != author.Id)
            {
                throw new AuthorAlreadyExistsException(newName);
            }

            author.ChangeName(newName);
        }
    }
}
