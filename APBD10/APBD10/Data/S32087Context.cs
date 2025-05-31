using System;
using System.Collections.Generic;
using APBD10.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD10.Data;

public partial class S32087Context : DbContext
{
    public S32087Context(DbContextOptions<S32087Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }
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
