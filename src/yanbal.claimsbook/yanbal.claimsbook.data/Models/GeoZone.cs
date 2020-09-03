using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using yanbal.claimsbook.data.Models.Behavior;

namespace yanbal.claimsbook.data.Models
{
    public class GeoZone : IGuid
    {
        [Column("ID")]
        public Guid ID { get; set; }
        [Column("Code")]
        public string Code { get; set; }
        [Column("Department")]
        public string Department { get; set; }
        [Column("Province")]
        public string Province { get; set; }
        [Column("District")]
        public string District { get; set; }
    }
}
