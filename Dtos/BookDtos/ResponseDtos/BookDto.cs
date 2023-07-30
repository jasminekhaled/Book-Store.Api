using Shopping.Models;
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
        public int Price { get; set; }

        [MaxLength(length: 150)]
        public string Author { get; set; }
        public int Rate { get; set; }
        public byte[] Poster { get; set; }
        public int Year { get; set; }
        public List<Category> Categories { get; set; }
    }
}
