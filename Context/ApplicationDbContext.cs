using Microsoft.EntityFrameworkCore;
using Shopping.Models.AuthModule;
using Shopping.Models.BookModule;

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
    }
}
