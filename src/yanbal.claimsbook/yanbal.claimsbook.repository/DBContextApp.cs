using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models;
using yanbal.claimsbook.repository.Mapping;

namespace yanbal.claimsbook.repository
{
    public class DBContextApp : DbContext
    {
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<GeoZone> GeoZones { get; set; }

        public DBContextApp(DbContextOptions<DBContextApp> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DocumentTypeMap());
            modelBuilder.ApplyConfiguration(new GeoZoneMap());
        }
    }
}
