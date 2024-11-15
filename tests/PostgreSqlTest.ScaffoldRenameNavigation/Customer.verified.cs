﻿// <auto-generated>
//
// ┌───────────────────┬────────────────────────────────────────────────────┐
// │ Provider          │ Npgsql.EntityFrameworkCore.PostgreSQL              │
// ├───────────────────┼────────────────────────────────────────────────────┤
// │ Connection String │ Host=127.0.0.1;Database=postgres;Username=postgres │
// └───────────────────┴────────────────────────────────────────────────────┘
// 
// </auto-generated>

#nullable enable
#pragma warning disable

using System;
using System.Collections.Generic;

namespace ScaffoldRenameNavigation;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Company { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string Email { get; set; } = null!;

    public int? SupportRepId { get; set; }

    public virtual ICollection<Invoice> CustomerInvoices { get; set; } = new List<Invoice>();

    public virtual Employee? SupportRepresentative { get; set; }
}
