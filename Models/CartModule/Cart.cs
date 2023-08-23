using Shopping.Models.AuthModule;

namespace Shopping.Models.CartModule
{
    public class Cart
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public List<CartBooks> cartBooks { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

    }
}
