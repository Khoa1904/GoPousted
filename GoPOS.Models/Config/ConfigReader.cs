using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace GoPOS.Models.Config
{
    /// <summary>
    ///  CONFIG INI 파일을 다루기 위한 클레스
    /// </summary>
    public class ConfigReader
    {


        /// <summary> 
        /// ini 파일쓰기
        /// <param name="strSection">section name</param>
        /// <param name="strKey">key</param>
        /// <param name="strValue">value</param>  
        /// <param name="strFilePath">file path</param> 
        /// </summary> 
        public static void fnWriteIni(string strSection, string strKey, string strValue, string strFilePath)
        {
            commIniParse(false, strSection, strKey, strValue, strFilePath);
        }

        /// <summary> 
        /// ini 파일읽기
        /// <param name="strSection">section name</param>
        /// <param name="strKey">key</param>
        /// <param name="strDefault">default</param>  
        /// <param name="strFilePath">file path</param> 
        /// </summary> 
        public static string fnReadIni(string strSection, string strKey, string strDefault, string strFilePath)
        {
            return commIniParse(true, strSection, strKey, strDefault, strFilePath);
        }

        /// <summary> 
        /// SysMessage DataTable 넣기
        /// <param name="strFilePath">file path</param> 
        /// </summary> 
        public static DataTable fnSetSysMessageIni(string strFilePath)
        {
            return commonSysMsg(strFilePath);
        }


        public static string[] fnSplit(string input, string pattern)
        {
            string[] arr = System.Text.RegularExpressions.Regex.Split(input, pattern);
            return arr;
        }

        public static string fnTrim(string input)
        {
            string strTrim = "";

            if (!input.Equals("") && input != null)
                strTrim = input.Trim(); ;

            return strTrim;
        }

        private static void AppendToFile(string strPath, string strContent)
        {
            FileStream fs = new FileStream(strPath, FileMode.Append);
            StreamWriter streamWriter = new StreamWriter(fs, System.Text.Encoding.UTF8);
            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            streamWriter.WriteLine(strContent);
            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }

        private static void WriteArray(string strPath, string[] strContent)
        {
            FileStream fs = new FileStream(strPath, FileMode.Truncate);
            StreamWriter streamWriter = new StreamWriter(fs, System.Text.Encoding.UTF8);
            streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < strContent.Length; i++)
            {
                if (strContent[i].Trim() == "\r\n")
                    continue;
                streamWriter.WriteLine(strContent[i].Trim());
            }
            streamWriter.Flush();
            streamWriter.Close();
            fs.Close();
        }

        //INI parsing 
        private static string commIniParse(bool isRead, string sectionName, string key, string val, string filePath)
        {
            string strSection = "[" + sectionName + "]";
            string strBuf;
            try
            {
                // A file does not exist, create 
                if (!File.Exists(filePath))
                {
                    FileStream sr = File.Create(filePath);
                    sr.Close();
                }
                //Read INI file 
                System.IO.StreamReader stream = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
                strBuf = stream.ReadToEnd();
                stream.Close();
            }
            catch (Exception e)
            {
                return e.ToString();
            }

            string[] rows = fnSplit(strBuf, "\r\n");
            string rowData;
            int i = 0;

            for (; i < rows.Length; i++)
            {
                rowData = rows[i].Trim();
                //Empty line 
                if (0 == rowData.Length)
                {
                    continue;
                }

                //This is a comment 
                if ("'" == rowData[0].ToString().Substring(0, 1))
                {
                    continue;
                }

                //Did not find 
                if (strSection != rowData)
                {
                    continue;
                }

                //Find 
                break;
            }
            // B could not find the corresponding section, create a create a property 
            if (i >= rows.Length)
            {
                AppendToFile(filePath, "\r\n" + strSection + "\r\n" + key + "=" + val);
                return val;
            }

            // Find the section
            i += 1; // skip section

            int bakIdxSection = i;// the next line of the backup section
            string[] strKeys;

            // Find Properties 
            for (; i < rows.Length; i++)
            {
                rowData = rows[i].Trim();
                // Empty line 
                if (0 == rowData.Length)
                {
                    continue;
                }

                // This is a comment 
                if (';' == rowData[0])
                {
                    continue;
                }

                // Cross-border 
                if ('[' == rowData[0])
                {
                    break;
                }

                strKeys = fnSplit(rowData, "=");
                if (strKeys == null || strKeys.Length != 2)
                {
                    continue;
                }

                // Find property 
                if (strKeys[0].Trim() == key)
                {
                    //Read 
                    if (isRead)
                    {
                        if (0 == strKeys[1].Trim().Length)
                        {
                            rows[i] = strKeys[0].Trim() + "=" + val;
                            WriteArray(filePath, rows);
                            return val;
                        }
                        else
                        {
                            // Find 
                            return strKeys[1].Trim();
                        }
                    }
                    //Write 
                    else
                    {
                        rows[i] = strKeys[0].Trim() + "=" + val;
                        WriteArray(filePath, rows);
                        return val;
                    }
                }
            }
            //내용생성
            //rows[bakIdxSection] = rows[bakIdxSection] + "\r\n" + key + "=" + val; 
            //WriteArray(filePath, rows); 
            return val;
        }

        //INI parsing 
        public static string[] commIniTaskParse(string filePath)
        {
            string strBuf = ""; ;
            try
            {
                // A file does not exist, create 
                if (!File.Exists(filePath))
                {
                    FileStream sr = File.Create(filePath);
                    sr.Close();
                }
                //Read INI file 
                System.IO.StreamReader stream = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
                strBuf = stream.ReadToEnd();
                stream.Close();
            }
            catch (Exception e)
            {
            }

            string[] rows = fnSplit(strBuf, "\r\n");

            return rows;
        }


        //SysMessage.ini read 후 datatable에 저장
        private static DataTable commonSysMsg(string FileName)
        {
            DataTable dt = new DataTable();
            DataRow dRow = null;

            dt.Columns.Add(new DataColumn("code", typeof(string)));
            dt.Columns.Add(new DataColumn("msg", typeof(string)));

            string strBuf;

            // 파일이 존재하지 않으면 생성
            if (!File.Exists(FileName))
            {
                FileStream sr = File.Create(FileName);
                sr.Close();
            }

            //INI파일 전체 Read
            System.IO.StreamReader stream = new System.IO.StreamReader(FileName, System.Text.Encoding.UTF8);
            strBuf = stream.ReadToEnd();
            stream.Close();

            string[] rows = fnSplit(strBuf, "\r\n");
            string[] temp;
            string strRow;

            for (int i = 1; i < rows.Length; i++)
            {
                dRow = dt.NewRow();
                strRow = rows[i].Trim();
                if (strRow != string.Empty && strRow != null)
                {
                    temp = fnSplit(strRow, "=");

                    dRow["code"] = temp[0].Trim();
                    dRow["msg"] = temp[1].Trim();
                    dt.Rows.Add(dRow);
                }
                else
                {
                    break;
                }
            }
            return dt;
        }


        /// <summary> 
        /// dat 파일읽기
        /// <param name="filePath">file path</param> 
        /// </summary> 
        public static string fnReadDat(string filePath)
        {
            return commDatParse(filePath);
        }

        /// <summary> 
        /// INI 파일읽기
        /// <param name="sections">section names</param> 
        /// <param name="cols">colums names</param> 
        /// <param name="filePath">file path</param> 
        /// <return>dataset</return>
        /// </summary> 
        public static DataSet fnReadIni(string[] sections, string filePath)
        {
            return commINI(sections, filePath);
        }
        //INI parsing 
        private static DataSet commINI(string[] sections, string filePath)
        {
            string strBuf = "";
            try
            {
                // A file does not exist, create 
                if (!File.Exists(filePath))
                {
                    //FileStream sr = File.Create(filePath);
                    //sr.Close();
                }
                //Read INI file 
                System.IO.StreamReader stream = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
                strBuf = stream.ReadToEnd();
                stream.Close();
            }
            catch (Exception e)
            {
            }

            DataSet ds = new DataSet("config");


            int nSection = sections.Length;
            int index = 1;
            string[] rows = fnSplit(strBuf, "\r\n");
            string[] temp;
            string strRow;

            for (int i = 0; i < nSection; i++)
            {
                DataTable dt = new DataTable(sections[i]);
                dt.Columns.Add(new DataColumn("code", typeof(string)));
                dt.Columns.Add(new DataColumn("value", typeof(string)));


                for (; index < rows.Length; index++)
                {
                    DataRow dRow = dt.NewRow();
                    strRow = rows[index].Trim();
                    if (0 == strRow.Length)
                    {
                        continue;
                    }

                    // This is a comment 
                    if ("[" == strRow.Substring(0, 1))
                    {
                        index = index + 1;
                        break;
                    }
                    temp = fnSplit(strRow, "=");

                    dRow["code"] = temp[0].Trim();
                    dRow["value"] = temp[1].Trim();
                    dt.Rows.Add(dRow);
                }
                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary> 
        /// KeyMap 파일읽기
        /// <param name="filePath">file path</param> 
        /// <return>dataset</return>
        /// </summary> 
        public static DataTable fnReadKeyMap(string filePath)
        {
            return commKeyMap(filePath);
        }
        //dat parsing 
        private static DataTable commKeyMap(string filePath)
        {
            DataTable dt = new DataTable();
            DataRow dRow = null;

            dt.Columns.Add(new DataColumn("CdScan", typeof(string)));   //key Scan Code
            dt.Columns.Add(new DataColumn("CdFunc", typeof(string)));   //기능키 코드 값
            dt.Columns.Add(new DataColumn("NmFunc", typeof(string)));   //기능키 이름
            dt.Columns.Add(new DataColumn("FgDisp", typeof(string)));   //기능 버튼 화면 표시 구분
            dt.Columns.Add(new DataColumn("PoDisp", typeof(string)));   //화면 표시 위치
            dt.Columns.Add(new DataColumn("NmDisp", typeof(string)));   //기능 버튼 화면 표시명
            dt.Columns.Add(new DataColumn("ClDisp", typeof(string)));   //기능 버튼글씨 색갈
            dt.Columns.Add(new DataColumn("SqFunc", typeof(string)));   //CR-LF
            dt.Columns.Add(new DataColumn("NmDispEn", typeof(string)));   //add name key map En

            string[] rows = null;
            try
            {
                // A file does not exist, create 
                if (!File.Exists(filePath))
                {
                    FileStream sr = File.Create(filePath);
                    sr.Close();
                }

                rows = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);
            }
            catch (Exception e)
            {
            }

            string[] key;
            string rowData;

            for (int i = 0; i < rows.Length; i++)
            {
                rowData = rows[i].Trim();
                //Empty line 
                if (0 == rowData.Length)
                {
                    continue;
                }

                //This is a comment 
                if ("'" == rowData[0].ToString().Substring(0, 1))
                {
                    continue;
                }

                dRow = dt.NewRow();
                key = rowData.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                dRow["CdScan"] = fnTrim(key[0]);
                dRow["CdFunc"] = fnTrim(key[1]);
                dRow["NmFunc"] = fnTrim(key[2]);
                dRow["SqFunc"] = fnTrim(key[3]);
                dRow["FgDisp"] = fnTrim(key[4]);
                dRow["PoDisp"] = fnTrim(key[5]);
                dRow["NmDisp"] = fnTrim(key[6]);
                dRow["ClDisp"] = fnTrim(key[7]);
                dRow["NmDispEn"] = key.Length >= 9 ? fnTrim(key[8]) : dRow["NmDisp"];//add name key map En          
                dt.Rows.Add(dRow);
            }

            return dt;
        }


        //dat parsing 
        private static string commDatParse(string filePath)
        {
            string strBuf = "";

            if (!File.Exists(filePath))
            {
                FileStream sr = File.Create(filePath);
                sr.Close();
            }

            //Read file 
            System.IO.StreamReader stream = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
            strBuf = stream.ReadToEnd();
            stream.Close();

            return strBuf;
        }

        public static string commSqlParse(string filePath)
        {
            string strBuf = "";
            try
            {

                if (!File.Exists(filePath))
                {
                    FileStream sr = File.Create(filePath);
                    sr.Close();
                }

                //Read file 
                System.IO.StreamReader stream = new System.IO.StreamReader(filePath, System.Text.Encoding.UTF8);
                strBuf = stream.ReadToEnd();
                stream.Close();
            }
            catch (Exception ex)
            {
            }

            return strBuf;
        }

    }
}

