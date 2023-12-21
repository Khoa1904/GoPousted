using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Services
{
    public interface IOrderPaySimpleMobileService
    {
        Task<MST_INFO_EASYPAY[]> GetPayCPList();
    }
}
