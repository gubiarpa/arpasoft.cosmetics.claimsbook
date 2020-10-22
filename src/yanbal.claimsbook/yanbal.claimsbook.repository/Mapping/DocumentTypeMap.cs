using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.repository.Mapping
{
    public class DocumentTypeMap : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.ToTable("DocumentType").HasKey(m => m.ID);
        }
    }
}
