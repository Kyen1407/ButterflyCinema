using System;
using System.Collections;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Concession
{
    public string ConcessionId { get; set; } = null!;

    public string? ConcessionName { get; set; }

    public int? ConcessionPrice { get; set; }

    public BitArray? ConcessionStatus { get; set; }

    public string? ConcessionImg { get; set; }

    public string? ConcessionItem { get; set; }

    public virtual ICollection<Comboitem> Comboitems { get; set; } = new List<Comboitem>();
}
