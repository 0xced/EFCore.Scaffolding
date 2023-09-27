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

namespace ScaffoldOneTableOrderedColumns;

public partial class Album
{
    public int AlbumId { get; set; }

    public string Title { get; set; } = null!;

    public int ArtistId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}