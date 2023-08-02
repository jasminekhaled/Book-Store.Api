using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.AuthModule
{
    public class User
    {
        public int id { get; set; }

        [MaxLength(length: 30)]
        [MinLength(length: 5)]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        public string Password { get; set; }
    }
}
