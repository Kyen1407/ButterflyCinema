using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Comboitem
{
    public string ComboId { get; set; } = null!;

    public string ConcessionId { get; set; } = null!;

    public int? Quantity { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Concession Concession { get; set; } = null!;
}
