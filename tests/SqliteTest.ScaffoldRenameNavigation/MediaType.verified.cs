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

public partial class MediaType
{
    public int MediaTypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Track> MediaTypeTracks { get; set; } = new List<Track>();
}