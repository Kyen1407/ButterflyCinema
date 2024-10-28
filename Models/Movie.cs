using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Movie
{
    public string MovieId { get; set; } = null!;

    public string? MovieName { get; set; }

    public string? Genre { get; set; }

    public string? Director { get; set; }

    public string? Actor { get; set; }

    public string? Description { get; set; }

    public int? AgeRestriction { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? LanguageOption { get; set; }

    public int? Duration { get; set; }

    public string? Country { get; set; }

    public double? Rating { get; set; }

    public string? Poster { get; set; }

    public string? Trailer { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
