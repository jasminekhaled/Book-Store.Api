using Shopping.Models.AuthModule;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models.BookModule
{
    public class BookUsers
    {
        public int id { get; set; }
        public int bookId { get; set; }
        public int userId { get; set; }

        [ForeignKey("bookId")]
        public Book Book { get; set; }
        [ForeignKey("userId")]
        public User User { get; set; }
    }
}
