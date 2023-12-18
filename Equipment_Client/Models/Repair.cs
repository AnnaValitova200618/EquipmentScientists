using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Repair
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime DateStartDowntime { get; set; }

    public DateTime DateEndDowntime { get; set; }

    public int IdReport { get; set; }

    public virtual Report IdReportNavigation { get; set; } = null!;
}
