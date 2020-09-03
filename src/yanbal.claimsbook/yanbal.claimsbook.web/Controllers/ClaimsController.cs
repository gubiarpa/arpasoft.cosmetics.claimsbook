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

        public async Task<IActionResult> Index()
        {
            List<DocumentType> documentTypes = await _context.DocumentTypes.OrderBy(x => x.Description).ToListAsync();
            ViewBag.DocumentTypes = documentTypes;
            return View();
        }
    }
}
