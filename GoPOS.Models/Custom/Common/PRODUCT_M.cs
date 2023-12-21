using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class PRODUCT_M
    {
        /// <summary>
        /// 매장-상품정보

        /// </summary>
        public PRODUCT_M()
        {
            /*
            SHOP_CODE            VARCHAR(6) NOT NULL,
            PRD_CODE             VARCHAR(26) NOT NULL,
            PRD_NAME             VARCHAR(50) NOT NULL,
            LCLASS_CODE          VARCHAR(4) NOT NULL,
            MCLASS_CODE          VARCHAR(6) NOT NULL,
            SCLASS_CODE          VARCHAR(8) NOT NULL,
            BRAND_CODE           VARCHAR(8) DEFAULT '*' NOT NULL,
            STYLE_CODE           VARCHAR(19) DEFAULT '*' NOT NULL,
            YEAR_CODE            VARCHAR(4) DEFAULT '*' NOT NULL,
            SEASON_CODE          VARCHAR(2) DEFAULT '*' NOT NULL,
            COLOR_CODE           VARCHAR(3) DEFAULT '*' NOT NULL,
            SIZE_CODE            VARCHAR(4) DEFAULT '*' NOT NULL,
            SIZE_CLASS_CODE      VARCHAR(2) DEFAULT '*' NOT NULL,
            MAP_PRD_CODE         VARCHAR(26),
            CORNER_CODE          VARCHAR(2) DEFAULT '00' NOT NULL,
            SPLY_UPRC            NUMERIC(10,2) DEFAULT 0 NOT NULL,
            TOGO_CHARGE          NUMERIC(10,2) DEFAULT 0 NOT NULL,
            DLV_CHARGE           NUMERIC(10,2) DEFAULT 0 NOT NULL,
            CST_ACCDC_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
            TAX_YN               VARCHAR(1) DEFAULT 'Y' NOT NULL,
            TIP_MENU_YN          VARCHAR(1) DEFAULT 'N' NOT NULL,
            ORD_PRD_YN           VARCHAR(1) DEFAULT 'Y' NOT NULL,
            SALE_PRD_YN          VARCHAR(1) DEFAULT 'Y' NOT NULL,
            STOCK_MGR_YN         VARCHAR(1) DEFAULT 'Y' NOT NULL,
            STOCK_OUT_YN         VARCHAR(1) DEFAULT 'N' NOT NULL,
            SIDE_MENU_YN         VARCHAR(1) DEFAULT 'N' NOT NULL,
            SDA_CLASS_CODE       VARCHAR(2),
            SDS_GROUP_CODE       VARCHAR(6),
            PRICE_MGR_FLAG       VARCHAR(1) DEFAULT '0' NOT NULL,
            SET_PRD_FLAG         VARCHAR(1) DEFAULT '0' NOT NULL,
            DEPOSIT_FLAG         VARCHAR(1) DEFAULT '0' NOT NULL,
            DEPOSIT_UPRC         NUMERIC(10,2) DEFAULT 0 NOT NULL,
            ORG_PLACE_CODE       VARCHAR(3),
            VENDOR_CODE          VARCHAR(10) DEFAULT '0' NOT NULL,
            ORD_UNIT_FLAG        VARCHAR(1) DEFAULT '0' NOT NULL,
            ORD_UNIT_QTY         NUMERIC(8,0) DEFAULT 1 NOT NULL,
            ORD_MIN_QTY          NUMERIC(8,0) DEFAULT 1 NOT NULL,
            PRICE_CTRL_FLAG      VARCHAR(1) DEFAULT 'S' NOT NULL,
            USE_YN               VARCHAR(1) DEFAULT 'N' NOT NULL,
            FLOOR_PRT_YN         VARCHAR(1) DEFAULT 'N' NOT NULL,
            INSERT_DT            VARCHAR(14),
            UPDATE_DT            VARCHAR(14),
            POS_PRT_YN           VARCHAR(1) DEFAULT 'N' NOT NULL,
            PRD_REMARK           VARCHAR(300),
            STAMP_ACC_YN         VARCHAR(1),
            STAMP_ACC_QTY        NUMERIC(5,0) DEFAULT 0 NOT NULL,
            STAMP_USE_YN         VARCHAR(1),
            STAMP_USE_QTY        NUMERIC(5,0) DEFAULT 0 NOT NULL,
            PRD_DC_FLAG          VARCHAR(1),
            KIOSK_IMG_URL        VARCHAR(200),
            KIOSK_IMG_FILE_NAME  VARCHAR(30),
            SOLD_OUT_YN          VARCHAR(1) DEFAULT 'N' NOT NULL,
            DC_RULE_FLAG         VARCHAR(10),
            DEPOSIT_TYPE         VARCHAR(1),
            DEPOSIT_PRD_CODE     VARCHAR(26),
            DEPOSIT_USE_YN       VARCHAR(1)

            public string SHOP_CODE           { get; set; } = string.Empty;
            public string PRD_CODE            { get; set; } = string.Empty;
            public string PRD_NAME            { get; set; } = string.Empty;
            public string LCLASS_CODE         { get; set; } = string.Empty;
            public string MCLASS_CODE         { get; set; } = string.Empty;
            public string SCLASS_CODE         { get; set; } = string.Empty;
            public string BRAND_CODE          { get; set; } = string.Empty;
            public string STYLE_CODE          { get; set; } = string.Empty;
            public string YEAR_CODE           { get; set; } = string.Empty;
            public string SEASON_CODE         { get; set; } = string.Empty;
            public string COLOR_CODE          { get; set; } = string.Empty;
            public string SIZE_CODE           { get; set; } = string.Empty;
            public string SIZE_CLASS_CODE     { get; set; } = string.Empty;
            public string MAP_PRD_CODE        { get; set; } = string.Empty;
            public string CORNER_CODE         { get; set; } = string.Empty;
            public string SPLY_UPRC           { get; set; } = string.Empty;
            public string TOGO_CHARGE         { get; set; } = string.Empty;
            public string DLV_CHARGE          { get; set; } = string.Empty;
            public string CST_ACCDC_YN        { get; set; } = string.Empty;
            public string TAX_YN              { get; set; } = string.Empty;
            public string TIP_MENU_YN         { get; set; } = string.Empty;
            public string ORD_PRD_YN          { get; set; } = string.Empty;
            public string SALE_PRD_YN         { get; set; } = string.Empty;
            public string STOCK_MGR_YN        { get; set; } = string.Empty;
            public string STOCK_OUT_YN        { get; set; } = string.Empty;
            public string SIDE_MENU_YN        { get; set; } = string.Empty;
            public string SDA_CLASS_CODE      { get; set; } = string.Empty;
            public string SDS_GROUP_CODE      { get; set; } = string.Empty;
            public string PRICE_MGR_FLAG      { get; set; } = string.Empty;
            public string SET_PRD_FLAG        { get; set; } = string.Empty;
            public string DEPOSIT_FLAG        { get; set; } = string.Empty;
            public string DEPOSIT_UPRC        { get; set; } = string.Empty;
            public string ORG_PLACE_CODE      { get; set; } = string.Empty;
            public string VENDOR_CODE         { get; set; } = string.Empty;
            public string ORD_UNIT_FLAG       { get; set; } = string.Empty;
            public string ORD_UNIT_QTY        { get; set; } = string.Empty;
            public string ORD_MIN_QTY         { get; set; } = string.Empty;
            public string PRICE_CTRL_FLAG     { get; set; } = string.Empty;
            public string USE_YN              { get; set; } = string.Empty;
            public string FLOOR_PRT_YN        { get; set; } = string.Empty;
            public string INSERT_DT           { get; set; } = string.Empty;
            public string UPDATE_DT           { get; set; } = string.Empty;
            public string POS_PRT_YN          { get; set; } = string.Empty;
            public string PRD_REMARK          { get; set; } = string.Empty;
            public string STAMP_ACC_YN        { get; set; } = string.Empty;
            public string STAMP_ACC_QTY       { get; set; } = string.Empty;
            public string STAMP_USE_YN        { get; set; } = string.Empty;
            public string STAMP_USE_QTY       { get; set; } = string.Empty;
            public string PRD_DC_FLAG         { get; set; } = string.Empty;
            public string KIOSK_IMG_URL       { get; set; } = string.Empty;
            public string KIOSK_IMG_FILE_NAME { get; set; } = string.Empty;
            public string SOLD_OUT_YN         { get; set; } = string.Empty;
            public string DC_RULE_FLAG        { get; set; } = string.Empty;
            public string DEPOSIT_TYPE        { get; set; } = string.Empty;
            public string DEPOSIT_PRD_CODE    { get; set; } = string.Empty;
            public string DEPOSIT_USE_YN      { get; set; } = string.Empty;
            */
        }

        public PRODUCT_M(string shop_code, string prd_code)
        {
            this.SHOP_CODE = shop_code;
            this.PRD_CODE = prd_code;
        }

        public string SHOP_CODE { get; set; } = string.Empty;
        public string PRD_CODE { get; set; } = string.Empty;
        public string PRD_NAME { get; set; } = string.Empty;
        public string LCLASS_CODE { get; set; } = string.Empty;
        public string MCLASS_CODE { get; set; } = string.Empty;
        public string SCLASS_CODE { get; set; } = string.Empty;
        public string BRAND_CODE { get; set; } = string.Empty;
        public string STYLE_CODE { get; set; } = string.Empty;
        public string YEAR_CODE { get; set; } = string.Empty;
        public string SEASON_CODE { get; set; } = string.Empty;
        public string COLOR_CODE { get; set; } = string.Empty;
        public string SIZE_CODE { get; set; } = string.Empty;
        public string SIZE_CLASS_CODE { get; set; } = string.Empty;
        public string MAP_PRD_CODE { get; set; } = string.Empty;
        public string CORNER_CODE { get; set; } = string.Empty;
        public string SPLY_UPRC { get; set; } = string.Empty;
        public string TOGO_CHARGE { get; set; } = string.Empty;
        public string DLV_CHARGE { get; set; } = string.Empty;
        public string CST_ACCDC_YN { get; set; } = string.Empty;
        public string TAX_YN { get; set; } = string.Empty;
        public string TIP_MENU_YN { get; set; } = string.Empty;
        public string ORD_PRD_YN { get; set; } = string.Empty;
        public string SALE_PRD_YN { get; set; } = string.Empty;
        public string STOCK_MGR_YN { get; set; } = string.Empty;
        public string STOCK_OUT_YN { get; set; } = string.Empty;
        public string SIDE_MENU_YN { get; set; } = string.Empty;
        public string SDA_CLASS_CODE { get; set; } = string.Empty;
        public string SDS_GROUP_CODE { get; set; } = string.Empty;
        public string PRICE_MGR_FLAG { get; set; } = string.Empty;
        public string SET_PRD_FLAG { get; set; } = string.Empty;
        public string DEPOSIT_FLAG { get; set; } = string.Empty;
        public string DEPOSIT_UPRC { get; set; } = string.Empty;
        public string ORG_PLACE_CODE { get; set; } = string.Empty;
        public string VENDOR_CODE { get; set; } = string.Empty;
        public string ORD_UNIT_FLAG { get; set; } = string.Empty;
        public string ORD_UNIT_QTY { get; set; } = string.Empty;
        public string ORD_MIN_QTY { get; set; } = string.Empty;
        public string PRICE_CTRL_FLAG { get; set; } = string.Empty;
        public string USE_YN { get; set; } = string.Empty;
        public string FLOOR_PRT_YN { get; set; } = string.Empty;
        public string INSERT_DT { get; set; } = string.Empty;
        public string UPDATE_DT { get; set; } = string.Empty;
        public string POS_PRT_YN { get; set; } = string.Empty;
        public string PRD_REMARK { get; set; } = string.Empty;
        public string STAMP_ACC_YN { get; set; } = string.Empty;
        public string STAMP_ACC_QTY { get; set; } = string.Empty;
        public string STAMP_USE_YN { get; set; } = string.Empty;
        public string STAMP_USE_QTY { get; set; } = string.Empty;
        public string PRD_DC_FLAG { get; set; } = string.Empty;
        public string KIOSK_IMG_URL { get; set; } = string.Empty;
        public string KIOSK_IMG_FILE_NAME { get; set; } = string.Empty;
        public string SOLD_OUT_YN { get; set; } = string.Empty;
        public string DC_RULE_FLAG { get; set; } = string.Empty;
        public string DEPOSIT_TYPE { get; set; } = string.Empty;
        public string DEPOSIT_PRD_CODE { get; set; } = string.Empty;
        public string DEPOSIT_USE_YN { get; set; } = string.Empty;

        // 추가 
        public string NO { get; set; } = string.Empty; // 리스트용 NO
        public string NORMAL_UPRC { get; set; } = string.Empty; // 리스트용 가격
    }
}
