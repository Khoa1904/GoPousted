using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoPOS.Models;
using GoPOS.Models.Custom.API;

namespace GoPOS.Service.Service
{
    public interface IAuthenticLoginService
    {
        Task<ResponseModel> Login(string username, string password);
        void Logout();
        bool IsUserLoggedIn { get; }
    }
}
