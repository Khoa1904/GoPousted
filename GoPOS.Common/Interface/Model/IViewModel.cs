using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using System.ComponentModel;

namespace GoPOS.Common.Interface.Model
{
    public interface IViewModel : IScreen
    {
        bool SetIView(IView view);
        void SetData(object data);
        IScreen ActiveItem { get; set; }
        event EventHandler ViewInitialized;

        event EventHandler ViewLoaded;
        event EventHandler ViewUnloaded;
        event PropertyChangedEventHandler PropertyChanged;

        void InvokeViewInitialized();
        void InvokeViewLoaded();
        void InvokeViewUnloaded();

    }
}
