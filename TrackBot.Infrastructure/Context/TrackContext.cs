using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TrackBot.Domain.Models;

namespace TrackBot.Infrastructure.Context
{
    public partial class TrackContext : DbContext
    {
        public TrackContext()
        {
        }

        public TrackContext(DbContextOptions<TrackContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TrackLocation> TrackLocations { get; set; }
        public virtual DbSet<UserLanguagePreference> UserLanguagePreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackLocation>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_TackLocation_id");

                entity.ToTable("TrackLocation");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DateEvent).HasColumnType("datetime");
                entity.Property(e => e.DateTrack)
                    .HasColumnType("datetime")
                    .HasColumnName("date_track");
                entity.Property(e => e.Imei)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IMEI");
                entity.Property(e => e.Latitude)
                    .HasColumnType("decimal(12, 9)")
                    .HasColumnName("latitude");
                entity.Property(e => e.Longitude)
                    .HasColumnType("decimal(12, 9)")
                    .HasColumnName("longitude");
                entity.Property(e => e.TypeSource).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserLanguagePreference>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("UserLanguagePreference");

                entity.Property(e => e.LanguageCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("language_code");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}