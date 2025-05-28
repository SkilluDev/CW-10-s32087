using System;
using System.Collections.Generic;
using APBD10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Data;

public partial class s32087 : DbContext
{
    public s32087(DbContextOptions<s32087> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Medicament> Medicaments { get; set; }

    public virtual DbSet<Partium> Partia { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Polityk> Polityks { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public virtual DbSet<Przynaleznosc> Przynaleznoscs { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("s32087");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Client__C1961B3371422BB0");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("PK__Client_T__C823521EEF0BE314");

            entity.ToTable("Client_Trip");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Client_Tr__IdCli__452BE1DF");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Client_Tr__IdTri__46200618");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("PK__Country__F99F104DF2CA5D03");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Country_T__IdTri__424F7534"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Country_T__IdCou__415B50FB"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("PK__Country___F02A59602983F958");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor);

            entity.ToTable("Doctor");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(e => e.IdMedicament);

            entity.ToTable("Medicament");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Partium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Partia_pk");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataZalozenia).HasColumnType("datetime");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Skrot)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient);

            entity.ToTable("Patient");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Polityk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Polityk_pk");

            entity.ToTable("Polityk");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Imie)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nazwisko)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Powiedzenie)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription);

            entity.ToTable("Prescription");

            entity.HasIndex(e => e.IdDoctor, "IX_Prescription_IdDoctor");

            entity.HasIndex(e => e.IdPatient, "IX_Prescription_IdPatient");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Prescriptions).HasForeignKey(d => d.IdDoctor);

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Prescriptions).HasForeignKey(d => d.IdPatient);
        });

        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.HasKey(e => new { e.IdPrescription, e.IdMedicament });

            entity.ToTable("PrescriptionMedicament");

            entity.HasIndex(e => e.IdMedicament, "IX_PrescriptionMedicament_IdMedicament");

            entity.Property(e => e.Details).HasMaxLength(100);

            entity.HasOne(d => d.IdMedicamentNavigation).WithMany(p => p.PrescriptionMedicaments).HasForeignKey(d => d.IdMedicament);

            entity.HasOne(d => d.IdPrescriptionNavigation).WithMany(p => p.PrescriptionMedicaments).HasForeignKey(d => d.IdPrescription);
        });

        modelBuilder.Entity<Przynaleznosc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Przynaleznosc_pk");

            entity.ToTable("Przynaleznosc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Do).HasColumnType("datetime");
            entity.Property(e => e.Od).HasColumnType("datetime");
            entity.Property(e => e.PartiaId).HasColumnName("Partia_ID");
            entity.Property(e => e.PolitykId).HasColumnName("Polityk_ID");

            entity.HasOne(d => d.Partia).WithMany(p => p.Przynaleznoscs)
                .HasForeignKey(d => d.PartiaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Przynaleznosc_Partia");

            entity.HasOne(d => d.Polityk).WithMany(p => p.Przynaleznoscs)
                .HasForeignKey(d => d.PolitykId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Przynaleznosc_Polityk");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("PK__Trip__9B5492D1960B0BEE");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
