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

namespace ScaffoldOneTableOrderedColumns;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
