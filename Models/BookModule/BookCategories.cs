using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.BookModule
{
    public class BookCategories
    {
        public int Id { get; set; }
        public int bookId { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("bookId")]
        public Book Book { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
}
