using Shopping.Models.AuthModule;
using Shopping.Models.CartModule;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.BookModule
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(length: 200)]
        public string Title { get; set; }

        [MaxLength(length: 1000)]
        public string Description { get; set; }
        public int NumOfCopies { get; set; }
        public double Price { get; set; }

        [MaxLength(length: 150)]
        public string Author { get; set; }
        public double Rate { get; set; }
        public byte[] Poster { get; set; }
        public int Year { get; set; }
        public List<BookCategories> bookCategories { get; set; }
        public List<BookUsers> bookUsers { get; set; }
        public List<CartBooks> cartBooks { get; set; }

    }
}
