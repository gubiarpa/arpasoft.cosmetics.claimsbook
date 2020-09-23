using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Models
{
    public class ClaimPdfViewModel
    {
        public string IsAdult { get; set; }
        public string ClaimNumber { get; set; }
        public ClaimerPdfViewModel MainClaimer { get; set; }
        public ClaimerPdfViewModel GuardClaimer { get; set; }
        public ContractedGoodPdfViewModel ContractedGood { get; set; }
        public ClaimDetailPdfViewModel ClaimDetail { get; set; }
    }

    public class ClaimerPdfViewModel
    {
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ResponseTo { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
    }

    public class ContractedGoodPdfViewModel
    {
        public string GoodType { get; set; }
        public System.Decimal ClaimedAmount { get; set; }
        public string GoodDescription { get; set; }
    }

    public class ClaimDetailPdfViewModel
    {
        public string ClaimType { get; set; }
        public string ClaimDetail { get; set; }
        public string OrderDetail { get; set; }
    }
}
