using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.BookDtos.RequestDtos
{
    public class AddCategoryDto
    {
        [MaxLength(length: 50)]
        [MinLength(length: 3)]
        public string Name { get; set; }
    }
}
