using BLL.Interface;
using DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BLL.Repositery
{
    public class Login : ILogin
    {
        private readonly HellodocContext _context;

        public Login(HellodocContext context)
        {
            _context = context;
        }
        public bool ValidateLogin(LoginVm loginVm)
        {   
            return _context.AspNetUsers.Any(u => u.Email == loginVm.Email && u.PasswordHash == loginVm.PasswordHash);
            
        }
    }
}
