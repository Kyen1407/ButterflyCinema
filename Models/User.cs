﻿// Models/User.cs
using System;
using System.Collections.Generic;

namespace ButterflyCinema.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? RoleId { get; set; } // Giữ nguyên kiểu string

    public DateOnly? CreationDate { get; set; }

    public bool IsEmailVerified { get; set; } // Thêm trường xác thực email
    public string? EmailVerificationToken { get; set; } // Thêm trường token xác thực email

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Role? Role { get; set; }
}