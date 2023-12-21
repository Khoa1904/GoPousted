using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Caliburn.Micro;
using GoPOS.Common;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;
using static GoPOS.Common.Helpers.NativeMethods;

namespace GoPOS.ViewModels
{
    public class BasicHeaderControlViewModel : BaseItemViewModel, IHandle<ConfigUpdatedEventArgs>
    {
        public BasicHeaderControlViewModel(IWindowManager windowManager, IEventAggregator eventAggregator) : 
            base(windowManager, eventAggregator)
        {
            DataLocals.ShopInfoChanged +=DataLocals_ShopInfoChanged;
        }

        private void DataLocals_ShopInfoChanged(object sender, GoShared.Events.EventArgs<MST_INFO_SHOP> e)
        {
            NotifyOfPropertyChange(nameof(CorpName));
            NotifyOfPropertyChange(nameof(CorpTelNo));
        }

        public string VersionName => DataLocals.AppConfig.PosInfo.Version;

        public string VANName
        {
            get;
            private set;
        }

        public string? CorpName => DataLocals.ShopInfo?.CORP_NAME;
        

        public string? CorpTelNo
        {
            get
            {
                var unformatted = DataLocals.ShopInfo?.CORP_TEL_NO;
                if (unformatted != null)
                {
                    if(unformatted.Length == 9)
                    {
                        //return $"{long.Parse(unformatted):##-###-####}";
                        return unformatted.Substring(0,2) +'-'+ unformatted.Substring(2, 3) + '-' + unformatted.Substring(5, 4);
                    }
                    else if (unformatted.Length == 10)
                    {
                        //return $"{long.Parse(unformatted):##-####-####}";
                        return unformatted.Substring(0, 2) + '-' + unformatted.Substring(2, 4) + '-' + unformatted.Substring(6, 4);
                    }
                    else if (unformatted.Length == 11)
                    {
                        //return $"{long.Parse(unformatted):###-####-####}";
                        return unformatted.Substring(0, 3) + '-' + unformatted.Substring(3, 4) + '-' + unformatted.Substring(7, 4);
                    }
                    else
                    {
                        return unformatted;
                    }
                }
                else
                {
                    return string.Empty ;
                }
            }
        }

        public Task HandleAsync(ConfigUpdatedEventArgs message, CancellationToken cancellationToken)
        {
            NotifyOfPropertyChange(nameof(CorpName));
            NotifyOfPropertyChange(nameof(CorpTelNo));
            VANName = ResourceHelpers.GetVANName(DataLocals.POSVanConfig.VAN_CODE);
            NotifyOfPropertyChange(nameof(VANName));
            
            return Task.CompletedTask;
        }
    }
}
