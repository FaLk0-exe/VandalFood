using VandalFood.DAL.Models;

namespace VandalFood.Models.Cart
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
