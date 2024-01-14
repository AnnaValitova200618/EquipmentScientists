using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Laboratory
{
    public int Id { get; set; }

    public int IdDepartment { get; set; }

    public string Number { get; set; } = null!;

    public string Title { get; set; } = null!;

    public virtual Department IdDepartmentNavigation { get; set; } = null!;

    public virtual ICollection<Scientist> Scientists { get; } = new List<Scientist>();
}
