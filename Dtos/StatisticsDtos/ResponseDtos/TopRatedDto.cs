using Shopping.Models.BookModule;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.StatisticsDtos.ResponseDtos
{
    public class TopRatedDto
    {
        public int id { get; set; }
        [MaxLength(length: 200)]
        public string Title { get; set; }

        [MaxLength(length: 1000)]
        public string Description { get; set; }
        public int NumOfCopies { get; set; }
        public double Price { get; set; }

        [MaxLength(length: 150)]
        public string Author { get; set; }
        public double Rate { get; set; }
        
        public int Year { get; set; }
        public List<string> Categories { get; set; }
        public byte[] Poster { get; set; }
    }
}
