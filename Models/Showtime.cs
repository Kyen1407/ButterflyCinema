using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Showtime
{
    public string ShowtimeId { get; set; } = null!;

    public string? MovieId { get; set; }

    public string? RoomId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateOnly? ShowDate { get; set; }

    public string? Format { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Movie? Movie { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
