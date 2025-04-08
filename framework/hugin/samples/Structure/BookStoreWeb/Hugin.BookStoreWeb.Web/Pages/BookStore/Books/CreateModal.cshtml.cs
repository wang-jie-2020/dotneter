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
    public class CreateModalModel : BasePageModel
    {
        [BindProperty]
        public CreateBookViewModel Book { get; set; }

        public List<SelectListItem> Authors { get; set; }

        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;

        public CreateModalModel(
            IBookAppService bookAppService,
            IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
        }

        public async Task OnGetAsync()
        {
            Book = new CreateBookViewModel();

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
            await _bookAppService.CreateAsync(ObjectMapper.Map<CreateBookViewModel, BookEditDto>(Book));
            return NoContent();
        }

        public class CreateBookViewModel
        {
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
