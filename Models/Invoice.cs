using System;
using System.Collections;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;

    public string? ShowtimeId { get; set; }

    public string? UserId { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    public string? PaymentMethod { get; set; }

    public int? Discount { get; set; }

    public int? Vat { get; set; }

    public BitArray? InvoiceStatus { get; set; }

    public int? TotalAmount { get; set; }

    public virtual ICollection<Invoicedetail> Invoicedetails { get; set; } = new List<Invoicedetail>();

    public virtual Showtime? Showtime { get; set; }

    public virtual User? User { get; set; }
}
