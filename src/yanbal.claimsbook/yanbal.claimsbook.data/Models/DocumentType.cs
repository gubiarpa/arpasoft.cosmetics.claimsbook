using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using yanbal.claimsbook.data.Models.Behavior;

namespace yanbal.claimsbook.data.Models
{
    public class DocumentType : IGuid, IDescription
    {
        [Column("ID")]
        public Guid ID { get; set; }
        [Column("Description")]
        public string Description { get; set; }
    }
}
