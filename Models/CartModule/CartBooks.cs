using Shopping.Models.AuthModule;
using Shopping.Models.BookModule;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.CartModule
{
    public class CartBooks
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CartId { get; set; } 
        public int WantedCopies { get; set; }
        public double Price { get; set; }

        [ForeignKey("cartId")]
        public Cart Cart { get; set; }

        [ForeignKey("bookId")]
        public Book Book { get; set; }
    }
}
