using BLL.Interface;
using DAL.Models;
using DAL.ViewModel;
using System;
using System.Collections.Generic;
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

                Name = r.RequestClients.Select(x => x.FirstName).First() + " " + r.RequestClients.Select(x => x.LastName).First(),
                Requestor = r.FirstName + " " + r.LastName,
                RequestDate = r.CreatedDate,
                Phone = r.PhoneNumber,
                Address = r.RequestClients.Select(x => x.Street).First() + "," + r.RequestClients.Select(x => x.City).First() + "," + r.RequestClients.Select(x => x.State).First(),
                Notes = r.RequestClients.Select(x => x.Notes).First(),
                ChatWith = r.PhysicianId.ToString(),
                Physician = r.Physician.FirstName,
                Status = r.Status,
                year = (int)_context.RequestClients.Select(x => x.IntYear).First(),
                date = (int)_context.RequestClients.Select(x => x.IntDate).First(),
                month = _context.RequestClients.Select(x => x.StrMonth).First(),
                rPhonenumber = r.PhoneNumber,
                RequestTypeId = r.RequestTypeId,


            }).ToList();  
            return GetRequestData;
        }
    }
}
