using Equipment_Client.Models;
using System;
using System.Collections.Generic;

namespace Equipment_Client;

public partial class FhotoPath
{
    public int Id { get; set; }

    public byte[] Fhoto { get; set; } = null!;

    public int IdReport { get; set; }

    public int IdScientist { get; set; }

    public virtual Report IdReportNavigation { get; set; } = null!;
}
