using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class ContactType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<CustomerContact> CustomerContacts { get; set; } = new List<CustomerContact>();

    public virtual ICollection<OrderContact> OrderContacts { get; set; } = new List<OrderContact>();
}
