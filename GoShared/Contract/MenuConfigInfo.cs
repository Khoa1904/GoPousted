using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Contract
{
    public class MenuConfigInfo
    {
        public string ParentMenu { get; set; } = string.Empty;
        public string MenuCode { get; set; } = string.Empty;
        public string ViewModelNm { get; set; } = string.Empty;
        public string MenuName { get; set; } = string.Empty;
        public string MenuDescription { get; set; } = string.Empty;
        public string MenuType { get; set; } = string.Empty;
        public string MenuIcon { get; set; } = string.Empty;
        public string MenuIconUrl { get; set; } = string.Empty;
        public string MenuTitle { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;

        public string ModelName { get; set; } = string.Empty;
        public string Assembly { get; set; } = string.Empty;
        public string ModelNamespace { get; set; } = string.Empty;
        public int Sort { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsEnable { get; set; } = true;
        public bool IsUse { get; set; } = true;
    }
}
