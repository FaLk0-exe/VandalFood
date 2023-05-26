using System;
using System.Collections.Generic;

namespace VandalFood.DAL.Models;

public partial class RoleType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Operator> Operators { get; set; } = new List<Operator>();
}
