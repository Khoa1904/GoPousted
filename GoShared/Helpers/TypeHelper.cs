using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Type = System.Type;

namespace GoShared.Helpers
{
    public enum ESaveType { None, List, Tran }
    public enum EEditType { None, Add, Edit, Remove }
    public static class TypeHelper
    {

        /// <summary>
        ///     String To Hex String : CP949
        /// </summary>
        /// <param name="strData">string</param>
        /// <returns>Hex String</returns>
        public static string StringToHexString(string strData)
        {
            if (string.IsNullOrEmpty(strData))
            {
                return string.Empty;
            }

            string resultHex = string.Empty;
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            byte[] bytes = Encoding.GetEncoding(51949).GetBytes(strData.TrimSafe());
            return bytes.Aggregate(resultHex, (current, byteStr) => current + $"{byteStr:X2}");
        }

        /// <summary>
        ///     Hex String To String : CP949
        /// </summary>
        /// <param name="hexString">Hex String</param>
        /// <returns>string</returns>
        public static string HexStringToString(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return string.Empty;
            }

            hexString = hexString.TrimSafe().Replace(Environment.NewLine, "");
            hexString = string.Join("", hexString.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(51949).GetString(bytes);
        }

        public static string Comma(object amount)
        {
            //string nullStr = amount.ToString();

            //if (nullStr == null || nullStr == "")
            //    return "";

            return string.Format("{0:N0}", Convert.ToDecimal(amount));
        }

        public static double CommaDel(string amount)
        {
            double result;
            amount = amount.Replace(",", "");

            if (!double.TryParse(amount, out result))
            {
                amount = "0";
            }

            return Convert.ToDouble(amount);
        }

        public static string CommaDelStr(string amount)
        {
            double result;
            amount = amount.Replace(",", "");

            if (!double.TryParse(amount, out result))
            {
                amount = "0";
            }

            return amount;
        }

        public static int ToInt32(this string sValue)
        {
            int result = 0;

            try
            {
                if (!int.TryParse(sValue, out result))
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToInt32(result);
                }
            }
            catch (Exception)
            {
                result = 0;
            }

            return result;
        }

        public static int ParseInt32(this string sValue)
        {
            var s = new string(sValue.Where(c => char.IsDigit(c)).ToArray());
            return string.IsNullOrEmpty(s) ? 0 : int.Parse(s);
        }

        public static Int16 ToInt16(this string sValue)
        {
            Int16 result = 0;

            try
            {
                if (!Int16.TryParse(sValue, out result))
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToInt16(result);
                }
            }
            catch (Exception)
            {
                result = 0;
            }

            return result;
        }

        public static string ChgDataTime(string dt)
        {
            //20230331172931 to 2023-03-31 17:29:31

            try
            {
                string inputDate = dt;
                DateTime resultDate = DateTime.ParseExact(inputDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                string outputDate = resultDate.ToString("yyyy-MM-dd HH:mm:ss");

                return outputDate;
            }
            catch
            {
                return dt;
            }
        }

        public static string FormatDateString(this string dateString)
        {
            return dateString.Substring(0, 4) + "." + dateString.Substring(4, 2) + "." + dateString.Substring(6);
        }

        public static string FormatTimeString(this string dateTimeString)
        {
            var dt = DateTime.ParseExact((dateTimeString.Length > 6 ? string.Empty :
                        DateTime.Today.ToString("yyyyMMdd")) + dateTimeString, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentUICulture);
            return string.Format("{0:HH:mm:ss}", dt);
        }

        public static string FormatDateTimeString(this string dateTimeString)
        {
            var dt = DateTime.ParseExact(dateTimeString, "yyyyMMddHHmmss", Thread.CurrentThread.CurrentUICulture);
            return string.Format("{0:yyyy.MM.dd HH:mm:ss}", dt);
        }
        public static string ChgData(string dt)
        {
            try
            {
                //20230331 to 2023-03-31
                string inputDate = dt;
                DateTime resultDate = DateTime.ParseExact(inputDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                string outputDate = resultDate.ToString("yyyy-MM-dd");

                return outputDate;
            }
            catch
            {
                return dt;
            }
        }
        public static decimal ToDecimal(this string amount)
        {
            decimal result;
            amount = amount.Replace(",", "");

            if (!decimal.TryParse(amount, out result))
            {
                amount = "0";
            }

            return Convert.ToDecimal(amount);
        }
        public static Double ToDouble(this string value)
        {
            decimal result;
            value = value.Replace(",", "");

            if (!decimal.TryParse(value, out result))
            {
                value = "0";
            }

            return Convert.ToDouble(value);
        }
        public static decimal AmtTax(this decimal amt, double percentage = 10)
        {
            var dNoTaxAmt = Convert.ToDouble(amt) / (1 + percentage / 100);
            int nNoTaxAmt = (int)Math.Round(dNoTaxAmt);
            decimal nTaxAmt = amt - (decimal)nNoTaxAmt;
            return (int)Math.Round(nTaxAmt);
        }

        public static decimal AmtNoTax(this decimal amt, double percentage = 10)
        {
            var dNoTaxAmt = Convert.ToDouble(amt) / (1 + percentage / 100);
            int nNoTaxAmt = (int)Math.Round(dNoTaxAmt);
            return nNoTaxAmt;
        }

        public static string StrIntInc(this string str, int length)
        {
            return (Convert.ToInt32(str) + 1).ToString("d" + length.ToString());
        }

        public static bool CheckCollectionType(this Type type)
        {
            return type.GetInterfaces().Where(s => s.Name == "IEnumerable").Count() > 0;
        }

        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static T GetValueInDic<T>(this Dictionary<string, T> dic, string key) where T : class
        {
            if (dic.ContainsKey(key))
            {
                return dic[key] as T;
            }
            else
            {
                return null;
            }
        }

        #region Exceptions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string ToFormattedString(this Exception exception)
        {
            IEnumerable<string> messages = exception
                .GetAllExceptions()
                .Where(e => !String.IsNullOrWhiteSpace(e.Message))
                .Select(e => (e.InnerException?.Message + " " + e.Message).Trim());
            string flattened = String.Join(Environment.NewLine, messages); // <-- the separator here
            return flattened;
        }

        public static IEnumerable<Exception> GetAllExceptions(this Exception exception)
        {
            yield return exception;

            if (exception is AggregateException aggrEx)
            {
                foreach (Exception innerEx in aggrEx.InnerExceptions.SelectMany(e => e.GetAllExceptions()))
                {
                    yield return innerEx;
                }
            }
            else if (exception.InnerException != null)
            {
                foreach (Exception innerEx in exception.InnerException.GetAllExceptions())
                {
                    yield return innerEx;
                }
            }
        }

        // Return 0 if a character is in Hangul Jamo block, -1 otherwise
        public static bool IsHangulChar(Char c)
        {
            if ((int)c >= 4352 && (int)c <= 4607)
            {
                return true;
            }
            return false;
        }

        public static bool IsKoreanChar(string sChar)
        {
            Regex strRegex = new Regex(@"(\p{L}+?)(?:로|길)\s*(\d+)");// new Regex(@"[가-힣]");
            return strRegex.IsMatch(sChar);
        }

        public static bool IsNumeric(string sChar)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return regex.IsMatch(sChar);
        }

        // Return 0 if all characters are in Hangul Jamo block, -1 otherwise
        public static bool IsHangulText(string koreanTex)
        {
            for (var i = 0; i < koreanTex.Length; ++i)
            {
                if (IsHangulChar(koreanTex[i]))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Enc & Dec

        private static string EncryptDes(string source, string key)
        {
            using TripleDES tripleDesCryptoService = TripleDES.Create();
            using MD5 hashMd5Provider = MD5.Create();
            byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            tripleDesCryptoService.Key = byteHash;
            tripleDesCryptoService.Mode = CipherMode.ECB;
            byte[] data = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(tripleDesCryptoService.CreateEncryptor().TransformFinalBlock(data, 0, data.Length));
        }

        private static string DecryptDes(string encrypt, string key)
        {
            using TripleDES tripleDesCryptoService = TripleDES.Create();
            using MD5 hashMd5Provider = MD5.Create();
            byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
            tripleDesCryptoService.Key = byteHash;
            tripleDesCryptoService.Mode = CipherMode.ECB;
            byte[] data = Convert.FromBase64String(encrypt);
            return Encoding.UTF8.GetString(tripleDesCryptoService.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
        }

        public static string EncryptString(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : EncryptDes(str, " ");
        }

        public static string DecryptString(string str)
        {
            return string.IsNullOrWhiteSpace(str) ? "" : DecryptDes(str, " ");
        }


        #endregion

        #region Amount rounding


        public static decimal RoundUp(decimal toRound)
        {
            if (toRound % 10 == 0) return toRound;
            return (10 - toRound % 10) + toRound;
        }

        public static decimal RoundNear(decimal toRound, int roundTo)
        {
            var near = toRound % roundTo;
            return near > roundTo / 2 ? (roundTo - toRound % roundTo) + toRound : toRound - toRound % roundTo;
        }

        public static decimal RoundNear10(decimal toRound)
        {
            return RoundNear(toRound, 10);
        }

        #endregion

    }
}
