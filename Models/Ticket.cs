using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Ticket
{
    public string TicketId { get; set; } = null!;

    public string? ShowtimeId { get; set; }

    public string? SeatId { get; set; }

    public bool? IsBooked { get; set; }

    public int? TicketPrice { get; set; }

    public virtual Seat? Seat { get; set; }

    public virtual Showtime? Showtime { get; set; }
}
