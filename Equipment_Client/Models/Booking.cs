using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment_Client.Models;

public partial class Booking
{
    private bool approvedID;

    public int Id { get; set; }

    public int IdScientist { get; set; }

    public int IdEquipment { get; set; }

    public DateTime DateStart { get; set; } = DateTime.Now;

    public DateTime DateEnd { get; set; } = DateTime.Now;

    public int IdPurposeOfUse { get; set; }

    public int? IdConfirmation { get; set; }

    public byte? Approved { get; set; }
    [NotMapped]
    public bool ApprovedID 
    {
        get => Approved == 1; 
        set => Approved = value ? (byte)1 : (byte)0; }

    public virtual Equipment IdEquipmentNavigation { get; set; } = null!;

    public virtual PurposeOfUse IdPurposeOfUseNavigation { get; set; } = null!;

    public virtual Scientist IdScientistNavigation { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; } = new List<Report>();
}
