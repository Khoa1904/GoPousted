using GoPOS.Models.Custom.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Service
{
    public interface IAuthenticProcessingService
    {
        Task<AuhthenResponseModelResult> AuthenticateValidateLicense(ListStoreModel model, AuthenRequestHeader header);

        Task<bool> SaveDataPosKeyMang();

    }
}
