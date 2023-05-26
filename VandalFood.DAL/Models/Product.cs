using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class Product
{
    public int Id { get; set; }

    public bool? IsActive { get; set; }

    public string Description { get; set; } = null!;

    public int Weight { get; set; }
    public string Title { get; set; }

    public decimal Price { get; set; }

    public int ProductTypeId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductType ProductType { get; set; } = null!;
}
 