using Microsoft.EntityFrameworkCore;
using Shopping.Models.AuthModule;
using Shopping.Models.BookModule;
using Shopping.Models.CartModule;

namespace Shopping.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategories> bookCategories { get; set; }
        public DbSet<BookUsers> bookUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartBooks> CartBooks { get; set; }
    }
}
