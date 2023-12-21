using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GoPOS.Models.Config
{
    public class BaseIniConfig
    {
        public string GetConfigByName(string sectionName, string configName)
        {
            // Get propert
            var pi = this.GetType().GetProperty(sectionName);
            if (pi != null)
            {
                var piv = pi.GetValue(this, null);
                var pi1 = piv.GetType().GetProperty(configName);
                if (pi1 != null)
                {
                    return pi1.GetValue(piv, null).ToString();
                }
            }

            return string.Empty;
        }
    }
}
