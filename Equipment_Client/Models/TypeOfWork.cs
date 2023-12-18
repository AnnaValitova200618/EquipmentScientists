using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class TypeOfWork
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; } = new List<Report>();
}
