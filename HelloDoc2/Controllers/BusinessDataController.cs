using Microsoft.AspNetCore.Mvc;
using HelloDoc2.Models.ViewModel;
using HelloDoc2.Models;


namespace HelloDoc2.Controllers
{
    public class BusinessDataController : Controller
    {

        HellodocContext _context;

        public BusinessDataController(HellodocContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(BusinessData model)
        {

            Request request = new Request();

            RequestClient requestClient = new RequestClient();

            RequestWiseFile requestWiseFile = new RequestWiseFile();

            Business business = new Business();


            business.Name = model.B_FirstName + " " + model.B_LastName;
            business.CreatedDate = DateTime.Now;
            _context.Businesses.Add(business);
            await _context.SaveChangesAsync();


            requestClient.FirstName = model.FirstName;
            requestClient.LastName = model.LastName;
            requestClient.Email = model.Email;
            requestClient.PhoneNumber = model.Phone;
            requestClient.Street = model.Street;
            requestClient.City = model.City;
            requestClient.State = model.State;
            requestClient.ZipCode = model.ZipCode;
            _context.RequestClients.Add(requestClient);
            await _context.SaveChangesAsync();

            request.FirstName = model.B_FirstName;
            request.LastName = model.B_LastName;
            request.Email = model.B_Email;
            request.PhoneNumber = model.B_Phone;
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Home");
        }
    }
}
