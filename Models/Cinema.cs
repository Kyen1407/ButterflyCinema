using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Cinema
{
    public string CinemaId { get; set; } = null!;

    public string? CinemaName { get; set; }

    public string? CinemaAddress { get; set; }

    public string? AddressUrl { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
