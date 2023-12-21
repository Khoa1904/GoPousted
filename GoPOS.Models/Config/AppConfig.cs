using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using System.IO;
using System.Data;
using System.Collections;

using GoShared.Helpers.Nini.Ini;
using GoShared;
using GoPOS.Models.Common;

namespace GoPOS.Models.Config
{
    public class AppConfig : BaseIniConfig
    {
        public const string PathConfig = "Config";
        public const string FileConfig = "GOPOS.ini";
        public const string MenuConfig = "MenuConfig.xml";

        private static string appConfigFile = Path.Combine(PathConfig.GetFolder(), FileConfig);

        private static string[] sectionNames = { "PosInfo", "PosComm" };

        public PosInfo PosInfo { get; set; }
        public PosComm PosComm { get; set; }
        public PosOption PosOption { get; set; }

        public AppConfig()
        {
            PosInfo = new PosInfo();
            PosComm = new PosComm();
        }

        /// <summary>
        /// Load IniConfig from ini file
        /// </summary>
        /// <returns></returns>
        public static AppConfig Load()
        {
            var appConfig = new AppConfig();
            IniDocument doc = new IniDocument(appConfigFile);

            foreach (DictionaryEntry section in doc.Sections)
            {
                if (!sectionNames.Contains(section.Key))
                {
                    continue;
                }
                appConfig.FromIniSection(doc.Sections[section.Key.ToString()]);
            }

            return appConfig;
        }

        /// <summary>
        /// Save ini file
        /// </summary>
        public void Save()
        {
            IniDocument doc = new IniDocument();

            foreach (var secName in sectionNames)
            {
                var pi = this.GetType().GetProperty(secName);
                if (pi == null)
                {
                    continue;
                }

                var piv = pi.GetValue(this, null);
                if (piv == null)
                {
                    continue;
                }

                IniSection section = new IniSection(secName);

                var pil = piv.GetType().GetProperties();
                foreach (var item in pil)
                {
                    var v = item.GetValue(piv, null);
                    //ConfigReader.fnWriteIni(secName, item.Name, v == null ? string.Empty : v.ToString(), appConfigFile);
                    section.Set(item.Name, v == null ? string.Empty : v.ToString());
                }

                doc.Sections.Add(section);
            }

            try
            {
                lock (this)
                {
#if (PocketPC)
                    string tempPath = Path.Combine(FXConsts.FOLDER_CONFIG.GetPocketFolder(), "temp.ini");
                    File.Copy(appConfigFile, tempPath, true);
                    System.Threading.Thread.Sleep(100);
#endif
                    doc.Save(appConfigFile);
                }
            }
            catch
            {
            }
        }

        public static bool Exists()
        {
            return File.Exists(appConfigFile);
        }
    }
}
