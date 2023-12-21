using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GoShared.Helpers.Nini.Ini;
using GoShared.Helpers.Nini.Config;
using GoPOS.Models.Config;

namespace GoShared
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        static public string GetFolder(this string subFolder)
        {
            return GetFolder(subFolder, false);
        }

        static public string GetFolder(this string subFolder, bool sameLevel)
        {
            string appFolder = GetAppBaseFolder();
            DirectoryInfo di = new DirectoryInfo(appFolder);
            string path = Path.Combine(sameLevel ? di.FullName : di.Parent.FullName, subFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// Get folder under lib folder of current application directory
        /// 
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        static public string GetLibFolder(this string subFolder)
        {
            string appFolder = GetAppBaseFolder();
            string path = string.Format("{0}{1}lib{1}{2}", appFolder, Path.DirectorySeparatorChar, subFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        static public string GetPocketFolder(this string subFolder)
        {
            string appFolder = GetPocketFolder();
            DirectoryInfo di = new DirectoryInfo(appFolder);
            string path = Path.Combine(di.Parent.FullName, subFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        /// <summary>
        /// exe path
        /// </summary>
        /// <returns></returns>
        static public string GetAppBaseFolder()
        {
            string appFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (appFolder.StartsWith("file:\\"))
            {
                appFolder = appFolder.Substring("file:\\".Length);
            }

            return appFolder;
        }

        /// <summary>
        /// Pocket path
        /// </summary>
        /// <returns></returns>
        static public string GetPocketFolder()
        {
            string appFolder = string.Format("PocketStore{0}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            if (appFolder.StartsWith("file:\\"))
            {
                appFolder = appFolder.Substring("file:\\".Length);
            }

            return appFolder;
        }

        /// <summary>
        /// Get parent folder full
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        static public string GetParentFolder(this string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            return di.Parent.FullName;
        }

        public static byte[] ReadAllBytes(this string filePath)
        {
            byte[] buffer;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                int offset = 0;
                int count = (int)fs.Length;
                buffer = new byte[count];
                while (count > 0)
                {
                    int bytesRead = fs.Read(buffer, offset, count);
                    offset += bytesRead;
                    count -= bytesRead;
                }
            }

            return buffer;
        }

        /// <summary>
        /// load image from resource folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public Image ResourceLoad(string fileName)
        {
            string path = Path.Combine("resource".GetFolder(), fileName);
            if (!File.Exists(path))
            {
                return null;
            }

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return new Bitmap(fs);
            }
        }

        /// <summary>
        /// propName of obj
        /// is class, 
        /// dsData, dataTable columns: code/value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propName"></param>
        /// <param name="dsData"></param>
        static public void FromDataSet(this IniConfig obj, string propName, DataSet dsData)
        {
            if (dsData == null || !dsData.Tables.Contains(propName))
            {
                return;
            }

            var pi = obj.GetType().GetProperty(propName);
            if (pi == null)
            {
                return;
            }

            // posinfo value
            var pVal = pi.GetValue(obj, null);

            foreach (DataRow dr in dsData.Tables[propName].Rows)
            {
                string pName = dr["code"].ToString();
                string pValue = dr["value"].ToString();

                var spi = pVal.GetType().GetProperty(pName);
                if (spi == null)
                {
                    continue;
                }

                spi.SetValue(pVal, pValue, null);
            }

            pi.SetValue(obj, pVal, null);
        }

        /// <summary>
        /// Load IniConfig field from a section
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="section"></param>
        static public void FromIniSection(this BaseIniConfig obj, IniSection section)
        {
            var pi = obj.GetType().GetProperty(section.Name);
            if (pi == null)
            {
                return;
            }

            // posinfo value
            var pVal = pi.GetValue(obj, null);

            string[] keys = section.GetKeys();
            foreach (var key in keys)
            {
                string pValue = section.GetValue(key);
                var spi = pVal.GetType().GetProperty(key);
                if (spi == null)
                {
                    continue;
                }

                try
                {
                    spi.SetValue(pVal, pValue, null);
                }
                catch
                {
                }
            }

            pi.SetValue(obj, pVal, null);
        }

        public static List<Assembly> GoPOSAssemblies = null;
    }
}
