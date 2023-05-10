using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class PurposeOfUse
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();
}
