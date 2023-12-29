using Volo.Abp;

namespace Hugin.BookStore.Errors
{
    public class AuthorAlreadyExistsException : BusinessException
    {
        public const string AuthorAlreadyExists = BookStoreConsts.Name + ":AuthorAlreadyExists";

        public AuthorAlreadyExistsException(string name) : base(AuthorAlreadyExists)
        {
            WithData("name", name);
        }
    }
}
