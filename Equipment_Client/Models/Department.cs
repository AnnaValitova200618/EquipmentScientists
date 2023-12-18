using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Laboratory> Laboratories { get; } = new List<Laboratory>();
}
