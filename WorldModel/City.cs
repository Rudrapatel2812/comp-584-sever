﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorldModel;

[Table("City")]
public partial class City
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    [Column("name")]
    [StringLength(10)]
    public string Name { get; set; } = null!;

    [Column("lat")]
    public int Lat { get; set; }

    [Column("lng")]
    public int Lng { get; set; }

    [Column("population")]
    public int Population { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("Cities")]
    public virtual Country Country { get; set; } = null!;
}
