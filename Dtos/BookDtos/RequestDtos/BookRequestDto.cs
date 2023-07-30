using Shopping.Models;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.BookDtos.RequestDtos
{
    public class BookRequestDto
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
        public IFormFile Poster { get; set; }
        public int Year { get; set; }
        public List<Category> Categories { get; set; }
    }
}
