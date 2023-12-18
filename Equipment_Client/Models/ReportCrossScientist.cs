using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class ReportCrossScientist
{
    public int Id { get; set; }

    public int IdReport { get; set; }

    public int IdScientist { get; set; }

    public virtual Report IdReportNavigation { get; set; } = null!;

    public virtual Scientist IdScientistNavigation { get; set; } = null!;
}
