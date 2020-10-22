using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Models
{
    public class ClaimViewModel
    {
        public bool IsAdult { get; set; }
        public ClaimSheetDetail ClaimSheetDetail { get; set; }
        public ClaimerViewModel MainClaimer { get; set; }
        public ClaimerViewModel GuardClaimer { get; set; }
        public ContractedGoodViewModel ContractedGood { get; set; }
        public ClaimDetailViewModel ClaimDetail { get; set; }
        public string Token { get; set; }
    }

    public class ClaimSheetDetail
    {
        public int YearNumber { get; set; }
        public int SerialNumber { get; set; }
    }

    public class ClaimerViewModel
    {
        public Guid DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string Names { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string PhoneNumber { get; set; }
        public Guid AnswerType { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public string GeoZone { get; set; }
    }

    public class ContractedGoodViewModel
    {
        public bool IsAProduct { get; set; }
        public string ClaimedAmount { get; set; }
        public string GoodDescription { get; set; }
    }

    public class ClaimDetailViewModel
    {
        public bool IsAClaim { get; set; }
        public string ClaimDetail { get; set; }
        public string OrderDetail { get; set; }
    }
}
