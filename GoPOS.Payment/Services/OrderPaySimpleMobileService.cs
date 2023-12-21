using GoPOS.Models;
using GoPOS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Payment.Services
{
    public class OrderPaySimpleMobileService : IOrderPaySimpleMobileService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<MST_INFO_EASYPAY[]> GetPayCPList()
        {
            using (var db = new DataContext())
            {
                return Task.FromResult(db.mST_INFO_EASYPAYs.OrderBy(p => p.PAYCP_CODE).ToArray());
            }
        }
    }
}
