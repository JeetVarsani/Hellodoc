using HelloDoc2.Models;
using HelloDoc2.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HelloDoc2.Controllers
{
    public class PatientDataController : Controller
    {
        HellodocContext _context;
        public PatientDataController(HellodocContext context)
        {
            _context = context;
        }

        async public Task<IActionResult> Index(PatientData model)
        {
            AspNetUser aspNetUser = new AspNetUser();

            User user = new User();

            Request request = new Request();

            RequestClient requestClient = new RequestClient();

            RequestStatusLog requestStatusLog = new RequestStatusLog();


            RequestWiseFile requestWiseFile = new RequestWiseFile();

            var existUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            
                if (existUser == null)
                {
                    aspNetUser.UserName = model.FirstName;
                    aspNetUser.Email = model.Email;
                    aspNetUser.Phonenumber = model.Phone;
                    aspNetUser.CreatedDate = DateTime.Now;
                    aspNetUser.PasswordHash = model.PasswordHash;
                    _context.AspNetUsers.Add(aspNetUser);
                    await _context.SaveChangesAsync();

                    user.AspNetUserId = aspNetUser.Id;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.Street = model.Street;
                    user.City = model.City;
                    user.State = model.State;
                    user.ZipCode = model.ZipCode;
                    user.CreatedBy = "1";
                    user.CreatedDate = DateTime.Now;
                    user.StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.BirthDate.Month);
                    user.IntYear = model.BirthDate.Year;
                    user.IntDate = model.BirthDate.Day;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    request.UserId = user.UserId;
                }

                request.RequestTypeId = 1;
                request.FirstName = model.FirstName;
                request.LastName = model.LastName;
                request.PhoneNumber = model.Phone;
                request.Email = model.Email;
                request.Status = 1;
                request.UserId = user.UserId;
                request.CreatedDate = DateTime.Now;
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                requestClient.RequestId = request.RequestId;
                requestClient.FirstName = model.FirstName;
                requestClient.LastName = model.LastName;
                requestClient.PhoneNumber = model.Phone;
                requestClient.Email = model.Email;
                _context.RequestClients.Add(requestClient);
                await _context.SaveChangesAsync();

                requestStatusLog.RequestId = request.RequestId;
                requestStatusLog.Status = 3;
                requestStatusLog.Notes = model.Symptoms;
                requestStatusLog.CreatedDate = DateTime.Now;
                _context.RequestStatusLogs.Add(requestStatusLog);
                await _context.SaveChangesAsync();

                if (model.Filepath != null)
                {

                    IFormFile SingleFile = model.Filepath;
                    requestWiseFile.RequestId = request.RequestId;
                    requestWiseFile.CreatedDate = DateTime.Now;
                    requestWiseFile.FileName = SingleFile.FileName;
                    _context.Add(requestWiseFile);
                    await _context.SaveChangesAsync();
                    var filePath = Path.Combine("wwwroot", "upload", Path.GetFileName(SingleFile.FileName));
                    using (FileStream stream = System.IO.File.Create(filePath))
                    {
                        // The file is saved in a buffer before being processed
                        await SingleFile.CopyToAsync(stream);
                    }
                }
                User user1 = _context.Users.Where(s => s.Email == model.Email).FirstOrDefault();

                if (user1 != null)
                {
                    List<Request> requests = _context.Requests.Where(x => x.Email == model.Email).ToList();
                    requests.ForEach(x => x.UserId = user1.UserId);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
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
