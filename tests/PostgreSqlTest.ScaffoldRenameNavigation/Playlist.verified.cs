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

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}
