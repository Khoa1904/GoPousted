using Caliburn.Micro;
using GoShared.Helpers;
using static GoShared.Events.GoPOSEventHandler;
using System.Windows;
using GoPOS.Helpers.CommandHelper;
using System.Windows.Input;
using System.Windows.Controls;
using GoPOS.Models.Common;
using GoShared.Contract;
using System.ComponentModel;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.Interface.View;
using GoShared.Events;
using GoPOS.Helpers;
using System.Windows.Markup;
using System.IO;
using System.Reflection;
using GoShared;
using System.Net.NetworkInformation;
using Microsoft.IdentityModel.Tokens;
using System.Windows.Threading;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using GoPOS.Models;
using GoPOS.ViewModels;
using GoPOS.Common.Helpers;

namespace GoPOS.Common.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    ///        
    public class MainBasePageViewModel : BasePageViewModel, IMainBasePageViewModel
    {
        public List<FkMapInfo>? FunKeyMaps { get; set; }
        public List<ORDER_FUNC_KEY>? ExtMenus { get; set; }

        public MainBasePageViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) :
            base(windowManager, eventAggregator)
        {
            // Load AllFunKeyMaps
            FunKeyMaps = ResourceHelpers.FunKeyMaps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapKey"></param>
        /// <param name="csParams"></param>
        public virtual void ProcessFuncKeyClicked(FkMapInfo mapKey, params object[] csParams)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fnKeyNo"></param>
        public void ProcessFuncKeyClicked(string fnKeyNo)
        {
            var fkKey = ExtMenus?.FirstOrDefault(p => p.FK_NO == fnKeyNo);
            ProcessFuncKeyClicked(fkKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fkKey"></param>
        public void ProcessFuncKeyClicked(ORDER_FUNC_KEY fkKey, params object[] csParams)
        {
            if (fkKey == null) return;
            var mapKey = FunKeyMaps.FirstOrDefault(p => p.FK_NO == fkKey.FK_NO);
            if (mapKey == null || string.IsNullOrEmpty(mapKey.ViewModelName)) return;

            OnFuncKeyClicked(fkKey, mapKey);
            if (mapKey.IsPopup == FkMapActionTypes.Popup)
            {
                // mapKey.CSParams, width, height
                DialogHelper.ShowDialog(mapKey.ViewModelName, Convert.ToDouble(mapKey.CSParams[0]), Convert.ToDouble(mapKey.CSParams[1]), ValidateOnChildActivated);
            }
            else if (mapKey.IsPopup == FkMapActionTypes.ActiveItem)
            {
                this.ActiveForm(mapKey.ItemArea, mapKey.ViewModelName, ValidateOnChildActivated, csParams);
            }
            else
            {
                ProcessFuncKeyClicked(mapKey, csParams);
            }
        }

        protected virtual void OnFuncKeyClicked(ORDER_FUNC_KEY fnKey, FkMapInfo fkMap)
        {

        } 

        /// <summary>
        /// validate before moving to child, or popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ValidateOnChildActivated(object sender, ChildActivatedEventArgs e)
        {

        }
    }
}
