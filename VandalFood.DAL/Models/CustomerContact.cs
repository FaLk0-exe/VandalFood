using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class CustomerContact
{
    public int CustomerId { get; set; }

    public int ContactTypeId { get; set; }

    public string Value { get; set; } = null!;
}
