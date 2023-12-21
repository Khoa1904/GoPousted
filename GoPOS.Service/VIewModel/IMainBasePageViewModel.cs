using GoPOS.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace GoPOS.ViewModels
{
    public interface IMainBasePageViewModel
    {
        List<FkMapInfo>? FunKeyMaps { get; set; }
        List<ORDER_FUNC_KEY>? ExtMenus { get; set; }
        void ProcessFuncKeyClicked(string fnKeyNo);
        void ProcessFuncKeyClicked(ORDER_FUNC_KEY fkKey, params object[] csParams);
        void ProcessFuncKeyClicked(FkMapInfo mapKey, params object[] csParams);
    }
}
    