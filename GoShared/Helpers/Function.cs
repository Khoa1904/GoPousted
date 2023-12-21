using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS
{
    public static class Function
    {
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

        public static int ChgNum(string sValue)
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
        public static DateTime? S2Date(this string dt, string format = "yyyyMMdd")
        {
            try
            {
                //20230331 to 2023-03-31
                string inputDate = dt;
                DateTime resultDate = DateTime.ParseExact(inputDate, format, System.Globalization.CultureInfo.InvariantCulture);
                return resultDate;
            }
            catch
            {
                return null;
            }
        }

        public static int ChangeToNumber(string numberString)
        {
            try
            {
                if (string.IsNullOrEmpty(numberString))
                    return 0;
                numberString = numberString.Replace(",", ""); // Remove the comma
                int number = int.Parse(numberString);
                return number;

            }
            catch (Exception ex)
            {
                return 0;
            }
     
        }
    }

}
