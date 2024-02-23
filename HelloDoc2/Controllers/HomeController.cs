using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting.Internal;
using MimeKit;
using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using DAL.ViewModel;
using BLL.Interface;
using BLL.Repositery;
using DAL.Models;
namespace DAL.Controllers
{
    public class HomeController : Controller
    {
        private  IPatientRequest _patientRequest;
        private readonly HellodocContext _context;
        private readonly ILogin _login;
        public HomeController(HellodocContext context, ILogin login, IPatientRequest _PatientRequest, IPatientDashboard patientDashboard)
        {
            _context = context;
            _login = login;
            this._patientRequest = _PatientRequest;
        }

        public IActionResult Index()
        {
            var model = new List<PatientData>();
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


            int? uid = HttpContext.Session.GetInt32("userId");

            ViewBag.UserName = HttpContext.Session.GetString("username");
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
                //ViewBag.reqId = item.RequestId;
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
        public IActionResult Patient_Login(LoginVm loginVm)
        {

            if (ModelState.IsValid) {
                if (_login.ValidateLogin(loginVm))
                {
                    var user = _context.Users.FirstOrDefault(x => x.Email == loginVm.Email);
                    HttpContext.Session.SetInt32("userId", user.UserId);
                    HttpContext.Session.SetString("email", user.Email);
                    //HttpContext.Session.SetString("session1", user.UserName);
                    HttpContext.Session.SetString("username", user.FirstName + " " + user.LastName);
                    TempData["success"] = "Login Success";
                    return RedirectToAction("PatientDashboard");
                }
                else {
                    TempData["error"] = "Login Failed";
                    return View();
                }
            }
            else
            {
                return View();
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

        [HttpPost]
        public IActionResult PatientRequestForm(PatientData model)
        {
                _patientRequest.patientRequestForm(model);
                return RedirectToAction("Index","Home");       
   
        }

        public IActionResult FamilyFriendsForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult FamilyFriendsForm(FamilyData familyData)
        {
            _patientRequest.familyRequestForm(familyData);
            return RedirectToAction("Index","Home");
        }


        public IActionResult ConciergeForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConciergeForm(ConciergeData model)
        {

            _patientRequest.conciergeRequestForm(model);
            return RedirectToAction("Index", "Home");

        }

        public IActionResult BusinessForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BusinessForm(BusinessData model)
        {
            _patientRequest.businessRequestForm(model);
            return RedirectToAction("Index","Home");
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
                Address = user.Street + user.City + user.State,

            };
            TempData["success"] = "Login Successful";
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
                    existUser.State = model.State;
                    existUser.ZipCode = model.ZipCode;
                    existUser.Street = model.Street;
                    existUser.City = model.City;
                    _context.SaveChanges();

                }
                if (aspnetuser != null) {
                    aspnetuser.Email = model.Email;
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("PatientDashboardProfile", "Home");
        }

        public IActionResult ViewDocument(int reqId)
        {
            ViewBag.userName = HttpContext.Session.GetString("session1");
            HttpContext.Session.SetInt32("RequestId", reqId);
            var requestData = _context.Requests.FirstOrDefault(u => u.RequestId == reqId);
            ViewBag.Uploader = requestData.FirstName;   
            ViewBag.reqid = reqId;
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

        public IActionResult DownloadAll(int reqId)
        {
            var filesRow = _context.RequestWiseFiles.Where(x => x.RequestId == reqId).ToList();
            MemoryStream ms = new MemoryStream();
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                filesRow.ForEach(file =>
                {
                    var path = "D:\\Project\\dotnet\\HelloDoc2\\HelloDoc2\\wwwroot\\upload\\" + file.FileName;
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName); 
                    ZipArchiveEntry zipEntry = zip.CreateEntry(file.FileName);
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    using (Stream zipEntryStream = zipEntry.Open())
                    {
                        fs.CopyTo(zipEntryStream);
                    }
                });
            return File(ms.ToArray(), "application/zip", "download.zip");
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
            return View();
        }

        public IActionResult PatientRequestForSomeone()
        {
            return View();

        }


        [HttpPost]
        public IActionResult PatientRequestForSomeone(PatientRequestForMeAndSomeone model) 
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            Models.Request request = new Models.Request
            {

                UserId = user.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDate = DateTime.Now,
                Status = 4,
                PhoneNumber = model.Phone,
                Email = model.Email,
            };
            _context.Requests.Add(request);
            _context.SaveChanges();



            RequestClient requestClient = new RequestClient
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Email = model.Email,
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                Street = model.Street,
                State = model.State,
                City = model.City,
                ZipCode = model.ZipCode,
                RequestId = request.RequestId,

            };
            _context.RequestClients.Add(requestClient);
            _context.SaveChanges();

            RequestWiseFile requestWiseFile = new RequestWiseFile();

            if (model.Filepath != null)
            {

                IFormFile SingleFile = model.Filepath;
                requestWiseFile.RequestId = request.RequestId;
                requestWiseFile.CreatedDate = DateTime.Now;
                requestWiseFile.FileName = SingleFile.FileName;
                _context.Add(requestWiseFile);
                _context.SaveChanges();
                var filePath = Path.Combine("wwwroot", "upload", Path.GetFileName(SingleFile.FileName));
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    // The file is saved in a buffer before being processed
                    SingleFile.CopyTo(stream);
                }
            }
            return RedirectToAction("PatientDashboard","Home");
        }



        public IActionResult PatientRequestForMe()
        {
            int? userID = HttpContext.Session.GetInt32("userId");
            var user = _context.Users.FirstOrDefault(u => u.UserId == userID);

            int intYear = (int)user.IntYear;
            int intDate = (int)user.IntDate;
            string month = (string)user.StrMonth;
            DateTime date = new DateTime(intYear, DateTime.ParseExact(month, "MMM", CultureInfo.InvariantCulture).Month, intDate);
            PatientRequestForMeAndSomeone patientRequestForMe = new PatientRequestForMeAndSomeone()
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
            return View(patientRequestForMe);

        }

        [HttpPost]
        public IActionResult PatientRequestForMe(PatientRequestForMeAndSomeone model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            Models.Request request = new Models.Request
            {
                UserId = user.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDate = DateTime.Now,
                Status = 4,
                PhoneNumber = model.Phone,
                Email = model.Email,
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            RequestClient requestClient = new RequestClient
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Email = model.Email,
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                Street = model.Street,
                State = model.State,
                City = model.City,
                ZipCode = model.ZipCode,
                RequestId = request.RequestId,

            };
            _context.RequestClients.Add(requestClient);
            _context.SaveChanges();

            RequestStatusLog requestStatusLog = new RequestStatusLog
            {
                RequestId = request.RequestId,
                Status = request.Status,
                CreatedDate = DateTime.Now,
                Notes = model.Comments,

            };
            _context.RequestStatusLogs.Add(requestStatusLog);
            _context.SaveChanges();

            RequestWiseFile requestWiseFile = new RequestWiseFile();

            if (model.Filepath != null)
            {

                IFormFile SingleFile = model.Filepath;
                requestWiseFile.RequestId = request.RequestId;
                requestWiseFile.CreatedDate = DateTime.Now;
                requestWiseFile.FileName = SingleFile.FileName;
                _context.Add(requestWiseFile);
                _context.SaveChanges();
                var filePath = Path.Combine("wwwroot", "upload", Path.GetFileName(SingleFile.FileName));
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    // The file is saved in a buffer before being processed
                    SingleFile.CopyTo(stream);
                }
            }

            return RedirectToAction("PatientDashboard", "Home");

        }



        public IActionResult PatientRequestForSomeone1(PatientRequestForMeAndSomeone model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            Models.Request request = new Models.Request
            {
                UserId = user.UserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDate = DateTime.Now,
                Status = 4,
                PhoneNumber = model.Phone,
                Email = model.Email,
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            RequestClient requestClient = new RequestClient
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                Email = model.Email,
                IntDate = model.BirthDate.Day,
                IntYear = model.BirthDate.Year,
                StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month),
                Street = model.Street,
                State = model.State,
                City = model.City,
                ZipCode = model.ZipCode,
                RequestId = request.RequestId,

            };
            _context.RequestClients.Add(requestClient);
            _context.SaveChanges();
            RequestWiseFile requestWiseFile = new RequestWiseFile();

            if (model.Filepath != null)
            {

                IFormFile SingleFile = model.Filepath;
                requestWiseFile.RequestId = request.RequestId;
                requestWiseFile.CreatedDate = DateTime.Now;
                requestWiseFile.FileName = SingleFile.FileName;
                _context.Add(requestWiseFile);
                _context.SaveChanges();
                var filePath = Path.Combine("wwwroot", "upload", Path.GetFileName(SingleFile.FileName));
                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    // The file is saved in a buffer before being processed
                    SingleFile.CopyTo(stream);
                }
            }
            return RedirectToAction("PatientDashboard", "Home");

        }


        public IActionResult ResetPasswordPatient(String code, String email)
        {
            ViewBag.Code = code;
            ViewBag.Email = email;
            if (code == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPasswordPatient(CreateAccount req)
        {
            if (ModelState.IsValid)
            {
                var aspnetuser = _context.AspNetUsers.FirstOrDefault(a => a.Email == req.UserName);
                if (aspnetuser != null)
                {
                    if (req.PasswordHash != req.ConfirmPassword)
                    {
                        TempData["passerror"] = "Password and Confirmpassword doesn't match";
                        return View();
                    }

                    aspnetuser.PasswordHash = req.PasswordHash;
                    _context.AspNetUsers.Update(aspnetuser);
                    _context.SaveChanges();
                    TempData["pwdupdate"] = "Password is updated successfully";
                    return RedirectToAction("Patient_Login", "Home");
                }
                TempData["notvalidemail"] = "You are entered wrong email";
                return RedirectToAction("ResetPasswordPatient", "Home", new { code = req.Token, email = req.UserName });

            }
            TempData["passerror"] = "Password and Confirmpassword doesn't match";
            return RedirectToAction("ResetPasswordPatient", "Home", new { code = req.Token, email = req.UserName });

        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HelloDoc2", "testinghere1008@outlook.com"));
            message.To.Add(new MailboxAddress("HelloDoc2 Member", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.office365.com", 587, false);
                //await client.AuthenticateAsync("fakeidofjd00@gmail.com", "gzskmjedfwsnulle");
                await client.AuthenticateAsync("testinghere1008@outlook.com", "Simple@12345");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        private const int TokenExpirationHours = 24;

        public string GenerateToken()
        {
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            string token = Convert.ToBase64String(tokenBytes);

            return token;
        }

        public DateTime GetTokenExpiration()
        {
            return DateTime.UtcNow.AddHours(TokenExpirationHours);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordRequest(CreateAccount req)
        {
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == req.Email);
            if (user.UserName == null)
            {
                TempData["emailnotenter"] = "Please Enter Valid Email";
                return RedirectToAction("Forgot_Password", "Home");
            }

            var resetToken = GenerateToken();
            var resetLink = "<a href=" + Url.Action("ResetPasswordPatient", "Home", new { email = req.UserName, code = resetToken }, "https") + ">Reset Password</a>";

            var subject = "Password Reset Request";
            var body = "<b>Please find the Password Reset Link.</b><br/>" + resetLink;


            await SendEmailAsync(req.UserName, subject, body);
            TempData["emailsend"] = "Email is sent successfully to your email account";
            return RedirectToAction("Patient_Login", "Home");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Home/PatientRequestForm/checkemailexists/{email}")]

        [HttpGet]
        public IActionResult checkemailexists(string email)
        {
            var emailExists = _context.AspNetUsers.Any(u => u.Email == email);
            return Json(new { exists = emailExists });
        }

    }
}
