using Shopping.Models.BookModule;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.BookDtos.ResponseDtos
{
    public class BookDto
    {
        [MaxLength(length: 200)]
        public string Title { get; set; }

        [MaxLength(length: 1000)]
        public String Description { get; set; }
        public int NumOfCopies { get; set; }
        public double Price { get; set; }

        [MaxLength(length: 150)]
        public string Author { get; set; }
        public double Rate { get; set; }
        public byte[] Poster { get; set; }
        public int Year { get; set; }
        public List<int> categoryId { get; set; }
        public List<string> categoryName { get; set; }
    }
}
