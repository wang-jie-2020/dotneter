using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Services;

namespace Hugin.BookStore
{
    public interface IAuthorManager : IDomainService
    {
        Task<Author> CreateAsync([NotNull] string name, DateTime birthDate, [CanBeNull] string profile = null);

        Task ChangeNameAsync([NotNull] Author author, [NotNull] string newName);
    }
}
