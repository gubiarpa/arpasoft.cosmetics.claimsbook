using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using yanbal.claimsbook.data.Models;
using yanbal.claimsbook.repository;

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

        #region AjaxRequests
        public async Task<IActionResult> GetDocumentTypes()
        {
            var documentTypes = await _context.DocumentTypes.OrderBy(x => x.Description).ToListAsync();
            return new JsonResult(documentTypes);
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
        #endregion
    }
}
