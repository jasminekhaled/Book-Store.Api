using System.ComponentModel.DataAnnotations;

namespace Shopping.Dtos
{
    public class LogInDto
    {
      

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        public string Password { get; set; }
    
}
}
