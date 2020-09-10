using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class AnswerTypeMap : IEntityTypeConfiguration<AnswerType>
    {
        public void Configure(EntityTypeBuilder<AnswerType> builder)
        {
            builder.ToTable("AnswerType").HasKey(m => m.ID);
        }
    }
}
