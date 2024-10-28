using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ButterflyCinema.Models;

public partial class ButterflycinemaContext : DbContext
{
    public ButterflycinemaContext()
    {
    }

    public ButterflycinemaContext(DbContextOptions<ButterflycinemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<Comboitem> Comboitems { get; set; }

    public virtual DbSet<Concession> Concessions { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoicedetail> Invoicedetails { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=CinemaDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.CinemaId).HasName("cinemas_pkey");

            entity.ToTable("cinemas");

            entity.Property(e => e.CinemaId)
                .HasMaxLength(5)
                .HasColumnName("cinema_id");
            entity.Property(e => e.AddressUrl).HasColumnName("address_url");
            entity.Property(e => e.CinemaAddress)
                .HasMaxLength(100)
                .HasColumnName("cinema_address");
            entity.Property(e => e.CinemaName)
                .HasMaxLength(100)
                .HasColumnName("cinema_name");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("combo_pkey");

            entity.ToTable("combo");

            entity.Property(e => e.ComboId)
                .HasMaxLength(5)
                .HasColumnName("combo_id");
            entity.Property(e => e.ComboImg).HasColumnName("combo_img");
            entity.Property(e => e.ComboName)
                .HasMaxLength(50)
                .HasColumnName("combo_name");
            entity.Property(e => e.ComboPrice).HasColumnName("combo_price");
            entity.Property(e => e.ComboStatus)
                .HasColumnType("bit(1)")
                .HasColumnName("combo_status");
        });

        modelBuilder.Entity<Comboitem>(entity =>
        {
            entity.HasKey(e => new { e.ComboId, e.ConcessionId }).HasName("comboitems_pkey");

            entity.ToTable("comboitems");

            entity.Property(e => e.ComboId)
                .HasMaxLength(5)
                .HasColumnName("combo_id");
            entity.Property(e => e.ConcessionId)
                .HasMaxLength(5)
                .HasColumnName("concession_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Combo).WithMany(p => p.Comboitems)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comboitems_combo_id_fkey");

            entity.HasOne(d => d.Concession).WithMany(p => p.Comboitems)
                .HasForeignKey(d => d.ConcessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comboitems_concession_id_fkey");
        });

        modelBuilder.Entity<Concession>(entity =>
        {
            entity.HasKey(e => e.ConcessionId).HasName("concessions_pkey");

            entity.ToTable("concessions");

            entity.Property(e => e.ConcessionId)
                .HasMaxLength(5)
                .HasColumnName("concession_id");
            entity.Property(e => e.ConcessionImg).HasColumnName("concession_img");
            entity.Property(e => e.ConcessionItem)
                .HasMaxLength(50)
                .HasColumnName("concession_item");
            entity.Property(e => e.ConcessionName)
                .HasMaxLength(50)
                .HasColumnName("concession_name");
            entity.Property(e => e.ConcessionPrice).HasColumnName("concession_price");
            entity.Property(e => e.ConcessionStatus)
                .HasColumnType("bit(1)")
                .HasColumnName("concession_status");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.InvoiceId)
                .HasMaxLength(10)
                .HasColumnName("invoice_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.InvoiceDate).HasColumnName("invoice_date");
            entity.Property(e => e.InvoiceStatus)
                .HasColumnType("bit(1)")
                .HasColumnName("invoice_status");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.ShowtimeId)
                .HasMaxLength(10)
                .HasColumnName("showtime_id");
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount");
            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .HasColumnName("user_id");
            entity.Property(e => e.Vat).HasColumnName("vat");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("invoice_showtime_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("invoice_user_id_fkey");
        });

        modelBuilder.Entity<Invoicedetail>(entity =>
        {
            entity.HasKey(e => e.InvoicedetailId).HasName("invoicedetails_pkey");

            entity.ToTable("invoicedetails");

            entity.Property(e => e.InvoicedetailId)
                .HasMaxLength(12)
                .HasColumnName("invoicedetail_id");
            entity.Property(e => e.InvoiceId)
                .HasMaxLength(10)
                .HasColumnName("invoice_id");
            entity.Property(e => e.ItemId)
                .HasMaxLength(10)
                .HasColumnName("item_id");
            entity.Property(e => e.ItemType)
                .HasMaxLength(50)
                .HasColumnName("item_type");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SubTotal).HasColumnName("sub_total");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Invoicedetails)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("invoicedetails_invoice_id_fkey");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("movies_pkey");

            entity.ToTable("movies");

            entity.Property(e => e.MovieId)
                .HasMaxLength(5)
                .HasColumnName("movie_id");
            entity.Property(e => e.Actor)
                .HasMaxLength(150)
                .HasColumnName("actor");
            entity.Property(e => e.AgeRestriction).HasColumnName("age_restriction");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .HasColumnName("director");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.LanguageOption)
                .HasMaxLength(50)
                .HasColumnName("language_option");
            entity.Property(e => e.MovieName).HasColumnName("movie_name");
            entity.Property(e => e.Poster).HasColumnName("poster");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Trailer).HasColumnName("trailer");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewId).HasName("news_pkey");

            entity.ToTable("news");

            entity.Property(e => e.NewId)
                .HasMaxLength(5)
                .HasColumnName("new_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.PostDate).HasColumnName("post_date");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .HasMaxLength(5)
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("rooms_pkey");

            entity.ToTable("rooms");

            entity.Property(e => e.RoomId)
                .HasMaxLength(8)
                .HasColumnName("room_id");
            entity.Property(e => e.CinemaId)
                .HasMaxLength(5)
                .HasColumnName("cinema_id");
            entity.Property(e => e.RoomName).HasColumnName("room_name");
            entity.Property(e => e.RoomStatus)
                .HasColumnType("bit(1)")
                .HasColumnName("room_status");
            entity.Property(e => e.TotalSeats).HasColumnName("total_seats");

            entity.HasOne(d => d.Cinema).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CinemaId)
                .HasConstraintName("rooms_cinema_id_fkey");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("seats_pkey");

            entity.ToTable("seats");

            entity.Property(e => e.SeatId)
                .HasMaxLength(10)
                .HasColumnName("seat_id");
            entity.Property(e => e.RoomId)
                .HasMaxLength(8)
                .HasColumnName("room_id");
            entity.Property(e => e.SeatName)
                .HasMaxLength(3)
                .HasColumnName("seat_name");
            entity.Property(e => e.SeatType)
                .HasColumnType("bit(1)")
                .HasColumnName("seat_type");

            entity.HasOne(d => d.Room).WithMany(p => p.Seats)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("seats_room_id_fkey");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId).HasName("showtime_pkey");

            entity.ToTable("showtime");

            entity.Property(e => e.ShowtimeId)
                .HasMaxLength(10)
                .HasColumnName("showtime_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Format)
                .HasMaxLength(15)
                .HasColumnName("format");
            entity.Property(e => e.MovieId)
                .HasMaxLength(5)
                .HasColumnName("movie_id");
            entity.Property(e => e.RoomId)
                .HasMaxLength(8)
                .HasColumnName("room_id");
            entity.Property(e => e.ShowDate).HasColumnName("show_date");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("showtime_movie_id_fkey");

            entity.HasOne(d => d.Room).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("showtime_room_id_fkey");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("ticket_pkey");

            entity.ToTable("ticket");

            entity.Property(e => e.TicketId)
                .HasMaxLength(15)
                .HasColumnName("ticket_id");
            entity.Property(e => e.IsBooked).HasColumnName("is_booked");
            entity.Property(e => e.SeatId)
                .HasMaxLength(10)
                .HasColumnName("seat_id");
            entity.Property(e => e.ShowtimeId)
                .HasMaxLength(10)
                .HasColumnName("showtime_id");
            entity.Property(e => e.TicketPrice).HasColumnName("ticket_price");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("ticket_seat_id_fkey");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("ticket_showtime_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasMaxLength(12)
                .HasColumnName("user_id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(3)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId)
                .HasMaxLength(5)
                .HasColumnName("role_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(70)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
