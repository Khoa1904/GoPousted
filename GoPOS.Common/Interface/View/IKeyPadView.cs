using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Common.Interface.View
{
    public interface IKeyPadView
    {
        event EventHandler? OnBack;
        event EventHandler? OnEnter;
        event EventHandler? OnClear;
        public string Text { get; set; }
        void ClearText();
    }
}
