﻿// <auto-generated>
//
// ┌───────────────────┬──────────────────────────────────────┐
// │ Provider          │ Microsoft.EntityFrameworkCore.Sqlite │
// ├───────────────────┼──────────────────────────────────────┤
// │ Connection String │ Data Source=Chinook.sqlite           │
// └───────────────────┴──────────────────────────────────────┘
// 
// </auto-generated>

#nullable enable
#pragma warning disable

using System;
using System.Collections.Generic;

namespace ScaffoldRenameNavigation;

public partial class InvoiceLine
{
    public int InvoiceLineId { get; set; }

    public int InvoiceId { get; set; }

    public int TrackId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;

    public virtual Track Track { get; set; } = null!;
}
