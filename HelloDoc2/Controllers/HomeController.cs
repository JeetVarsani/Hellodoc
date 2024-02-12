using HelloDoc2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HelloDoc2.Controllers
{
    public class HomeController : Controller
    {
        private readonly HellodocContext _context;

        public HomeController( HellodocContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Forgot_Password()
        {
            return View();
        }
        public IActionResult PatientDashboard()
        {
            ViewBag.UserName = HttpContext.Session.GetString("session1");
            return View();
        }
        public IActionResult Patient_Login(AspNetUser model)
        {
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.PasswordHash );
            if (user != null)
            {
                HttpContext.Session.SetString("session1",user.UserName);
                return RedirectToAction("PatientDashboard", "Home");
            }
            else {
                ModelState.AddModelError(String.Empty,"Invalid email or Password");
                return View("Patient_Login");

            }
        }
        public IActionResult Submit_Request()
        {
            return View();
        }

        public IActionResult PatientRequestForm()
        {
            return View();
        }

        public IActionResult FamilyFriendsForm()
        {
            return View();
        }
        public IActionResult ConciergeForm()
        {
            return View();
        }
        public IActionResult BusinessForm()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
