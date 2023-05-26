using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string LeftName { get; set; } = null!;

    public virtual ICollection<CustomerContact> CustomerContacts { get; set; } = new List<CustomerContact>();
}
