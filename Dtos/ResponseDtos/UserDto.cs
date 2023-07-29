using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos.ResponseDtos
{
    public class UserDto
    {

        [MaxLength(length: 30)]
        [MinLength(length: 5)]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }


    }
}
