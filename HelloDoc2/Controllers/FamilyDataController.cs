using Microsoft.AspNetCore.Mvc;
using HelloDoc2.Models;
using HelloDoc2.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc2.Controllers
{
    public class FamilyDataController : Controller
    {
        HellodocContext _context;
        public FamilyDataController(HellodocContext context)
        {
            _context = context;
        }
        async public Task<IActionResult> Index(FamilyData model)
        {

            return View();
        }
    }
}
