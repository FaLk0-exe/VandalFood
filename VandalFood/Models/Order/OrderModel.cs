using VandalFood.Models.Cart;

namespace VandalFood.Models.Order
{
    public class OrderModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string? Mail { get; set; }
        public string Phone { get; set; }

        public IEnumerable<CartModel> Items { get; set; }
    }
}
