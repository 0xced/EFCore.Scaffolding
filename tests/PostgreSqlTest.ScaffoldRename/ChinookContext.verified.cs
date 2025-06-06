﻿// <auto-generated>
//
// ┌───────────────────┬────────────────────────────────────────────────────┐
// │ Provider          │ Npgsql.EntityFrameworkCore.PostgreSQL              │
// ├───────────────────┼────────────────────────────────────────────────────┤
// │ Connection String │ Host=127.0.0.1;Database=postgres;Username=postgres │
// └───────────────────┴────────────────────────────────────────────────────┘
// 
// </auto-generated>

#nullable enable
#pragma warning disable

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ScaffoldRename;

public partial class ChinookContext : DbContext
{
    public ChinookContext(DbContextOptions<ChinookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<MediaType> MediaTypes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("album_pkey");

            entity.ToTable("album");

            entity.HasIndex(e => e.ArtistId, "album_artist_id_idx");

            entity.Property(e => e.AlbumId)
                .ValueGeneratedNever()
                .HasColumnName("album_id");
            entity.Property(e => e.ArtistId).HasColumnName("artist_id");
            entity.Property(e => e.Title)
                .HasMaxLength(160)
                .HasColumnName("title");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("album_artist_id_fkey");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("artist_pkey");

            entity.ToTable("artist");

            entity.Property(e => e.ArtistId)
                .ValueGeneratedNever()
                .HasColumnName("artist_id");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.HasIndex(e => e.SupportRepId, "customer_support_rep_id_idx");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(70)
                .HasColumnName("address");
            entity.Property(e => e.City)
                .HasMaxLength(40)
                .HasColumnName("city");
            entity.Property(e => e.Company)
                .HasMaxLength(80)
                .HasColumnName("company");
            entity.Property(e => e.Country)
                .HasMaxLength(40)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.Fax)
                .HasMaxLength(24)
                .HasColumnName("fax");
            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
            entity.Property(e => e.State)
                .HasMaxLength(40)
                .HasColumnName("state");
            entity.Property(e => e.SupportRepId).HasColumnName("support_rep_id");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");

            entity.HasOne(d => d.SupportRep).WithMany(p => p.Clients)
                .HasForeignKey(d => d.SupportRepId)
                .HasConstraintName("customer_support_rep_id_fkey");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.ReportsTo, "employee_reports_to_idx");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(70)
                .HasColumnName("address");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birth_date");
            entity.Property(e => e.City)
                .HasMaxLength(40)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(40)
                .HasColumnName("country");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .HasColumnName("email");
            entity.Property(e => e.Fax)
                .HasMaxLength(24)
                .HasColumnName("fax");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnName("phone");
            entity.Property(e => e.ReportsTo).HasColumnName("reports_to");
            entity.Property(e => e.State)
                .HasMaxLength(40)
                .HasColumnName("state");
            entity.Property(e => e.Title)
                .HasMaxLength(30)
                .HasColumnName("title");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .HasColumnName("postal_code");

            entity.HasOne(d => d.ReportsToNavigation).WithMany(p => p.InverseReportsToNavigation)
                .HasForeignKey(d => d.ReportsTo)
                .HasConstraintName("employee_reports_to_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("genre_pkey");

            entity.ToTable("genre");

            entity.Property(e => e.GenreId)
                .ValueGeneratedNever()
                .HasColumnName("genre_id");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.HasIndex(e => e.CustomerId, "invoice_customer_id_idx");

            entity.Property(e => e.InvoiceId)
                .ValueGeneratedNever()
                .HasColumnName("invoice_id");
            entity.Property(e => e.BillingAddress)
                .HasMaxLength(70)
                .HasColumnName("billing_address");
            entity.Property(e => e.BillingCity)
                .HasMaxLength(40)
                .HasColumnName("billing_city");
            entity.Property(e => e.BillingCountry)
                .HasMaxLength(40)
                .HasColumnName("billing_country");
            entity.Property(e => e.BillingState)
                .HasMaxLength(40)
                .HasColumnName("billing_state");
            entity.Property(e => e.BillingZipCode)
                .HasMaxLength(10)
                .HasColumnName("billing_postal_code");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.InvoiceDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("invoice_date");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_customer_id_fkey");
        });

        modelBuilder.Entity<InvoiceLine>(entity =>
        {
            entity.HasKey(e => e.InvoiceLineId).HasName("invoice_line_pkey");

            entity.ToTable("invoice_line");

            entity.HasIndex(e => e.InvoiceId, "invoice_line_invoice_id_idx");

            entity.HasIndex(e => e.TrackId, "invoice_line_track_id_idx");

            entity.Property(e => e.InvoiceLineId)
                .ValueGeneratedNever()
                .HasColumnName("invoice_line_id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_line_invoice_id_fkey");

            entity.HasOne(d => d.Track).WithMany(p => p.InvoiceLines)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_line_track_id_fkey");
        });

        modelBuilder.Entity<MediaType>(entity =>
        {
            entity.HasKey(e => e.MediaTypeId).HasName("media_type_pkey");

            entity.ToTable("media_type");

            entity.Property(e => e.MediaTypeId)
                .ValueGeneratedNever()
                .HasColumnName("media_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("playlist_pkey");

            entity.ToTable("playlist");

            entity.Property(e => e.PlaylistId)
                .ValueGeneratedNever()
                .HasColumnName("playlist_id");
            entity.Property(e => e.Name)
                .HasMaxLength(120)
                .HasColumnName("name");

            entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("playlist_track_track_id_fkey"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("playlist_track_playlist_id_fkey"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "TrackId").HasName("playlist_track_pkey");
                        j.ToTable("playlist_track");
                        j.HasIndex(new[] { "PlaylistId" }, "playlist_track_playlist_id_idx");
                        j.HasIndex(new[] { "TrackId" }, "playlist_track_track_id_idx");
                        j.IndexerProperty<int>("PlaylistId").HasColumnName("playlist_id");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                    });
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.HasKey(e => e.TrackId).HasName("track_pkey");

            entity.ToTable("track");

            entity.HasIndex(e => e.AlbumId, "track_album_id_idx");

            entity.HasIndex(e => e.GenreId, "track_genre_id_idx");

            entity.HasIndex(e => e.MediaTypeId, "track_media_type_id_idx");

            entity.Property(e => e.TrackId)
                .ValueGeneratedNever()
                .HasColumnName("track_id");
            entity.Property(e => e.AlbumId).HasColumnName("album_id");
            entity.Property(e => e.Bytes).HasColumnName("bytes");
            entity.Property(e => e.Composer)
                .HasMaxLength(220)
                .HasColumnName("composer");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.MediaTypeId).HasColumnName("media_type_id");
            entity.Property(e => e.Milliseconds).HasColumnName("milliseconds");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Album).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.AlbumId)
                .HasConstraintName("track_album_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("track_genre_id_fkey");

            entity.HasOne(d => d.MediaType).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.MediaTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("track_media_type_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
