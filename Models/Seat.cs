using System;
using System.Collections;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Seat
{
    public string SeatId { get; set; } = null!;

    public string? RoomId { get; set; }

    public string? SeatName { get; set; }

    public BitArray? SeatType { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
