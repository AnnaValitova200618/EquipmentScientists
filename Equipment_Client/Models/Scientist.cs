using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment_Client.Models;

public partial class Scientist
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdPosition { get; set; }

    public DateTime? DismissalDate { get; set; }

    public int IdLaboratoty { get; set; }

    [NotMapped]
    public string FIO
    {
        get
        {
            string fio = $"{Lastname} {(Firstname != "" ? (Firstname[0] + ".") : "")}{(Patronymic != "" ? (Patronymic[0] + ".") : "")}";
            return fio;
        }
    }

    [NotMapped]
    public bool Choice {  get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<Equipment> Equipment { get; } = new List<Equipment>();

    public virtual Laboratory IdLaboratotyNavigation { get; set; } = null!;

    public virtual Position IdPositionNavigation { get; set; } = null!;

}
