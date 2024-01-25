using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Repair
{
    public int Id { get; set; }

    public DateTime DateStartDowntime { get; set; } = DateTime.Now;

    public DateTime DateEndDowntime { get; set; } = DateTime.Now;

    public int IdReport { get; set; }

    public virtual Report IdReportNavigation { get; set; } = null!;
}
