using System;

namespace Hugin.BookStore.Etos
{
    public class BookEventArgs
    {
        public Guid BookId { get; set; }

        public string Message => "Book Event";
    }
}
