using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class CustomerOrder
{
    public int Id { get; set; }

    public int OperatorId { get; set; }

    public int OrderStatusId { get; set; }

    public DateTime OrderDate { get; set; }

    public string CustomerName { get; set; } = null!;

    public virtual Operator Operator { get; set; } = null!;

    public virtual ICollection<OrderContact> OrderContacts { get; set; } = new List<OrderContact>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
