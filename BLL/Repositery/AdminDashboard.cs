using BLL.Interface;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositery
{
    public class AdminDashboard : IAdminDashboard
    {
        private readonly HellodocContext _context;
        public AdminDashboard(HellodocContext context) 
        {
            _context = context;

        }
        public List<RequestListAdminDash> requestDataAdmin(int Status, string reqTypeId, int RegionId)
        {
            //var requestTypeId = _context.Requests.Where(o => o.RequestTypeId == reqTypeId);
            var requestList = _context.Requests.Where(o => o.Status == Status);

            if (reqTypeId != null)
            {
                requestList = requestList.Where(o => o.Status == Status && o.RequestTypeId.ToString() == reqTypeId);
            }
            if(RegionId != 0)
            {
            var requestdata = _context.RequestClients.Where(i => i.RegionId == RegionId);
            requestList = requestList.Where(i => i.RequestId == requestdata.Select(u => u.RequestId).First());

            }
            var GetRequestData = requestList.Select(r => new RequestListAdminDash() {

                RequestId = r.RequestId,
                Name = r.RequestClients.Select(x => x.FirstName).First() + " " + r.RequestClients.Select(x => x.LastName).First(),
                Requestor = r.FirstName + " " + r.LastName,
                RequestDate = r.CreatedDate,
                Address = r.RequestClients.Select(x => x.Street).First() + "," + r.RequestClients.Select(x => x.City).First() + "," + r.RequestClients.Select(x => x.State).First(),
                Notes = r.RequestClients.Select(x => x.Notes).First(),
                ChatWith = r.PhysicianId.ToString(),
                Physician = r.Physician.FirstName,
                Status = r.Status,
                year = (int)_context.RequestClients.Select(x => x.IntYear).First(),
                date = (int)_context.RequestClients.Select(x => x.IntDate).First(),
                month = _context.RequestClients.Select(x => x.StrMonth).First(),
                //DateOfBirth = new DateTime(year, DateTime.ParseExact(month, "MMM", CultureInfo.InvariantCulture).Month, date),

                rPhonenumber = r.PhoneNumber,
                RequestTypeId = r.RequestTypeId,


            }).ToList();  
            return GetRequestData;
        }

        public List<RequestListAdminDash> ViewCase(int requestId)
        {

            //var requestTypeId = _context.Requests.Where(o => o.RequestTypeId == reqTypeId);
            var requestList = _context.Requests.Where(o => o.RequestId == requestId);
            var user = _context.RequestClients.FirstOrDefault(x => x.RequestId == requestId);

            int intYear = (int)user.IntYear;
            int intDate = (int)user.IntDate;
            string month = (string)user.StrMonth;
            DateTime birthdate = new DateTime(intYear, DateTime.ParseExact(month, "MMM", CultureInfo.InvariantCulture).Month, intDate);
            var GetRequestData = requestList.Select(r => new RequestListAdminDash()
            {
                
                RequestId = r.RequestId,
                FirstName = r.RequestClients.Select(x => x.FirstName).First(),
                LastName = r.RequestClients.Select(x => x.LastName).First(),
                Email = r.Email,
                Name = r.RequestClients.Select(x => x.FirstName).First() + " " + r.RequestClients.Select(x => x.LastName).First(),
                Requestor = r.FirstName + " " + r.LastName,
                RequestDate = r.CreatedDate,
                Phone = r.PhoneNumber,
                Address = r.RequestClients.Select(x => x.Street).First() + "," + r.RequestClients.Select(x => x.City).First() + "," + r.RequestClients.Select(x => x.State).First(),
                Notes = r.RequestClients.Select(x => x.Notes).First(),
                ChatWith = r.PhysicianId.ToString(),
                Physician = r.Physician.FirstName,
                Status = r.Status,
               
                rPhonenumber = r.PhoneNumber,
                RequestTypeId = r.RequestTypeId,

            }).ToList();
            return GetRequestData;
        }


        public ViewNotesVm ViewNotes(int requestId)
        {
            

            var user = _context.RequestNotes.FirstOrDefault(u => u.RequestId == requestId);
            var usertwo = _context.RequestStatusLogs.FirstOrDefault(x => x.RequestId == requestId);

            if (user != null && usertwo != null)
            {
                ViewNotesVm viewNotesVm = new ViewNotesVm()
                {
                    AdminNotes = user.AdminNotes,
                    PhysicianNotes = user.PhysicianNotes,
                    TransferNotes = usertwo.Notes,
                    RequestId = user.RequestId,
                };
                return viewNotesVm;
            }
            else
            {
                var usermain = _context.Requests.FirstOrDefault(x => x.RequestId == requestId);
                _context.RequestNotes.Add(new RequestNote()
                {
                    RequestId = requestId,
                    CreatedBy = (int)_context.Users.FirstOrDefault(x => x.UserId == usermain.UserId).AspNetUserId,
                    CreatedDate = DateTime.Now,
                });
                _context.SaveChanges();


                _context.RequestStatusLogs.Add(new RequestStatusLog()
                {
                    RequestId = requestId,
                    CreatedDate = DateTime.Now,
                });
                _context.SaveChanges();


                var edituser = _context.RequestNotes.FirstOrDefault(u => u.RequestId == requestId);
                var editusertwo = _context.RequestStatusLogs.FirstOrDefault(x => x.RequestId == requestId);

                ViewNotesVm viewNotesVm = new ViewNotesVm()
                {
                    AdminNotes = edituser.AdminNotes,
                    PhysicianNotes = edituser.PhysicianNotes,
                    TransferNotes = editusertwo.Notes,
                    RequestId = edituser.RequestId,
                };
                return viewNotesVm;
            }
        }

        public void editViewNotes(ViewNotesVm model, int requestId)
        {
            var reqnotes = _context.RequestNotes.FirstOrDefault(x => x.RequestId == requestId);
            var req = _context.Requests.FirstOrDefault(x => x.RequestId == requestId);
            if(reqnotes != null)
            {
                reqnotes.AdminNotes = model.AdminNotes;
                reqnotes.PhysicianNotes = model.PhysicianNotes;
                reqnotes.ModifiedBy = (int)_context.Users.FirstOrDefault(x => x.UserId == req.UserId).AspNetUserId;
                //reqnotes.ModifiedBy = _context.Requests.Where(x => x.RequestId == model.RequestId).Select(x => x.User.AspNetUserId).First();
                reqnotes.ModifiedDate = DateTime.Now;
                _context.SaveChanges();
            }
            

        }
    }
}
