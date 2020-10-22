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
        public DbSet<AnswerType> AnswerTypes { get; set; }
        public DbSet<GoodType> GoodTypes { get; set; }
        public DbSet<ClaimType> ClaimTypes { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Claimer> Claimers { get; set; }
        public DbSet<ConfigKey> ConfigKeys { get; set; }

        public DBContextApp(DbContextOptions<DBContextApp> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DocumentTypeMap());
            modelBuilder.ApplyConfiguration(new GeoZoneMap());
            modelBuilder.ApplyConfiguration(new AnswerTypeMap());
            modelBuilder.ApplyConfiguration(new ClaimTypeMap());
            modelBuilder.ApplyConfiguration(new ClaimerMap());
            modelBuilder.ApplyConfiguration(new ClaimMap());
            modelBuilder.ApplyConfiguration(new GoodTypeMap());
            modelBuilder.ApplyConfiguration(new ConfigKeyMap());
        }
    }
}
