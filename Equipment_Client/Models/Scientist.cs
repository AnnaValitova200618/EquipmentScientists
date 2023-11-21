using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment_Client.Models;

public partial class Scientist
{
    private string fIO;

    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    [NotMapped]
    public string FIO { get; set; } 

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdPosition { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<Equipment> Equipment { get; } = new List<Equipment>();

    public virtual Position IdPositionNavigation { get; set; } = null!;

   
}
