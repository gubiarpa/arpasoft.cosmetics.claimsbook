using System;
using System.Collections.Generic;
using System.Text;

namespace yanbal.claimsbook.data.Models
{
    public class ContractedGood
    {
        public GoodType GoodType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
