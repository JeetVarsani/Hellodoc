using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModel
{
    public class AdminDashboardViewModel
    {
        public List<RequestListAdminDash> requestListAdminDash { get; set; }

        public RequestListAdminDash? adminDash { get; set; }

        public Request requestData { get; set; }

        public int StatusForName { get; set; }

        public int Regin_Short {get; set;}

        public string reqTypId { get; set; }
    }

    public class RequestListAdminDash
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }


        public string Requestor { get; set; }

        public DateTime? RequestDate { get; set; } //Createddate

        public string Phone { get; set; }

        public string? Notes { get; set; }

        public string? Address { get; set; }

        public string ChatWith { get; set; }

        public string Actions { get; set; }

        public string Physician { get; set; }

        public DateTime DateOfService { get; set; }

        public string Region { get; set; }

        public int Status { get; set; }

        public int year { get; set; }

        public int date { get; set; }

        public string month { get; set; }

        public int RequestTypeId { get; set; }
        public string rPhonenumber { get; set; }
    }
}
