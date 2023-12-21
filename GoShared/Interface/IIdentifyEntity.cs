using GoShared.Helpers;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Interface
{
    public interface IIdentifyEntity
    {
        public object Base_PKValue()
        {
            return "";
        }
        string Base_PrimaryName();
        string Resource();
        void EntityDefValue(EEditType eEdit, string userID, object data);
    }
}
