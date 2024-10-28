using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Invoicedetail
{
    public string InvoicedetailId { get; set; } = null!;

    public string? InvoiceId { get; set; }

    public string? ItemType { get; set; }

    public string? ItemId { get; set; }

    public int? Quantity { get; set; }

    public int? UnitPrice { get; set; }

    public int? SubTotal { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
