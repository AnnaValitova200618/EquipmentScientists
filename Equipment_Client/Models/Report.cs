using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equipment_Client.Models;

public partial class Report
{
    public int Id { get; set; }

    public int IdBooking { get; set; }

    public string Condition { get; set; } = null!;

    public byte? AvailabilityZip { get; set; }
    [NotMapped]
    public bool AvailabilityZipID
    {
        get => AvailabilityZip == 1;
        set => AvailabilityZip = value ? (byte)1 : (byte)0;
    }

    public byte? AvailabilityСonsumable { get; set; }
    [NotMapped]
    public bool AvailabilityСonsumableID
    {
        get => AvailabilityСonsumable == 1;
        set => AvailabilityСonsumable = value ? (byte)1 : (byte)0;
    }
    public string? Operators { get; set; }
    public DateTime DateStartFact { get; set; } = DateTime.Now;

    public DateTime DateEndFact { get; set; } = DateTime.Now;

    public DateTime DateFirstUseEquipment { get; set; } = DateTime.Now;

    public int IdPlaceOfUse { get; set; }

    public string DescriptionPlaceOfUse { get; set; } = null!;

    public string NumberDocument { get; set; } = null!;

    public int IdTypeOfWork { get; set; }

    public string NumberOfMeasurements { get; set; } = null!;

    public string CharacteristicsWork { get; set; } = null!;

    public DateTime DateLastUseEquipment { get; set; } = DateTime.Now;

    public DateTime DateSigningReportScientists { get; set; }

    public DateTime? DateReturn { get; set; } 

    public string? StatusEquipment { get; set; }

    public string? ConflictSituationScientists { get; set; }

    public string? ConflictSituationResponsible { get; set; }

    public DateTime? DateSigningReportReponsibleScientists { get; set; }

    public virtual ICollection<FhotoPath> FhotoPaths { get; } = new List<FhotoPath>();

    public virtual Booking IdBookingNavigation { get; set; } = null!;

    public virtual PlaceOfUse IdPlaceOfUseNavigation { get; set; } = null!;

    public virtual TypeOfWork IdTypeOfWorkNavigation { get; set; } = null!;

    public virtual ICollection<Repair> Repairs { get; } = new List<Repair>();

    public virtual ICollection<ReplacementOfConsumable> ReplacementOfConsumables { get; } = new List<ReplacementOfConsumable>();

}
