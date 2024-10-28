using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class News
{
    public string NewId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateOnly? PostDate { get; set; }
}
