using GoPOS.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using GoPOS.Common.Interface.View;
using GoPOS.OrderPay.Interface.View;
using System.Collections.ObjectModel;
using GoPOS.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GoPOS.Services;
using System.Windows.Media;
using System.Windows;

namespace GoPOS.OrderPay.ViewModels
{
    public class PostPayTableDetailViewModel
    {
        private IPostPayTableDetailView _view;

        private TABLE_THR _dynamicContent;
        public TABLE_THR DynamicContent
        {
            get
            { return _dynamicContent; }
            set
            {
                if (_dynamicContent != value)
                {
                    _dynamicContent = value;
                    OnPropertyChanged(nameof(DynamicContent));
                }
            }
        }

        private ObservableCollection<MST_TABLE_DEPT> _properties;
        public ObservableCollection<MST_TABLE_DEPT> Properties
        {
            get
            { return _properties; }
            set
            {
                if (_properties != value)
                {
                    _properties = value;
                    OnPropertyChanged(nameof(Properties));
                }
            }
        }

        public PostPayTableDetailViewModel()
        {
        }
        public bool SetIView(IPostPayTableDetailView view)
        {
            _view = (IPostPayTableDetailView)view;
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
public class TABLE_THR
{
    public string? SHOP_CODE { get; set; }
    public string? TABLE_FLAG { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double WIDTH { get; set; }
    public double HEIGHT { get; set; }
    public string? USE_YN { get; set; }
    public string? INSERT_DT { get; set; }
    public string? UPDATE_DT { get; set; }
    public string TABLE_CODE { get; set; }
    public string TABLE_NAME { get; set; }
    public string SHAPE_FLAG { get; set; }
    public string TG_CODE { get; set; }
    public List<ORDER_GRID_ITEM> ORDER_ITEMS { get; set; }

    private Brush _myBackgroundtitle;
    public Brush MyBackgroundTitle
    {
        get => SHAPE_FLAG != "3" ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
        set
        {
            _myBackgroundtitle = value;
        }
    }

    private Brush _myBackgroundbody;
    public Brush MyBackgroundBody
    {
        get => SHAPE_FLAG != "3" ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
        set
        {
            _myBackgroundbody = value;
        }
    }

    private Visibility _imageBackground;
    public Visibility ImageBackground
    {
        get => SHAPE_FLAG != "3" ? Visibility.Visible : Visibility.Hidden;
        set
        {
            _imageBackground = value;
        }
    }

    public Visibility HiddenPropertiers { get; set; } = Visibility.Hidden;
}