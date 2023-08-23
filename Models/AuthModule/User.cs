using Microsoft.EntityFrameworkCore;
using Shopping.Models.BookModule;
using Shopping.Models.CartModule;
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
        public List<BookUsers> bookUsers { get; set; }
        public Cart Cart { get; set; }

    }
}
