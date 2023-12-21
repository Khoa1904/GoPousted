using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.Model
{
    public interface IPage
    {
        void Initialize();
        void SetData();
        void ClearData();
    }
}
