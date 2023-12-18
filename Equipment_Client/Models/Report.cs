using System;
using System.Collections.Generic;

namespace Equipment_Client.Models;

public partial class Report
{
    public int Id { get; set; }

    public int IdBooking { get; set; }

    public string Condition { get; set; } = null!;

    public byte? AvailabilityZip { get; set; }

    public byte? AvailabilityСonsumable { get; set; }

    public DateTime DateStartFact { get; set; }

    public DateTime DateEndFact { get; set; }

    public DateTime DateFirstUseEquipment { get; set; }

    public int IdPlaceOfUse { get; set; }

    public string DescriptionPlaceOfUse { get; set; } = null!;

    public string NumberDocument { get; set; } = null!;

    public int IdTypeOfWork { get; set; }

    public int NumberOfMeasurements { get; set; }

    public string CharacteristicsWork { get; set; } = null!;

    public DateTime DateLastUseEquipment { get; set; }

    public DateTime DateSigningReportReponsible { get; set; }

    public DateTime DateReturn { get; set; }

    public string StatusEquipment { get; set; } = null!;

    public string? ConflictSituationScientists { get; set; }

    public string? ConflictSituationResponsible { get; set; }

    public DateTime? DateSigningReportScientists { get; set; }

    public virtual ICollection<FhotoPath> FhotoPaths { get; } = new List<FhotoPath>();

    public virtual Booking IdBookingNavigation { get; set; } = null!;

    public virtual PlaceOfUse IdPlaceOfUseNavigation { get; set; } = null!;

    public virtual TypeOfWork IdTypeOfWorkNavigation { get; set; } = null!;

    public virtual ICollection<Repair> Repairs { get; } = new List<Repair>();

    public virtual ICollection<ReplacementOfConsumable> ReplacementOfConsumables { get; } = new List<ReplacementOfConsumable>();

    public virtual ICollection<ReportCrossScientist> ReportCrossScientists { get; } = new List<ReportCrossScientist>();
}
