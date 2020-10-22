using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using yanbal.claimsbook.data.Models;

namespace yanbal.claimsbook.data.Utils
{
    public static class ExtensionMethods
    {
        public static string ToDbString(this GoodTypeEnum goodType)
        {
            switch (goodType)
            {
                case GoodTypeEnum.Product:
                    return "Producto";
                case GoodTypeEnum.Service:
                    return "Servicio";
                default:
                    throw new Exception("Unmapped Value");
            }
        }

        public static string ToDbString(this ClaimTypeEnum claimType)
        {
            switch (claimType)
            {
                case ClaimTypeEnum.Claim:
                    return "Reclamo";
                case ClaimTypeEnum.Complaint:
                    return "Queja";
                default:
                    throw new Exception("Unmapped Value");
            }
        }

        public static Claim GetCopy(this Claim claim)
        {
            return new Claim()
            {
                ID = claim.ID,
                MainClaimerID = claim.MainClaimerID,
                GuardClaimerID = claim.GuardClaimerID,
                GoodTypeID = claim.GoodTypeID,
                ClaimedAmount = claim.ClaimedAmount,
                Description = claim.Description,
                ClaimTypeID = claim.ClaimTypeID,
                ClaimDetail = claim.ClaimDetail,
                OrderDetail = claim.OrderDetail
            };
        }
    }
}
