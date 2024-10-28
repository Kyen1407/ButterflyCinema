using System;
using System.Collections;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Combo
{
    public string ComboId { get; set; } = null!;

    public string? ComboName { get; set; }

    public int? ComboPrice { get; set; }

    public BitArray? ComboStatus { get; set; }

    public string? ComboImg { get; set; }

    public virtual ICollection<Comboitem> Comboitems { get; set; } = new List<Comboitem>();
}
