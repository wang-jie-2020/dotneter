using System;
using System.Linq;
using System.Threading.Tasks;
using Hugin.BookStore.Dtos;
using Hugin.BookStore.Enums;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Validation;
using Xunit;

namespace Hugin.BookStore
{
    public class BookAppService_Tests : ApplicationTestBase
    {
        private readonly IBookAppService _bookAppService;

        public BookAppService_Tests()
        {
            _bookAppService = GetRequiredService<IBookAppService>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Books()
        {
            //Act
            var result = await _bookAppService.GetListAsync(
                new PagedAndSortedResultRequestDto()
            );

            //Assert
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(b => b.Name == "1984");
        }

        [Fact]
        public async Task Should_Create_A_Valid_Book()
        {
            //Act
            var result = await _bookAppService.CreateAsync(
                new BookEditDto
                {
                    Name = "New test book 42",
                    Price = 10,
                    PublishDate = DateTime.Now,
                    BookType = BookType.Horror
                }
            );

            //Assert
            result.Id.ShouldNotBe(Guid.Empty);
            result.Name.ShouldBe("New test book 42");
        }

        [Fact]
        public async Task Should_Not_Create_A_Book_Without_Name()
        {
            var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
            {
                await _bookAppService.CreateAsync(
                    new BookEditDto
                    {
                        Name = "",
                        Price = 10,
                        PublishDate = DateTime.Now,
                        BookType = BookType.Adventure
                    }
                );
            });

            exception.ValidationErrors
                .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
        }
    }
}
