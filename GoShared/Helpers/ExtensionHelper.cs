using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace GoShared.Helpers;
public static class ExtensionHelper
{
    public static string RemoveNewline(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? "" : source.Replace("\n", " ").Replace("\r", " ");
    }

    public static string RemoveDoubleSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? "" : Regex.Replace(source, @"\s+", $"{(char)32}");
    }

    public static string ReplacePlainText(this string source)
    {
        return source.RemoveNewline().RemoveDoubleSpace();
    }

    public static string Left(this string source, int length)
    {
        if (source.TrimSafe().Length < length)
        {
            return string.Empty;
        }

        return source[..length];
    }

    public static async void AWait(this Task task, Action completedCallback, Action<Exception> errorCallback)
    {
        try
        {
            await task;
            completedCallback?.Invoke();
        }
        catch (Exception exception)
        {
            errorCallback?.Invoke(exception);
        }
    }

    public static string TrimSafe(this string source)
    {
        return string.IsNullOrWhiteSpace(source) ? string.Empty : source.Trim();
    }

    public static string LogEncrypt(this string message)
    {
        return "DecryptLog : " + message;
//#if DEBUG
        
//#else
//        return "EncryptLog : " + WindowHelper.EncryptString(message);
//#endif
    }

    public static bool IsGenericList(this object o)
    {
        var oType = o.GetType();
        return oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>));
    }

    public static string GetCSharpType(this Type dotNetType, bool isNull = false)
    {
        string nullable = isNull ? "?" : "";
        string prefix = "System.";
        string typeName = dotNetType.ToString().StartsWith(prefix) ? dotNetType.ToString().Remove(0, prefix.Length) : dotNetType.ToString();
        string cstype = typeName switch
        {
            "Boolean" => "bool",
            "Byte" => "byte",
            "SByte" => "sbyte",
            "Char" => "char",
            "Decimal" => "double",
            "Double" => "double",
            "Single" => "float",
            "Int32" => "int",
            "UInt32" => "uint",
            "Int64" => "long",
            "UInt64" => "ulong",
            "Object" => "object",
            "Int16" => "short",
            "UInt16" => "ushort",
            "String" => "string",
            _ => typeName,
        };
        return $"{cstype}{nullable}";
    }

    /// <summary>
    ///     Dispose List
    ///     _list.OfType<IDisposable>().ForIn(x => x.Dispose());
    /// </summary>
    /// <param name="seq">IEnumerable</param>
    /// <param name="act">Action</param>
    /// <typeparam name="T">IDisposable</typeparam>
    public static void ForIn<T>(this IEnumerable<T> seq, Action<T> act) where T : IDisposable
    {
        foreach (var item in seq)
        {
            act(item);
        }
    }

    public static T CopyFields<T>(this object fromObj)
    {
        return CopyFields<T>(fromObj, null);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fromObj"></param>
    /// <returns></returns>
    public static T CopyFields<T>(this object fromObj, Dictionary<string, string> mapFields)
    {
        var allTypes = fromObj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var nt = Activator.CreateInstance<T>();
        var ntt = nt.GetType();
        foreach (var tp in allTypes)
        {
            var val = tp.GetValue(fromObj);

            var np = ntt.GetProperty(tp.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (np == null && mapFields != null)
            {
                if (mapFields.ContainsKey(tp.Name))
                {
                    np = ntt.GetProperty(mapFields[tp.Name], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }
            }
            if (np == null) continue;
            try
            {
                np.SetValue(nt, val);
            }
            catch
            {
            }
        }

        return nt;
    }

    public static T CopyFieldsFrom<T>(this T toObj, object fromObj, Dictionary<string, string> mapFields)
    {
        if (toObj == null || fromObj == null)
        {
            return toObj;
        }
        var allTypes = fromObj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var ntt = toObj.GetType();
        foreach (var tp in allTypes)
        {
            var val = tp.GetValue(fromObj);

            var np = ntt.GetProperty(tp.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (np == null && mapFields != null)
            {
                if (mapFields.ContainsKey(tp.Name))
                {
                    np = ntt.GetProperty(mapFields[tp.Name], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }
            }

            if (np == null) continue;
            try
            {
                np.SetValue(toObj, val);
            }
            catch (Exception ex)
            {
            }
        }

        return toObj;
    }

    public static T AddFieldsFrom<T>(this T toObj, object fromObj, Dictionary<string, string> mapFields)
    {
        if (toObj == null || fromObj == null)
        {
            return toObj;
        }
        var allTypes = fromObj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        var ntt = toObj.GetType();
        foreach (var tp in allTypes)
        {
            var val = tp.GetValue(fromObj);

            var np = ntt.GetProperty(tp.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (np == null && mapFields != null)
            {
                if (mapFields.ContainsKey(tp.Name))
                {
                    np = ntt.GetProperty(mapFields[tp.Name], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }
            }

            if (np == null) continue;
            try
            {
                var cValue = np.GetValue(toObj);
                if (cValue != null && cValue.IsNumericType())
                {
                    decimal cv = Convert.ToDecimal(cValue);
                    decimal dv = Convert.ToDecimal(val) + cv;

                    string tName = np.PropertyType.Name;
                    if (tName.Contains("short", StringComparison.OrdinalIgnoreCase) ||
                        tName.Contains("int16", StringComparison.OrdinalIgnoreCase))
                    {
                        np.SetValue(toObj, Convert.ToInt32(dv));
                    }
                    else if (tName.Contains("int", StringComparison.OrdinalIgnoreCase))
                    {
                        np.SetValue(toObj, Convert.ToInt16(dv));
                    }
                    else
                    {
                        np.SetValue(toObj, dv);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex.ToFormattedString());
            }
        }

        return toObj;
    }

    public static T[] CopyToEachItems<T>(this ObservableCollection<T> fromList)
    {
        var toArray = new T[fromList.Count];
        for (int i = 0; i < fromList.Count; i++)
        {
            toArray[i] = fromList[i].ObjectCopy<T>();
        }

        return toArray;
    }

    public static T ObjectCopy<T>(this object fromObj)
    {
        var toObj = Activator.CreateInstance<T>();
        var allTypes = fromObj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var tp in allTypes)
        {
            var val = tp.GetValue(fromObj);
            var np = toObj.GetType().GetProperty(tp.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            try
            {
                np.SetValue(toObj, val);
            }
            catch { }
        }

        return toObj;
    }
}

public static class PagingUtils
{
    public static IEnumerable<T> Page<T>(this IEnumerable<T> en, int pageSize, int page)
    {
        return en.Skip(page * pageSize).Take(pageSize);
    }
    public static IQueryable<T> Page<T>(this IQueryable<T> en, int pageSize, int page)
    {
        return en.Skip(page * pageSize).Take(pageSize);
    }
}
