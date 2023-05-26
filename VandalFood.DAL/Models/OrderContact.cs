using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class OrderContact
{
    public int CustomerOrderId { get; set; }

    public int ContactTypeId { get; set; }

    public string Value { get; set; } = null!;

    public virtual ContactType ContactType { get; set; } = null!;

    public virtual CustomerOrder CustomerOrder { get; set; } = null!;
}
