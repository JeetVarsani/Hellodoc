using HelloDoc2.DataModels;
using HelloDoc2.Models;
using HelloDoc2.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using System.Globalization;

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
            var model = new List<HelloDoc2.Models.ViewModel.PatientData>();
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
        //public IActionResult PatientDashboard()
        //{
        //    ViewBag.ActivePage = "PatientDashboard";
        //    ViewBag.username = HttpContext.Session.GetString("session1");
        //    string sessionemail = HttpContext.Session.GetString("email");

        //    int userid = _context.Users.FirstOrDefault(x => x.Email == sessionemail).UserId;
        //    var tabledb = (from r in _context.Requests
        //                   join rw in _context.RequestWiseFiles
        //                   on r.RequestId equals rw.RequestId
        //                   where r.UserId == userid
        //                   select new
        //                   {
        //                       r.RequestId,
        //                       r.CreatedDate,
        //                       r.Status,
        //                       rw.FileName
        //                   }).ToList();
        //    List<PatientData> list = new List<PatientData>();
        //    foreach (var r in tabledb)
        //    {
        //        var count = _context.RequestWiseFiles.Where(o => o.RequestId == r.RequestId).Count();
        //        PatientData patientrequest = new PatientData();
        //        patientrequest.CreatedDate = r.CreatedDate.ToString();
        //        patientrequest.Documents = r.FileName;
        //        patientrequest.Id = r.RequestId;
        //        patientrequest.Status = r.Status.ToString();
        //        patientrequest.Count = count; 
        //        list.Add(patientrequest);
        //    }


        //    return View(list);
        //}

        public IActionResult PatientDashboard()
        {

            //if (_contextAccessor.HttpContext.Session.GetString("UserId") == null)
            //{
            //    return RedirectToAction("LoginPatient", "Home");
            //}
            int? uid = HttpContext.Session.GetInt32("userId");
            //string Username = HttpContext.Session.GetString("Username");

            ViewBag.UserName = HttpContext.Session.GetString("session1");
            ViewBag.ActivePage = "PatientDashboard";
            var applicationDbContext = (from r in _context.Requests
                                        where r.UserId == uid
                                        select new
                                        {
                                            r.RequestId,
                                            r.CreatedDate,
                                            r.Status,
                                        }

                                        ).ToList();

            List<PatientData> list = new List<PatientData>();
            foreach (var item in applicationDbContext)
            {
                var count = _context.RequestWiseFiles.Where(o => o.RequestId == item.RequestId).Count();

                var reqwisefile = _context.RequestWiseFiles.FirstOrDefault(x => x.RequestId == item.RequestId);
                string createddate = item.CreatedDate.ToString();
                PatientData user = new PatientData();
                user.CreatedDate = createddate;
                if (item.Status == 1)
                {
                    user.Status = "Unassigned";
                }
                else if (item.Status == 2)
                {
                    user.Status = "Accepted";
                }
                else if (item.Status == 3)
                {
                    user.Status = "Cancelled";
                }
                else if (item.Status == 4)
                {
                    user.Status = "Reserving";
                }
                user.Count = count;

                user.Id = item.RequestId;
                list.Add(user);
            }


            //ViewBag.Username = Username;
            return View(list);
        }
        public IActionResult Patient_Login(AspNetUser model)
        {
            
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == model.PasswordHash );
            if (user != null)
            {
                var userid = _context.Users.FirstOrDefault(u => u.Email == user.Email);
                HttpContext.Session.SetInt32("userId",userid.UserId);
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

        public IActionResult PatientDashboardProfile()
        {
            ViewBag.ActivePage = "PatientDashboardProfile";
            int? userID = HttpContext.Session.GetInt32("userId");
            var user = _context.Users.FirstOrDefault(u => u.UserId == userID);

            int intYear = (int)user.IntYear;
            int intDate = (int)user.IntDate;
            string month = (string)user.StrMonth;
            DateTime date = new DateTime(intYear, DateTime.ParseExact(month, "MMM", CultureInfo.InvariantCulture).Month, intDate);
            PatientProfile patientProfile = new PatientProfile()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Street = user.Street,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Phone = user.Mobile,
                BirthDate = date,

            };

            return View(patientProfile);
        }


        

        public IActionResult EditProfile(PatientProfile model) {

            var userid = HttpContext.Session.GetInt32("userId");
            var existUser = _context.Users.FirstOrDefault(u => u.UserId == userid);
            //var aspnetuser = _context.AspNetUsers.FirstOrDefault(u=> u.Email == existUser.Email);
            var aspnetuser = _context.AspNetUsers.FirstOrDefault(u => u.Id == existUser.AspNetUserId);

            if (userid != null) {
                if (existUser != null)
                {
                    existUser.FirstName = model.FirstName;
                    existUser.LastName = model.LastName;
                    existUser.StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month);
                    existUser.IntYear = model.BirthDate.Year;
                    existUser.IntDate = model.BirthDate.Day;
                    existUser.Email = model.Email;
                    existUser.Mobile = model.Phone;
                    _context.SaveChanges();

                }
                if (aspnetuser != null) { 
                    aspnetuser.Email = model.Email;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("PatientDashboardProfile","Home");        
        }

        public IActionResult ViewDocument(int reqId) 
        { 
            ViewBag.userName = HttpContext.Session.GetString("session1");
            HttpContext.Session.SetInt32("RequestId", reqId);
            var requestData = _context.Requests.FirstOrDefault(u => u.RequestId == reqId);
            ViewBag.reqid = reqId;
            ViewBag.Uploader = requestData.FirstName;
            var documents = _context.RequestWiseFiles.Where(u => u.RequestId == reqId).ToList();
            ViewBag.document = documents;
            return View();  
        }

        public IActionResult Download(int documentid)
        {
            var filename = _context.RequestWiseFiles.FirstOrDefault(u => u.RequestWiseFileId == documentid);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", filename.FileName);
            return File(System.IO.File.ReadAllBytes(filePath), "multipart/form-data", System.IO.Path.GetFileName(filePath));

        }

        

        [HttpPost]

        public IActionResult Upload([FromForm] IFormFile Filepath)
        {
            int? reqid = HttpContext.Session.GetInt32("RequestId");
            RequestWiseFile requestWiseFile = new RequestWiseFile();
            IFormFile SingleFile = Filepath;
            requestWiseFile.RequestId = (int)reqid;
            requestWiseFile.FileName = SingleFile.FileName;
            _context.Add(requestWiseFile);
            _context.SaveChanges();
            var filePath = Path.Combine("wwwroot", "upload", Path.GetFileName(SingleFile.FileName));
            using (FileStream stream = System.IO.File.Create(filePath))
            {
                // The file is saved in a buffer before being processed
                SingleFile.CopyTo(stream);
            }
            TempData["Uploadscs"] = "File Uploaded Successfully.Please Refresh Page";
            return RedirectToAction("ViewDocument", "Home");
        }



        public IActionResult PatientRequestForMe() 
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
