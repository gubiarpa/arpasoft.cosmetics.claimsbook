using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class ClaimerMap : IEntityTypeConfiguration<Claimer>
    {
        public void Configure(EntityTypeBuilder<Claimer> builder)
        {
            builder.ToTable("Claimer").HasKey(m => m.ID);
        }
    }
}
