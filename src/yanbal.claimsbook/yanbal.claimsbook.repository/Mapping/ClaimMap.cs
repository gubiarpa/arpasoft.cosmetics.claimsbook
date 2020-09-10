using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class ClaimMap : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToTable("Claim").HasKey(m => m.ID);
        }
    }
}
