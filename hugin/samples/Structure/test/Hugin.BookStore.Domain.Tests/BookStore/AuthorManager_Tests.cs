using Hugin.BookStore.impl;

namespace Hugin.BookStore
{
    public class AuthorManager_Tests : DomainTestBase
    {
        private readonly AuthorManager _authorManager;

        public AuthorManager_Tests()
        {
            _authorManager = GetRequiredService<AuthorManager>();
        }

        //[Fact]
        //public async Task Should_Not_Allow_To_Create_Duplicate_Author()
        //{
        //    await Assert.ThrowsAsync<AuthorAlreadyExistsException>(async () =>
        //    {
        //        await _authorManager.ChangeNameAsync(
        //            new Author(Guid.Empty, "Douglas Adams", DateTime.Now), "new name"
        //        );
        //    });
        //}
    }
}