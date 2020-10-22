using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class ConfigKeyMap : IEntityTypeConfiguration<ConfigKey>
    {
        public void Configure(EntityTypeBuilder<ConfigKey> builder)
        {
            builder.ToTable("ConfigKey").HasKey(m => m.Code);
        }
    }
}
