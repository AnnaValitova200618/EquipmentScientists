using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Position
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Scientist> Scientists { get; } = new List<Scientist>();
}
