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
            Request request = new Request();

            RequestClient requestClient = new RequestClient();

            RequestStatusLog requestStatusLog = new RequestStatusLog();
            
            RequestWiseFile requestWiseFile = new RequestWiseFile();

            request.RequestTypeId = 2;
            request.FirstName = model.F_FirstName;
            request.LastName = model.F_LastName;
            request.Email = model.F_Email;
            request.CreatedDate = DateTime.Now;
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();



            requestClient.RequestId = request.RequestId;
            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.PhoneNumber = model.PhoneNumber;
            requestClient.Email = model.Email;

            requestClient.StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.Birthdate.Month);
            requestClient.IntYear = model.Birthdate.Year;
            requestClient.IntDate = model.Birthdate.Day;

            requestClient.Street = model.Street;
            requestClient.City = model.City;
            requestClient.State = model.State;
            requestClient.ZipCode = model.ZipCode;

            _context.RequestClients.Add(requestClient);
            await _context.SaveChangesAsync();

            //requestStatusLog.RequestStatusLogId = 1;
            requestStatusLog.RequestId = request.RequestId;
            requestStatusLog.Notes = model.Symptoms;
            requestStatusLog.Status = 2;
            _context.RequestStatusLogs.Add(requestStatusLog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }
    }
}
