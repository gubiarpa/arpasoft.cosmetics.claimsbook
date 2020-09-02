using System;
using System.Collections.Generic;
using System.Text;
using yanbal.claimsbook.data.Models.Behavior;

namespace yanbal.claimsbook.data.Models
{
    public class ClaimDetail : IGuid, IDescription
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
    }
}
