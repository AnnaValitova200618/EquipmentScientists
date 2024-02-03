using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class ReplacementOfConsumable
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int IdReport { get; set; }

    public virtual Report IdReportNavigation { get; set; } = null!;
}
