using GoPOS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoPOS.Common.Helpers
{
    public class ResourceHelpers
    {
        static List<FkMapInfo> _fkMapInfos = null;
        public static List<FkMapInfo> FunKeyMaps
        {
            get
            {
                if (_fkMapInfos == null)
                {
                    LoadFnKeyMappings();
                }

                return _fkMapInfos;
            }
        }
        static void LoadFnKeyMappings()
        {
            _fkMapInfos = new List<FkMapInfo>();
            var assembly = Assembly.LoadFile(Environment.CurrentDirectory + "\\GoPOS.Resources.dll");
            var resourceName = "GoPOS.Resources.maps.FkMappings.ini";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                lines.ForEach(l =>
                {
                    var ps = l.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    _fkMapInfos.Add(new FkMapInfo(ps[0], ps.Length > 1 ? ps[1] : string.Empty));
                });
            }
        }

        static MasterTableIdInfo[] _masterTableIdInfos = null;
        public static MasterTableIdInfo[] MasterTableIds
        {
            get
            {
                if (_masterTableIdInfos == null)
                {
                    LoadMasterTableIds();
                }

                return _masterTableIdInfos;
            }
        }

        private static void LoadMasterTableIds()
        {
            var list = new List<MasterTableIdInfo>();
            var assembly = Assembly.LoadFile(Environment.CurrentDirectory + "\\GoPOS.Resources.dll");
            var resourceName = "GoPOS.Resources.maps.mst_tableids.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                lines.ForEach(l =>
                {
                    var ps = l.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    list.Add(new MasterTableIdInfo()
                    {
                        MST_ID = ps[0],
                        MST_TLNAME = ps[1],
                        MST_TPNAME = ps[2]
                    });
                });
            }

            _masterTableIdInfos = list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vanCode"></param>
        /// <returns></returns>
        public static string GetVANName(string vanCode)
        {
            var list = new List<MasterTableIdInfo>();
            var assembly = Assembly.LoadFile(Environment.CurrentDirectory + "\\GoPOS.Resources.dll");
            var resourceName = "GoPOS.Resources.maps.van_list.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                var lines = result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var line = lines.FirstOrDefault(p => p.StartsWith(vanCode));
                return !string.IsNullOrEmpty(line) ? line.Substring(line.IndexOf("-") + 1) : line;
            }

        }

        public static string LoadSqlCommand(SQLFileTypes sqlType, string commandName)
        {
            string directory = string.Empty + Directory.GetParent(Environment.CurrentDirectory);
            directory += string.Format(@"\data\sql\{0}.sql", Enum.GetName(typeof(SQLFileTypes), sqlType));
            var fileContent = File.ReadAllText(directory);
            Match m = Regex.Match(fileContent, string.Format(@"(?s)(?<=(?<!/+\s*)<{0}>\s+)(?!//).+\s(?=</{0}>)", commandName));
            if (m.Success)
            {
                return m.Groups[0].Value;
            }

            return string.Empty;
        }
    }

    public enum SQLFileTypes
    {
        OrderPay,
        Payment,
        Sales,
        SellingStatus,
        ConfigSetup,
        AccountSettlement,
        Printer,
        ProductSalesStatus,
        ResetDB,
        PostPay
    }
}
