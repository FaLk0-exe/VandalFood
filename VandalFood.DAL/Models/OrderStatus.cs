using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
}
