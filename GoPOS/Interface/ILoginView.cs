using GoPOS.Common.Interface.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoPOS.Interface
{
    public interface ILoginView : IView
    {
        TextBox txtEMP_NO { get; }
        TextBox txtEMP_PWD { get; }
        bool IsPasswordFocused();
        bool PasswordFocused { get; set; }
    }
}
