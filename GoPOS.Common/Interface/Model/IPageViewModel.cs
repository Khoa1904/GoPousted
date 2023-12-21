using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.Model
{
    public interface IPageViewModel : IViewModel
    {
        void ActiveForm(string formName, Type formItemType);
        void ActiveForm(string formName, string viewModelName);
        void ChildActivated(string areaName, bool activated, object data);
        void SetData(object data);
    }
}
