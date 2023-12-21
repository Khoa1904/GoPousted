using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    internal class ClassGenerator
    {

        internal static void Generate()
        {
            var dbConn = new FbConnection(Firebird());
            FbDataAdapter fbDataAdapter = new FbDataAdapter(@"select f.rdb$relation_name AS TABLE_NAME, 
    f.rdb$field_name AS FIELD_NAME, 
    f.RDB$DESCRIPTION AS COMMENT, 
    case t.RDB$FIELD_TYPE
        when 7 then 'Int16'
        when 8 then 'int'
        when 10 then 'float'
        when 12 then 'DateTime'
        when 13 then 'Timestamp'
        when 14 then 'string'
        when 16 then 'decimal'
        when 27 then 'double'
        when 35 then 'Timestamp'
        when 37 then 'string?'
        when 261 then 'object'
    end                                  FIELD_TYPE,
    t.RDB$CHARACTER_LENGTH               FIELD_LEN,
    t.RDB$FIELD_SCALE AS FIELD_SCALE,
    t.RDB$FIELD_PRECISION AS FIELD_PREC,
    case when keys.field_name is null
        then cast(0 as int) 
        else cast(1 as int) end as KEY,
    keys.key_pos
from rdb$relation_fields f
join rdb$relations r on f.rdb$relation_name = r.rdb$relation_name
and r.rdb$view_blr is null
and (r.rdb$system_flag is null or r.rdb$system_flag = 0)
JOIN RDB$FIELDS t on t.RDB$FIELD_NAME = f.RDB$FIELD_SOURCE
left join (
    select
    rc.rdb$relation_name as table_name,
    sg.rdb$field_name as field_name,
    sg.""RDB$FIELD_POSITION"" AS key_pos
from
    rdb$indices ix
    left join rdb$index_segments sg on ix.rdb$index_name = sg.rdb$index_name
    left join rdb$relation_constraints rc on rc.rdb$index_name = ix.rdb$index_name
where
    rc.rdb$constraint_type = 'PRIMARY KEY') as keys on 
        f.RDB$RELATION_NAME = keys.TABLE_NAME
        AND F.RDB$FIELD_NAME = keys.FIELD_NAME
order by 1, f.RDB$RELATION_NAME, f.rdb$field_position, keys.key_pos;", dbConn);

            var dsFields = new DataSet();
            fbDataAdapter.Fill(dsFields);

            string fieldTemplate = @"/// <summary>
	/// #COMMENT
	/// </summary>
	[Comment(""#COMMENT"")]#PRECISION#KEY
	public #FIELD_TYPE #FIELD_NAME { get; set; }#DEF_VALUE";

            string tableTemplate = @"using GoShared.Helpers;
using GoShared.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GoPOS.Models;

[Table(""#TABLE_NAME"")]
public class #TABLE_NAME : IIdentifyEntity
{
#FIELDS_LIST
#TABpublic string Base_PrimaryName()
#TAB{
#TAB#TABreturn #NAMES_PK;
#TAB}

#TABpublic void EntityDefValue(EEditType eEdit, string userID, object data)
#TAB{
#TAB}
    
#TABpublic string Resource()
#TAB{
#TAB#TABthrow new NotImplementedException();
#TAB}    
}";

            var tableName = string.Empty;
            StringBuilder sb = new StringBuilder();
            string sKeyNames = string.Empty;
            for (int i = 0; i < dsFields.Tables[0].Rows.Count; i++)
            {
                var dr = dsFields.Tables[0].Rows[i];
                var tbn = dr["TABLE_NAME"].ToString().Trim();
                var fn = dr["FIELD_NAME"].ToString().Trim();

                var cm = dr["COMMENT"].ToString().Trim();
                var ft = dr["FIELD_TYPE"].ToString().Trim();
                var fs = Convert.ToInt32(dr["FIELD_SCALE"].ToString());
                var fp = dr["FIELD_PREC"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FIELD_PREC"].ToString());
                var nkey = Convert.ToInt32(dr["KEY"].ToString());
                var kp = dr["KEY_POS"] == DBNull.Value ? 0 : Convert.ToInt32(dr["KEY_POS"].ToString());

                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = tbn;
                }

                if (tableName != tbn)
                {
                    string tbClass = tableTemplate;
                    tbClass = tbClass.Replace("#TABLE_NAME", tableName);
                    tbClass = tbClass.Replace("#FIELDS_LIST", sb.ToString());
                    tbClass = tbClass.Replace("#NAMES_PK", "\"" + (string.IsNullOrEmpty(sKeyNames) ? string.Empty : sKeyNames.Substring(0, sKeyNames.Length - 1)) + "\"");
                    tbClass = tbClass.Replace("#TAB", "\t");

                    sb = new StringBuilder();
                    sKeyNames = string.Empty;

                    SaveClassFile(tableName, tbClass);
                    tableName = tbn;
                }

                string fText = "\t" + fieldTemplate;
                fText = fText.Replace("#FIELD_NAME", fn);
                fText = fText.Replace("#DEF_VALUE", fn.Contains("CNT") || fn.Contains("AMT") ? 
                                    " = 0;" : String.Empty);

                fText = fText.Replace("#COMMENT", cm);
                fText = fText.Replace("#FIELD_TYPE", ft);
                if (ft == "decimal")
                {
                    fText = fText.Replace("#PRECISION",
                        Environment.NewLine + String.Format("\t[Precision({0}, {1})]", fp, Math.Abs(fs)));
                }
                else
                {
                    fText = fText.Replace("#PRECISION", string.Empty);
                }

                string sIsKey = nkey == 1 ? 
                    string.Format("\r\n\t[Key, Column(Order = {0})]\r\n\t[Required]", kp + 1) : "";
                fText = fText.Replace("#KEY", sIsKey);
                
                if (nkey == 1)
                {
                    sKeyNames += fn;
                    sKeyNames += "_";
                }

                sb.AppendLine(fText);
            }
        }

        static void SaveClassFile(string tableName, string classText)
        {
            var dirs = Environment.CurrentDirectory + @"\" + tableName.Substring(0, tableName.IndexOf("_"));
            if (!Directory.Exists(dirs))
            {
                Directory.CreateDirectory(dirs);
            }

            var fileName = Path.Combine(dirs, tableName + ".cs");
            File.WriteAllText(fileName, classText, Encoding.UTF8);
        }

        static string Firebird()
        {
            string directory = string.Empty + Directory.GetParent(Environment.CurrentDirectory);
            FbConnectionStringBuilder connectionBuilder = new()
            {
                ServerType = FbServerType.Embedded,
                ClientLibrary = directory + @"\libs\Firebird4\fbclient.dll",
                Database = directory + @"\data\GoPOS.FDB",
                UserID = "SYSDBA",
                Password = "masterkey",
                Charset = FbCharset.Utf8.ToString()
            };

            return connectionBuilder.ToString();
        }

        internal static void AddImages(string csprojFile)
        {
            /*
             * 
             * <ItemGroup>
    <Content Include="..\Resource\Images\SalesMngMain.png" Link="Resource\Images\SalesMngMain.png" />
  </ItemGroup>	
             * */
            string resFolder = Directory.GetParent(Environment.CurrentDirectory) + "\\resource";
            var files = Directory.GetFiles(resFolder, "*", SearchOption.AllDirectories);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  <ItemGroup>");
            foreach ( var file in files )
            {
                sb.AppendFormat("    <Content Include=\"..\\Resource\\{0}\" Link=\"Resource\\{0}\" />",
                    file.Remove(0, resFolder.Length));
                sb.AppendLine();
            }
            sb.AppendLine("  </ItemGroup>");

            var lines = File.ReadAllLines(csprojFile);
            var list = lines.ToList();
            list.Insert(lines.Length - 1, sb.ToString());
            File.WriteAllLines(csprojFile, list);
        }

        internal static void ChangeResxPath(string solFolder)
        {
            // find all xaml files
            var files = Directory.GetFiles(solFolder, "*.xaml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var contents = File.ReadAllText(file);
                int idx = 0;
                while (idx >= 0)
                {

                }
            }

            Console.WriteLine("DONE");
        }
    }

}
