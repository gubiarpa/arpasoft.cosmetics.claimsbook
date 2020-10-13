using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using yanbal.claimsbook.data.Models;
using yanbal.claimsbook.data.Utils;
using yanbal.claimsbook.repository;
using yanbal.claimsbook.web.Models;
using Rotativa.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Cors;
using yanbal.claimsbook.web.Helpers;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Http.Extensions;
using yanbal.claimsbook.web.Utils.MD5;

namespace yanbal.claimsbook.web.Controllers
{
    public class ClaimsController : Controller
    {
        protected readonly DBContextApp _context;

        public ClaimsController(DBContextApp context)
        {
            _context = context;
        }

        public IActionResult Index()
        {           
            return View();
        }

        #region Reports
        public async Task<IActionResult> GenerateClaimPdf(Guid ID)
        {
            try
            {
                Claim claim = null; Claimer mainClaimer = null, guardClaimer = null;
                claim = await _context.Claims.SingleOrDefaultAsync(x => x.ID.Equals(ID));

                #region MainClaimer
                mainClaimer = (await _context.Claimers.SingleOrDefaultAsync(x => x.ID.Equals(claim.MainClaimerID))).Decrypt();
                var mainClaimerPdf = new ClaimerPdfViewModel();
                mainClaimerPdf.DocumentType = (await _context.DocumentTypes.SingleOrDefaultAsync(x => x.ID.Equals(mainClaimer.DocumentTypeID))).Description;
                mainClaimerPdf.DocumentNumber = mainClaimer.DocumentNumber;
                mainClaimerPdf.FullName = string.Format("{0} {1} {2}", mainClaimer.Names, mainClaimer.PaternalSurname, mainClaimer.MaternalSurname);
                mainClaimerPdf.PhoneNumber = mainClaimer.PhoneNumber;
                mainClaimerPdf.ResponseTo = (await _context.AnswerTypes.SingleOrDefaultAsync(x => x.ID.Equals(mainClaimer.AnswerTypeID))).Description;
                mainClaimerPdf.EMail = mainClaimer.EMail;
                var geoZoneMain = await _context.GeoZones.SingleOrDefaultAsync(x => x.ID.Equals(mainClaimer.GeoZoneID));
                mainClaimerPdf.Address = string.Format("{0} {1}, {2}, {3}", mainClaimer.Address, geoZoneMain.District, geoZoneMain.Province, geoZoneMain.Department);
                #endregion

                #region GuardClaimer
                var guardClaimerPdf = new ClaimerPdfViewModel();
                if (claim.GuardClaimerID != null)
                {
                    if (claim.GuardClaimerID != null ) guardClaimer = await _context.Claimers.SingleOrDefaultAsync(x => x.ID.Equals(claim.GuardClaimerID));
                    guardClaimer = (await _context.Claimers.SingleOrDefaultAsync(x => x.ID.Equals(claim.GuardClaimerID))).Decrypt();
                    guardClaimerPdf.DocumentType = (await _context.DocumentTypes.SingleOrDefaultAsync(x => x.ID.Equals(guardClaimer.DocumentTypeID))).Description;
                    guardClaimerPdf.DocumentNumber = guardClaimer.DocumentNumber;
                    guardClaimerPdf.FullName = string.Format("{0} {1} {2}", guardClaimer.Names, guardClaimer.PaternalSurname, guardClaimer.MaternalSurname);
                    guardClaimerPdf.PhoneNumber = guardClaimer.PhoneNumber;
                    guardClaimerPdf.ResponseTo = (await _context.AnswerTypes.SingleOrDefaultAsync(x => x.ID.Equals(guardClaimer.AnswerTypeID))).Description;
                    guardClaimerPdf.EMail = guardClaimer.EMail;
                    var geoZoneGuard = await _context.GeoZones.SingleOrDefaultAsync(x => x.ID.Equals(guardClaimer.GeoZoneID));
                    guardClaimerPdf.Address = string.Format("{0} {1}, {2}, {3}", guardClaimer.Address, geoZoneGuard.District, geoZoneGuard.Province, geoZoneGuard.Department);
                }
                #endregion

                #region ContractedGood
                var contractedGood = await _context.GoodTypes.SingleOrDefaultAsync(x => x.ID.Equals(claim.GoodTypeID));
                var contractedGoodPdf = new ContractedGoodPdfViewModel();
                contractedGoodPdf.GoodType = contractedGood.Description;
                contractedGoodPdf.ClaimedAmount = claim.ClaimedAmount;
                contractedGoodPdf.GoodDescription = claim.Description;
                #endregion

                #region ClaimDetail
                var claimDetail = await _context.ClaimTypes.SingleOrDefaultAsync(x => x.ID.Equals(claim.ClaimTypeID));
                var claimDetailPdf = new ClaimDetailPdfViewModel();
                claimDetailPdf.ClaimType = claimDetail.Description;
                claimDetailPdf.ClaimDetail = claim.ClaimDetail;
                claimDetailPdf.OrderDetail = claim.OrderDetail;
                #endregion

                #region BusinessInfo
                var configKeys = await _context.ConfigKeys.ToListAsync();
                var companyInfo = new CompanyInfo()
                {
                    Address = configKeys.SingleOrDefault(x => x.Code.Equals("DomicilioFiscal")).Value,
                    DocumentNumber = configKeys.SingleOrDefault(x => x.Code.Equals("Ruc")).Value,
                    Name = configKeys.SingleOrDefault(x => x.Code.Equals("RazonSocial")).Value
                };
                #endregion

                #region Claim
                var claimResponse = new ClaimPdfViewModel()
                {
                    CompanyInfo = companyInfo,
                    IsAdult = claim.GuardClaimerID == null ? "Sí" : "No",
                    ClaimNumber = string.Format("{0}-{1}", claim.YearNumber, claim.SerialNumber.ToString("0000")),
                    ClaimDate = claim.DateClaim,
                    MainClaimer = mainClaimerPdf,
                    GuardClaimer = guardClaimerPdf,
                    ContractedGood = contractedGoodPdf,
                    ClaimDetail = claimDetailPdf
                };
                #endregion

                return new ViewAsPdf("GenerateClaimPdf", claimResponse)
                {
                    //Password = mainClaimerPdf.DocumentNumber
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> Conditions()
        {
            var file = await System.IO.File.ReadAllBytesAsync("wwwroot/docs/Condiciones.pdf");
            return File(file, "application/pdf");
        }
        #endregion

        #region AjaxRequests
        public async Task<IActionResult> GetDocumentTypes()
        {
            var documentTypes = await _context.DocumentTypes.OrderBy(x => x.OrderNumber).ToListAsync();
            return new JsonResult(documentTypes);
        }

        public async Task<IActionResult> GetAnswerTypes()
        {
            var answerTypes = await _context.AnswerTypes.OrderBy(x => x.Description).ToListAsync();
            return new JsonResult(answerTypes);
        }

        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.GeoZones
                .Where(x => x.Code.EndsWith("0000"))
                .OrderBy(x => x.Code)
                .ToListAsync();
            return new JsonResult(departments);
        }
        
        public async Task<IActionResult> GetProvinces(string geoCode)
        {
            var departmentCode = geoCode.Substring(0, 2);
            var provinces = await _context.GeoZones
                .Where(x => x.Code.StartsWith(departmentCode) && x.Code.EndsWith("00") && !x.Code.EndsWith("0000"))
                .OrderBy(x => x.Code)
                .ToListAsync();
            return new JsonResult(provinces);
        }

        public async Task<IActionResult> GetDistricts(string geoCode)
        {
            var provinceCode = geoCode.Substring(0, 4);
            var districts = await _context.GeoZones
                .Where(x => x.Code.StartsWith(provinceCode) && !x.Code.EndsWith("00"))
                .OrderBy(x => x.Code)
                .ToListAsync();
            return new JsonResult(districts);
        }

        public async Task<IActionResult> GetCompanyInfo()
        {
            var configKeys = await _context.ConfigKeys.ToListAsync();
            var companyInfo = new CompanyInfo()
            {
                Address = configKeys.SingleOrDefault(x => x.Code.Equals("DomicilioFiscal")).Value,
                DocumentNumber = configKeys.SingleOrDefault(x => x.Code.Equals("Ruc")).Value,
                Name = configKeys.SingleOrDefault(x => x.Code.Equals("RazonSocial")).Value
            };
            return new JsonResult(companyInfo);
        }

        [HttpPost]
        public async Task<IActionResult> SaveClaim(ClaimViewModel claimRequest)
        {
            try
            {
                // GeoZone
                var geoZone = await _context.GeoZones.SingleOrDefaultAsync(x => x.Code == claimRequest.MainClaimer.GeoZone);
                var guardGeoZone = await _context.GeoZones.SingleOrDefaultAsync(x => x.Code == claimRequest.GuardClaimer.GeoZone);

                // Main Claimer
                var mainClaimer = new Claimer()
                {
                    DocumentTypeID = claimRequest.MainClaimer.DocumentType,
                    DocumentNumber = claimRequest.MainClaimer.DocumentNumber,
                    Names = claimRequest.MainClaimer.Names,
                    PaternalSurname = claimRequest.MainClaimer.PaternalSurname,
                    MaternalSurname = claimRequest.MainClaimer.MaternalSurname,
                    AnswerTypeID = claimRequest.MainClaimer.AnswerType,
                    PhoneNumber = claimRequest.MainClaimer.PhoneNumber,
                    EMail = claimRequest.MainClaimer.EMail,
                    Address = claimRequest.MainClaimer.Address,
                    GeoZoneID = geoZone.ID
                };
                var mainclaimerEncrypt = mainClaimer.Encript();
                _context.Claimers.Add(mainclaimerEncrypt);
                mainClaimer.ID = mainclaimerEncrypt.ID;

                var guardClaimer = new Claimer();

                if (!claimRequest.IsAdult)
                {
                    guardClaimer = new Claimer()
                    {
                        DocumentTypeID = claimRequest.GuardClaimer.DocumentType,
                        DocumentNumber = claimRequest.GuardClaimer.DocumentNumber,
                        Names = claimRequest.GuardClaimer.Names,
                        PaternalSurname = claimRequest.GuardClaimer.PaternalSurname,
                        MaternalSurname = claimRequest.GuardClaimer.MaternalSurname,
                        AnswerTypeID = claimRequest.GuardClaimer.AnswerType,
                        PhoneNumber = claimRequest.GuardClaimer.PhoneNumber,
                        EMail = claimRequest.GuardClaimer.EMail,
                        Address = claimRequest.GuardClaimer.Address,
                        GeoZoneID = guardGeoZone.ID
                    };
                    var guardClaimerEncrypt = guardClaimer.Encript();
                    _context.Claimers.Add(guardClaimerEncrypt);
                    guardClaimer.ID = guardClaimerEncrypt.ID;
                }
                else
                {
                    guardClaimer = null;
                }

                await _context.SaveChangesAsync();

                // Good Type
                var goodTypeEnum = claimRequest.ContractedGood.IsAProduct ?
                    GoodTypeEnum.Product : GoodTypeEnum.Service;
                var goodType = (await _context.GoodTypes.SingleOrDefaultAsync(
                    x => x.Description == goodTypeEnum.ToDbString()));

                // Claim Type
                var claimTypeEnum = claimRequest.ClaimDetail.IsAClaim ?
                    ClaimTypeEnum.Claim : ClaimTypeEnum.Complaint;
                var claimType = (await _context.ClaimTypes.SingleOrDefaultAsync(
                    x => x.Description == claimTypeEnum.ToDbString()));

                // Serial Number
                var yearClaims = _context.Claims.Where(x => x.YearNumber.Equals(DateTime.Now.Year));
                var serialNumber = 0;
                if ((yearClaims == null) || (await yearClaims.CountAsync() == 0))
                    serialNumber = 1;
                else
                    serialNumber = (await yearClaims.MaxAsync(x => x.SerialNumber)) + 1;

                // Year Number
                var dateNow = DateTime.Now;

                // Claim
                var claim = new Claim()
                {
                    MainClaimerID = mainClaimer.ID,
                    GuardClaimerID = guardClaimer == null ? null : (Guid?)guardClaimer.ID,
                    GoodTypeID = goodType.ID,
                    ClaimedAmount = claimRequest.ContractedGood.ClaimedAmount,
                    Description = claimRequest.ContractedGood.GoodDescription,
                    ClaimTypeID = claimType.ID,
                    ClaimDetail = claimRequest.ClaimDetail.ClaimDetail,
                    OrderDetail = claimRequest.ClaimDetail.OrderDetail,
                    SerialNumber = serialNumber,
                    YearNumber = dateNow.Year,
                    DateClaim = DateTime.Now
                };

                _context.Claims.Add(claim);

                await _context.SaveChangesAsync();

                /// Send Mail

                var configKeys = await _context.ConfigKeys.ToListAsync();
                var logPath = string.Format(
                    configKeys.SingleOrDefault(x => x.Code.Equals("LogPath")).Value,
                    DateTime.Now.ToString("yyyy-MM-dd"));
                var storagePath = configKeys.SingleOrDefault(x => x.Code.Equals("StoragePath")).Value;

                string storageFile = "", fullHost = "", urlPdf = "";

                try
                {
                    var host = configKeys.SingleOrDefault(x => x.Code.Equals("MailHost")).Value;
                    var port = configKeys.SingleOrDefault(x => x.Code.Equals("MailPort")).Value;
                    var name = configKeys.SingleOrDefault(x => x.Code.Equals("MailName")).Value;
                    var from = configKeys.SingleOrDefault(x => x.Code.Equals("MailFrom")).Value;
                    var to = mainClaimer.EMail;
                    var cc = guardClaimer != null ? guardClaimer.EMail : null;
                    var bcc = configKeys.SingleOrDefault(x => x.Code.Equals("MailBcc")).Value;
                    var subject = string.Format(configKeys.SingleOrDefault(x => x.Code.Equals("MailSubject")).Value, claim.YearNumber, claim.SerialNumber.ToString("0000"));
                    var bodyTemplate = System.IO.File.ReadAllText("./Utils/Mails/ClaimTemplate.html");
                    var body = bodyTemplate
                        .Replace("{Names}", string.Format("{0} {1} {2}", mainClaimer.Names, mainClaimer.PaternalSurname, mainClaimer.MaternalSurname))
                        .Replace("{ClaimType}", claimTypeEnum.ToDbString())
                        .Replace("{YearNumber}", claim.YearNumber.ToString())
                        .Replace("{SerialNumber}", claim.SerialNumber.ToString("0000"));
                    var attachmentPath = "./Utils/Mails/logo-yanbal.jpg";

                    Logger.Write(logPath, "Se leyó todo correctamente");

                    var mailSender = new MailSender()
                    {
                        Host = host,
                        Port = port,
                        Name = name,
                        From = from,
                        To = to,
                        Cc = cc,
                        Bcc = bcc,
                        Subject = subject,
                        Body = body,
                        AttachmentPath = attachmentPath,
                        AttachmentContentId = "pic1"
                    };

                    Logger.Write(logPath, mailSender.Stringify());

                    try
                    {
                        storageFile = Path.Combine(/*storagePath*/"Storage", "Hoja de Reclamación " + claim.YearNumber.ToString() + "-" + claim.SerialNumber.ToString("0000") + ".pdf");
                        fullHost = HttpContext.Request.GetDisplayUrl().Substring(0, HttpContext.Request.GetDisplayUrl().IndexOf(HttpContext.Request.Path));
                        urlPdf = Path.Combine(fullHost, "Claims/GenerateClaimPdf", claim.ID.ToString());
                    }
                    catch (Exception ex)
                    {
                        return Ok(new { claim.ID, claim.SerialNumber, claim.YearNumber, error = ex.Message,
                        datos = new { storageFile, fullHost, urlPdf }, version = "2.0.0" });
                    }
                    
                    Logger.Write(logPath, "fullHost: " + fullHost);
                    Logger.Write(logPath, "urlPdf: " + urlPdf);
                    Logger.Write(logPath, "storageFile: " + storageFile);

                    mailSender.Send(urlPdf, storageFile, logPath);

                    claim.Completed = true;
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        claim.ID,
                        claim.SerialNumber,
                        claim.YearNumber,
                        datos = new { storageFile, fullHost, urlPdf }
                    });
                }
                catch (Exception ex)
                {
                    Logger.Write(logPath, ex.Message);
                    return Ok(new { claim.ID, claim.SerialNumber, claim.YearNumber, error = ex.Message, version = "2.0.0",
                        datos = new { storageFile, fullHost, urlPdf } });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
