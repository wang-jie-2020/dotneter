using System.Threading.Tasks;
using Hugin.BookStore.Localization;
using Hugin.BookStore.Permissions;
using Volo.Abp.UI.Navigation;

namespace Hugin.BookStoreWeb.Web
{
    public class BookStoreMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<BookStoreResource>();

            var bookStoreMenu = new ApplicationMenuItem(name: "BooksStore", displayName: l["Menu:BookStore"], icon: "fa fa-book");
            context.Menu.AddItem(bookStoreMenu);

            if (await context.IsGrantedAsync(BookStorePermissions.Book.Default))
            {
                bookStoreMenu.AddItem(new ApplicationMenuItem(name: "BooksStore.Books", displayName: l["Menu:Books"], url: "/BookStore/Books"));
            }
        }
    }
}

