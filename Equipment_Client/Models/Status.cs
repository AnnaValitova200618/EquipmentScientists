using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Equipment> Equipment { get; } = new List<Equipment>();
}
