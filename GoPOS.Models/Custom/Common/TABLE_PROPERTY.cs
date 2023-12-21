using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class TABLE_PROPERTY
    {
        /// <summary>
        /// 테이블 속성관리

        /// </summary>
        public TABLE_PROPERTY()
        {
            /*
            SHOP_CODE                    VARCHAR(6) NOT NULL,
            TABLE_FLAG                   VARCHAR(1) NOT NULL,
            PROPERTY_CODE                VARCHAR(2) NOT NULL,
            PROPERTY_NAME                VARCHAR(20) NOT NULL,
            X                            NUMERIC(3,0) NOT NULL,
            Y                            NUMERIC(3,0) NOT NULL,
            WIDTH                        NUMERIC(3,0) NOT NULL,
            HEIGHT                       NUMERIC(3,0) NOT NULL,
            TEXTALIGN_FLAG               VARCHAR(1) DEFAULT '1' NOT NULL,
            IMAGE_NAME                   VARCHAR(20),
            FONT                         VARCHAR(10) DEFAULT '굴림' NOT NULL,
            FONT_SIZE                    NUMERIC(2,0) DEFAULT 9 NOT NULL,
            FONT_STYLE_FLAG              VARCHAR(1) NOT NULL,
            FONT_COLOR                   VARCHAR(11) NOT NULL,
            USE_YN                       VARCHAR(1) DEFAULT 'Y' NOT NULL,
            INSERT_DT                    VARCHAR(14),
            UPDATE_DT                    VARCHAR(14)

            public string SHOP_CODE           { get; set; } = string.Empty;
            public string TABLE_FLAG          { get; set; } = string.Empty;
            public string PROPERTY_CODE       { get; set; } = string.Empty;
            public string PROPERTY_NAME       { get; set; } = string.Empty;
            public string X                   { get; set; } = string.Empty;
            public string Y                   { get; set; } = string.Empty;
            public string WIDTH               { get; set; } = string.Empty;
            public string HEIGHT              { get; set; } = string.Empty;
            public string TEXTALIGN_FLAG      { get; set; } = string.Empty;
            public string IMAGE_NAME          { get; set; } = string.Empty;
            public string FONT                { get; set; } = string.Empty;
            public string FONT_SIZE           { get; set; } = string.Empty;
            public string FONT_STYLE_FLAG     { get; set; } = string.Empty;
            public string FONT_COLOR          { get; set; } = string.Empty;
            public string USE_YN              { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            
            */
        }

        public TABLE_PROPERTY(string shop_code, string table_flag, string property_code)
        {
            this.SHOP_CODE = shop_code;
            this.TABLE_FLAG = table_flag;
            this.PROPERTY_CODE = property_code;           
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string TABLE_FLAG { get; set; } = string.Empty;
        public string PROPERTY_CODE { get; set; } = string.Empty;
        public string PROPERTY_NAME { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
        public string WIDTH { get; set; } = string.Empty;
        public string HEIGHT { get; set; } = string.Empty;
        public string TEXTALIGN_FLAG { get; set; } = string.Empty;
        public string IMAGE_NAME { get; set; } = string.Empty;
        public string FONT { get; set; } = string.Empty;
        public string FONT_SIZE { get; set; } = string.Empty;
        public string FONT_STYLE_FLAG { get; set; } = string.Empty;
        public string FONT_COLOR { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
    }
}
