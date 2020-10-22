using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class GeoZoneMap : IEntityTypeConfiguration<GeoZone>
    {
        public void Configure(EntityTypeBuilder<GeoZone> builder)
        {
            builder.ToTable("GeoZone").HasKey(m => m.ID);
        }
    }
}
