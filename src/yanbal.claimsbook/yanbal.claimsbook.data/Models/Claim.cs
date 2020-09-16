using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using yanbal.claimsbook.data.Models.Behavior;

namespace yanbal.claimsbook.data.Models
{
    public class Claim : IGuid, IDescription
    {
        public Guid ID { get; set; }
        public Guid MainClaimerID { get; set; }
        public Guid? GuardClaimerID { get; set; }
        public Guid GoodTypeID { get; set; }
        public System.Decimal ClaimedAmount { get; set; }
        public string Description { get; set; }
        public Guid ClaimTypeID { get; set; }
        public string ClaimDetail { get; set; }
        public string OrderDetail { get; set; }
    }
}
