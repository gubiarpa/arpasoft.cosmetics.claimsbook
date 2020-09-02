using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace yanbal.claimsbook.repository
{
    public class DBContextApp : DbContext
    {
        public DBContextApp(DbContextOptions<DBContextApp> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
