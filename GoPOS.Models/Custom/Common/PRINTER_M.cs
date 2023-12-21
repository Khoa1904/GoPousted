using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    /// <summary>
    /// 주방프린터관리

    /// </summary>
    public class PRINTER_M
    {
        public PRINTER_M()
        {
            /*
            CREATE TABLE PRINTER_M (
            SHOP_CODE      VARCHAR(6) NOT NULL,
            PRT_NO         VARCHAR(2) NOT NULL,
            PRT_NAME       VARCHAR(10),
            POS_NO         VARCHAR(2) NOT NULL,
            PRT_TYPE_FLAG  VARCHAR(2) NOT NULL,
            PRT_PORT       VARCHAR(2) NOT NULL,
            PRT_SPEED      VARCHAR(1) NOT NULL,
            USE_YN         VARCHAR(1) NOT NULL,
            INSERT_DT      VARCHAR(14),
            UPDATE_DT      VARCHAR(14),
            PRT_PAPER_QTY  NUMERIC(1,0) DEFAULT 1 NOT NULL,
            FLOOR_NO       VARCHAR(2),
            FLOOR_FLAG     VARCHAR(1),
            PRT_TCP_IP     VARCHAR(15),
            PRT_TCP_PORT   VARCHAR(5),
            PRT_BELL_YN    VARCHAR(1)


            public string SHOP_CODE     { get; set; } = string.Empty;
            public string PRT_NO        { get; set; } = string.Empty;
            public string PRT_NAME      { get; set; } = string.Empty;
            public string POS_NO        { get; set; } = string.Empty;
            public string PRT_TYPE_FLAG { get; set; } = string.Empty;
            public string PRT_PORT      { get; set; } = string.Empty;
            public string PRT_SPEED     { get; set; } = string.Empty;
            public string USE_YN        { get; set; } = string.Empty;
            public string INSERT_DT     { get; set; } = string.Empty;
            public string UPDATE_DT     { get; set; } = string.Empty;
            public string PRT_PAPER_QTY { get; set; } = string.Empty;
            public string FLOOR_NO      { get; set; } = string.Empty;
            public string FLOOR_FLAG    { get; set; } = string.Empty;
            public string PRT_TCP_IP    { get; set; } = string.Empty;
            public string PRT_TCP_PORT  { get; set; } = string.Empty;
            public string PRT_BELL_YN   { get; set; } = string.Empty;

            */
        }

        public PRINTER_M(string shop_code, string prt_no)
        {
            this.SHOP_CODE = shop_code;
            this.PRT_NO = prt_no;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string PRT_NO { get; set; } = string.Empty;
        public string PRT_NAME { get; set; } = string.Empty;
        public string POS_NO { get; set; } = string.Empty;
        public string PRT_TYPE_FLAG { get; set; } = string.Empty;
        public string PRT_PORT { get; set; } = string.Empty;
        public string PRT_SPEED { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
        public string PRT_PAPER_QTY { get; set; } = string.Empty;
        public string FLOOR_NO { get; set; } = string.Empty;
        public string FLOOR_FLAG { get; set; } = string.Empty;
        public string PRT_TCP_IP { get; set; } = string.Empty;
        public string PRT_TCP_PORT { get; set; } = string.Empty;
        public string PRT_BELL_YN { get; set; } = string.Empty;
    }
}
