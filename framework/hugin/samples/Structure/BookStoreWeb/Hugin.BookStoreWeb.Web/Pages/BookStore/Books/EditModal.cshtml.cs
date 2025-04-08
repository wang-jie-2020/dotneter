using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hugin.BookStore;
using Hugin.BookStore.Dtos;
using Hugin.BookStore.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Hugin.BookStoreWeb.Web.Pages.BookStore.Books
{
    public class EditModalModel : BasePageModel
    {
        [BindProperty]
        public EditBookViewModel Book { get; set; }

        public List<SelectListItem> Authors { get; set; }

        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;

        public EditModalModel(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
        }

        public async Task OnGetAsync(Guid id)
        {
            var bookDto = await _bookAppService.GetAsync(id);
            Book = ObjectMapper.Map<BookDto, EditBookViewModel>(bookDto);

            var authorLookup = await _authorAppService.GetListAsync(new AuthorQueryInput
            {
                Filter = "",
                MaxResultCount = 999,
                SkipCount = 0,
                Sorting = ""
            });
            Authors = authorLookup.Items
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _bookAppService.UpdateAsync(Book.Id, ObjectMapper.Map<EditBookViewModel, BookEditDto>(Book));
            return NoContent();
        }

        public class EditBookViewModel
        {
            [HiddenInput]
            public Guid Id { get; set; }

            [SelectItems(nameof(Authors))]
            [DisplayName("Author")]
            public Guid AuthorId { get; set; }

            [Required]
            [StringLength(128)]
            public string Name { get; set; }

            [Required]
            public BookType BookType { get; set; } = BookType.Undefined;

            [Required]
            [DataType(DataType.Date)]
            public DateTime PublishDate { get; set; } = DateTime.Now;

            [Required]
            public decimal Price { get; set; }
        }
    }
}
