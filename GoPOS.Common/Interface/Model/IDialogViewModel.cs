using System;

namespace GoPOS.Common.Interface.Model
{
    public interface IDialogViewModel
    {
        Dictionary<string, object> DialogResult { get; }
    }

}