using BLL.Interface;
using DAL.Models;
using DAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace DAL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminDashboard _adminDashboard;
        private readonly HellodocContext _context;

        public AdminController(HellodocContext context, IAdminDashboard adminDashboard) {
            _context = context;
            _adminDashboard = adminDashboard;

        }
        public IActionResult AdminDashboard(int Status, string reqtypeid, int RegionId)
        {

            var requestAdmin=_adminDashboard.requestDataAdmin(1,null, 0);
            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel()
            {
                requestListAdminDash = requestAdmin,
                StatusForName = Status,
                reqTypId = reqtypeid,
                Regin_Short = RegionId

                
            };
            return View(adminDashboardViewModel);
        }
        public IActionResult fetchRequests(int Status, string reqtypeid, int RegionId) 
        {

            var requestAdmin = _adminDashboard.requestDataAdmin(Status,reqtypeid, RegionId);
            AdminDashboardViewModel adminDashboardViewModel = new AdminDashboardViewModel()
            {
                requestListAdminDash = requestAdmin,
                StatusForName= Status,
                Regin_Short = RegionId,
                reqTypId = reqtypeid,


            };
            return PartialView("_RequestsAccToStatus",adminDashboardViewModel);
        }

        public IActionResult AdminMyProfile() { 
            return View(); 
        }

        public IActionResult CloseCase() {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Transfer()
        {
            return View();
        }
        public IActionResult ViewCase()
        {
            return View();
        }
        public IActionResult ViewNotes()
        {
            return View();
        }
        public IActionResult ViewUploads() {
            return View();
        }
    }
}
