//using AspNetCore;
using HelloDoc2.Models;
using HelloDoc2.Models.ViewModel;
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
            ViewBag.username = HttpContext.Session.GetString("session1");
            string sessionemail = HttpContext.Session.GetString("email");
            int userid = _context.Users.FirstOrDefault(x => x.Email == sessionemail).UserId;
            var tabledb = (from r in _context.Requests
                           join rw in _context.RequestWiseFiles
                           on r.RequestId equals rw.RequestId
                           where r.UserId == userid
                           select new
                           {
                               r.RequestId,
                               r.CreatedDate,
                               r.Status,
                               rw.FileName
                           }).ToList();
            List<PatientData> list = new List<PatientData>();
            foreach (var r in tabledb)
            {
                PatientData patientrequest = new PatientData();
                patientrequest.CreatedDate = r.CreatedDate.ToString();
                patientrequest.File = r.FileName;
                patientrequest.Id = r.RequestId;
                patientrequest.Status = r.Status.ToString();
                list.Add(patientrequest);
            }
            return View(list);
        }
        public IActionResult Patient_Login(AspNetUser model)
        {
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.PasswordHash );
            if (user != null)
            {
                HttpContext.Session.SetString("session1",user.UserName);
                HttpContext.Session.SetString("email",user.Email);
                return RedirectToAction("PatientDashboard", "Home");
            }
            else {
                ModelState.AddModelError(String.Empty,"Invalid email or Password");
                return View("Patient_Login");

            }
        }

        public IActionResult patient_logout()
        {
            HttpContext.Session.Remove("session1");
            return View("Patient_Login");
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
