using System;

namespace Hugin.BookStore.Etos
{
    public class BookErrorEventArgs
    {
        public Guid BookId { get; set; }

        public string Message => "Book Error Event";
    }
}
