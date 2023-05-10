using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment_Client.Models;

public partial class Equipment
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Dimansions { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public int? IdReponsibleScientists { get; set; }

    public int IdStatus { get; set; }
    [NotMapped]
    public bool ReadOnly { get => IdStatus == 5 || IdStatus == 7; }

    public int? IdType { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Scientist? IdReponsibleScientistsNavigation { get; set; }

    public virtual Status IdStatusNavigation { get; set; } = null!;

    public virtual Type? IdTypeNavigation { get; set; }
}
