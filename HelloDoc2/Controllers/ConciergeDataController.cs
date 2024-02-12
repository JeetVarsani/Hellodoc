using Microsoft.AspNetCore.Mvc;
using HelloDoc2.Models;
using HelloDoc2.Models.ViewModel;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace HelloDoc2.Controllers
{
    public class ConciergeDataController : Controller
    {
        HellodocContext _context;

        public ConciergeDataController(HellodocContext context)
        {
            _context = context;
        }
        async public Task<IActionResult> Index(ConciergeData model)
        {
            Request request = new Request();

            RequestClient requestClient = new RequestClient();

            //RequestStatusLog requestStatusLog = new RequestStatusLog();

            RequestWiseFile requestWiseFile = new RequestWiseFile();

            Concierge concierge = new Concierge();

            request.FirstName = model.C_FirstName;
            request.LastName = model.C_LastName;  


            concierge.ConciergeName = model.C_FirstName + " " + model.C_LastName;
            concierge.Address = model.Street + ", " + model.City + " " + model.State + " " + model.ZipCode;
            concierge.Street = model.Street;
            concierge.City = model.City;    
            concierge.State = model.State;
            concierge.ZipCode = model.ZipCode;
            concierge.CreatedDate = DateTime.Now;
            _context.Concierges.Add(concierge);
            await _context.SaveChangesAsync();

 
            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.Email = model.Email;
            requestClient.PhoneNumber = model.Phone;
            requestClient.StrMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(model.Birthdate.Month);
            requestClient.IntYear = model.Birthdate.Year;
            requestClient.IntDate = model.Birthdate.Day;
            _context.RequestClients.Add(requestClient);
            await _context.SaveChangesAsync();



            return RedirectToAction("Index", "Home");
        }
    }
}
