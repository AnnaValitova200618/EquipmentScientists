using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Report
{
    public int Id { get; set; }

    public int IdBooking { get; set; }

    public int IdPlan { get; set; }

    public virtual Booking IdBookingNavigation { get; set; } = null!;

    public virtual Plan IdPlanNavigation { get; set; } = null!;
}
