using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class OrderItem
{
    public int ProductId { get; set; }

    public int CustomerOrderId { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public virtual CustomerOrder CustomerOrder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
