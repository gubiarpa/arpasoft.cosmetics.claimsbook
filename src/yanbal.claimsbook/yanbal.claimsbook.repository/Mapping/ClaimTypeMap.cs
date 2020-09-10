using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class ClaimTypeMap : IEntityTypeConfiguration<ClaimType>
    {
        public void Configure(EntityTypeBuilder<ClaimType> builder)
        {
            builder.ToTable("ClaimType").HasKey(m => m.ID);
        }
    }
}
