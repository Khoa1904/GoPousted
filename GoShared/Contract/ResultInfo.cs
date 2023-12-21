using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoShared.Contract
{
    public partial class ResultInfo
    {
        public Guid RequestKeyID { get; set; }
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";
        public string Code { get; set; } = "";
        public int Record { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
        public dynamic Data { get; set; }
        public dynamic Info { get; set; }
        public string TSql { get; set; }
        //public int FrameNumber { get; set; }
        public UInt16 FunctionCode { get; set; }
        public ResultInfo()
        {
            Success = false;
        }
        public ResultInfo(string code, string mesage)
        {
            Success = false;
            Code = code;
            Message = "[" + code + "] " + mesage;
        }
        public ResultInfo(bool status, string code, string mesage)
        {
            Success = status;
            Code = code;
            Message = mesage;
        }
        public void Map(ResultInfo info)
        {
            Record += info.Record;
            Message += info.Message;
            Success = info.Success && Success;
            Record = info.Record;
        }
    }
}
