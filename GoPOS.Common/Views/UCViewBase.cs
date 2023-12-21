using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using System.Windows.Controls;

namespace GoPOS.Common.Views
{
    public partial class UCViewBase : UserControl, IView
    {
        //**-------------------------------------------------------------
        //public event EventHandler<EventArgs> ViewChanged;
        protected SynchronizationContext _context;

        public event EventHandler<EventArgs> ViewChanged;

        //**-------------------------------------------------------------

        #region Property
        public virtual SynchronizationContext SyncContext => _context;
        public virtual IViewModel ViewModel => (IViewModel)this.DataContext;

        #endregion


        //**-------------------------------------------------------------

        #region Constructor
        public UCViewBase()
        {
            _context = SynchronizationContext.Current;
            this.Loaded += UCViewBase_Loaded;
            this.DataContextChanged += UCViewBase_DataContextChanged;
            this.Unloaded += UCViewBase_Unloaded;
           
        }

        private void UCViewBase_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.InvokeViewUnloaded();
        }

        private void UCViewBase_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            var model = this.DataContext as IViewModel;
            if (model != null)
            {
                model.SetIView(this);
                model.InvokeViewInitialized();
            }
        }        
        
        #endregion

        //**-------------------------------------------------------------

        #region Puclic IView
        public virtual void Translate()
        {
            // Read langu from xml and mapper contron name
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enable"></param>
        public virtual void EnableControl(bool enable)
        {

        }

        #endregion

        //**-------------------------------------------------------------

        #region Event
        protected void UCViewBase_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewModel?.InvokeViewLoaded();
            Translate();
        }

        #endregion




    }
}
