using System;
using System.Collections.Generic;
using Equipment_Client.Models;
using Microsoft.EntityFrameworkCore;

namespace Equipment_Client;

public partial class EquipmentContext : DbContext
{
    public EquipmentContext()
    {
    }

    public EquipmentContext(DbContextOptions<EquipmentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<FhotoPath> FhotoPaths { get; set; }

    public virtual DbSet<Laboratory> Laboratories { get; set; }

    public virtual DbSet<PlaceOfUse> PlaceOfUses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<PurposeOfUse> PurposeOfUses { get; set; }

    public virtual DbSet<Repair> Repairs { get; set; }

    public virtual DbSet<ReplacementOfConsumable> ReplacementOfConsumables { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Scientist> Scientists { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Models.Type> Types { get; set; }

    public virtual DbSet<TypeOfWork> TypeOfWorks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=Equipment;Integrated Security=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Booking");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateEnd).HasColumnType("date");
            entity.Property(e => e.DateStart).HasColumnType("date");
            entity.Property(e => e.IdEquipment).HasColumnName("ID_Equipment");
            entity.Property(e => e.IdPurposeOfUse).HasColumnName("ID_PurposeOfUse");
            entity.Property(e => e.IdScientist).HasColumnName("ID_Scientist");

            entity.HasOne(d => d.IdEquipmentNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdEquipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Equipment");

            entity.HasOne(d => d.IdPurposeOfUseNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdPurposeOfUse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_PurposeOfUse");

            entity.HasOne(d => d.IdScientistNavigation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.IdScientist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_Scientists");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdReponsibleScientists).HasColumnName("ID_ReponsibleScientists");
            entity.Property(e => e.IdStatus).HasColumnName("ID_Status");
            entity.Property(e => e.IdType).HasColumnName("ID_Type");

            entity.HasOne(d => d.IdReponsibleScientistsNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdReponsibleScientists)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Equipment_Scientists1");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Equipment_Status");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("FK_Equipment_Type");
        });

        modelBuilder.Entity<FhotoPath>(entity =>
        {
            entity.ToTable("FhotoPath");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdReport).HasColumnName("ID_Report");
            entity.Property(e => e.IdScientist).HasColumnName("ID_Scientist");

            entity.HasOne(d => d.IdReportNavigation).WithMany(p => p.FhotoPaths)
                .HasForeignKey(d => d.IdReport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FhotoPath_Report");
        });

        modelBuilder.Entity<Laboratory>(entity =>
        {
            entity.ToTable("Laboratory");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdDepartment).HasColumnName("ID_Department");

            entity.HasOne(d => d.IdDepartmentNavigation).WithMany(p => p.Laboratories)
                .HasForeignKey(d => d.IdDepartment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Laboratory_Department");
        });

        modelBuilder.Entity<PlaceOfUse>(entity =>
        {
            entity.ToTable("PlaceOfUse");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("Position");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<PurposeOfUse>(entity =>
        {
            entity.ToTable("PurposeOfUse");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Repair>(entity =>
        {
            entity.ToTable("Repair");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateEndDowntime).HasColumnType("datetime");
            entity.Property(e => e.DateStartDowntime).HasColumnType("datetime");
            entity.Property(e => e.IdReport).HasColumnName("ID_Report");

            entity.HasOne(d => d.IdReportNavigation).WithMany(p => p.Repairs)
                .HasForeignKey(d => d.IdReport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repair_Report");
        });

        modelBuilder.Entity<ReplacementOfConsumable>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdReport).HasColumnName("ID_Report");

            entity.HasOne(d => d.IdReportNavigation).WithMany(p => p.ReplacementOfConsumables)
                .HasForeignKey(d => d.IdReport)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReplacementOfConsumables_Report");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AvailabilityZip).HasColumnName("AvailabilityZIP");
            entity.Property(e => e.DateEndFact).HasColumnType("date");
            entity.Property(e => e.DateFirstUseEquipment).HasColumnType("date");
            entity.Property(e => e.DateLastUseEquipment).HasColumnType("date");
            entity.Property(e => e.DateReturn).HasColumnType("date");
            entity.Property(e => e.DateSigningReportReponsibleScientists).HasColumnType("date");
            entity.Property(e => e.DateSigningReportScientists).HasColumnType("date");
            entity.Property(e => e.DateStartFact).HasColumnType("date");
            entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");
            entity.Property(e => e.IdPlaceOfUse).HasColumnName("ID_PlaceOfUse");
            entity.Property(e => e.IdTypeOfWork).HasColumnName("ID_TypeOfWork");

            entity.HasOne(d => d.IdBookingNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.IdBooking)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Booking");

            entity.HasOne(d => d.IdPlaceOfUseNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.IdPlaceOfUse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_PlaceOfUse");

            entity.HasOne(d => d.IdTypeOfWorkNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.IdTypeOfWork)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Report_Plan");
        });

        modelBuilder.Entity<Scientist>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DismissalDate).HasColumnType("date");
            entity.Property(e => e.IdLaboratoty).HasColumnName("ID_Laboratoty");
            entity.Property(e => e.IdPosition).HasColumnName("ID_Position");

            entity.HasOne(d => d.IdLaboratotyNavigation).WithMany(p => p.Scientists)
                .HasForeignKey(d => d.IdLaboratoty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Scientists_Laboratory");

            entity.HasOne(d => d.IdPositionNavigation).WithMany(p => p.Scientists)
                .HasForeignKey(d => d.IdPosition)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Scientists_Position");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Models.Type>(entity =>
        {
            entity.ToTable("Type");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<TypeOfWork>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Plan");

            entity.ToTable("TypeOfWork");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
