using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Services;

using GoPOS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


/*
 공통 > 메시지 박스

 */

namespace GoPOS.Common.ViewModels
{

    public class InputKeyPadRoundViewModel : BaseItemViewModel
    {
        private IKeyPadView _view;
        public event EventHandler OnEnter;

        public InputKeyPadRoundViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : base(windowManager, eventAggregator)
        {
            this.ViewInitialized += InputKeyPadViewRoundModel_ViewInitialized;
        }

        private void InputKeyPadViewRoundModel_ViewInitialized(object? sender, EventArgs e)
        {
            _view.OnEnter += _view_OnEnter;
            _view.OnClear += _view_OnClear;
        }

        private void _view_OnClear(object? sender, EventArgs e)
        {
            BaseKeyPad.InvokeKey(_eventAggregator, Key.Clear);
        }

        private void _view_OnEnter(object? sender, EventArgs e)
        {
            OnEnter?.Invoke(sender, e);
        }

        private string _Text = string.Empty;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public void ClearText()
        {
            _view.ClearText();
        }

        public override bool SetIView(IView view)
        {
            _view = view as IKeyPadView;
            if (_view == null)
            {
                return false;
            }

            return base.SetIView(view);
        }
    }
}