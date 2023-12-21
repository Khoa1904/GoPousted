using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{

    public class LSystem
    {
        public const string CSuccess = "999";
        public const string CSystemError = "9";

        public const string CNotSupport = "1";
        public static string LNotSupport = "No Support.";

        public const string CRequestNull = "2";
        public static string LRequestNull = "data is null.";

        public const string CDataNull = "3";
        public static string LDataNull = "data is null.";

        public const string CFindItemUpdateFail = "4";
        public static string LFindItemUpdateFail = "Can not find item update.";
        public static string LFindItemRemoveFail = "Can not find item remote.";

        public const string CItemNotFound = "5";
        public static string LItemNotFound = "Can not find item.";

        public const string CPageIndexInValid = "6";
        public static string LPageIndexInValid = "PageIndex InValid.";

        public const string CDetailNull = "7";
        public static string LDetailNull = "data is null.";

        public static string LInsertSuccess = "Insert success.";
        public static string LUpdateSuccess = "Update success.";
        public static string LRemoveSuccess = "Remote success.";
    }
}
