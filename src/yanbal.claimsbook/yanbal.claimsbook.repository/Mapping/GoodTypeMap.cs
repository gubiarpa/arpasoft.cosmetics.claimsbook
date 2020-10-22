using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class GoodTypeMap : IEntityTypeConfiguration<GoodType>
    {
        public void Configure(EntityTypeBuilder<GoodType> builder)
        {
            builder.ToTable("GoodType").HasKey(m => m.ID);
        }
    }
}
