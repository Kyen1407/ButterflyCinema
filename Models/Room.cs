using System;
using System.Collections;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Room
{
    public string RoomId { get; set; } = null!;

    public string? CinemaId { get; set; }

    public int? RoomName { get; set; }

    public int? TotalSeats { get; set; }

    public BitArray? RoomStatus { get; set; }

    public virtual Cinema? Cinema { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
