using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.CartsDtos.ResponseDtos
{
    public class CartDto
    {
        [MaxLength(length: 200)]
        public string Title { get; set; }

        [MaxLength(length: 150)]
        public string Author { get; set; }
        public int WantedCopies { get; set; }
        public double Price { get; set; }
        
   
    }
}
