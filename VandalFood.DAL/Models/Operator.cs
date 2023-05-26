using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class Operator
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string LeftName { get; set; } = null!;

    public string RightName { get; set; } = null!;

    public int RoleTypeId { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    public virtual RoleType RoleType { get; set; } = null!;
}
