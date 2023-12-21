using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Models
{
    public class ORDER_LIST
    {
        /// <summary>
        /// GetOrderList 주문 - 오더 리스트

        /// </summary>
        public ORDER_LIST()
        {
            /*
CREATE PROCEDURE SP_WAITING_GET_ODDTL(
    SHOP_CODE VARCHAR(6),
    SALE_DATE VARCHAR(8),
    ORDER_NO VARCHAR(4))
RETURNS (
    public string PRD_CODE         { get; set; } = string.Empty; //VARCHAR(26),
    public string PRD_NAME         { get; set; } = string.Empty; //VARCHAR(50),
    public string ORDER_SEQ_NO     { get; set; } = string.Empty; //VARCHAR(4),
    public string NORMAL_UPRC      { get; set; } = string.Empty; //NUMERIC(10,2),
    public string SALE_QTY         { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT           { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DCM_SALE_AMT     { get; set; } = string.Empty; //NUMERIC(12,2),
    public string SALE_MG_CODE     { get; set; } = string.Empty; //VARCHAR(4),
    public string SALE_MG_NAME     { get; set; } = string.Empty; //VARCHAR(30),
    public string DC_TYPE_FLAG     { get; set; } = string.Empty; //VARCHAR(1),
    public string DLV_PACK_FLAG    { get; set; } = string.Empty; //VARCHAR(1),
    public string TAX_YN           { get; set; } = string.Empty; //VARCHAR(1),
    public string CORNER_CODE      { get; set; } = string.Empty; //VARCHAR(2),
    public string ORG_SALE_MG_CODE { get; set; } = string.Empty; //VARCHAR(4),
    public string TIP_MENU_YN      { get; set; } = string.Empty; //VARCHAR(1),
    public string SVC_TIP_AMT      { get; set; } = string.Empty; //NUMERIC(12,2),
    public string VAT_AMT          { get; set; } = string.Empty; //NUMERIC(12,2),
    public string SALE_UPRC        { get; set; } = string.Empty; //NUMERIC(10,2),
    public string BRAND_CODE       { get; set; } = string.Empty; //VARCHAR(8),
    public string PRD_TYPE_FLAG    { get; set; } = string.Empty; //VARCHAR(1),
    public string DC_AMT_GEN       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_SVC       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_JCD       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_CPN       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_CST       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_FOD       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_CRD       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_PRM       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_PACK      { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_LYT       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string DC_AMT_TAX       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string CST_SALE_POINT   { get; set; } = string.Empty; //NUMERIC(12,4),
    public string CST_USE_POINT    { get; set; } = string.Empty; //NUMERIC(12,4),
    public string SDA_CODE         { get; set; } = string.Empty; //VARCHAR(10),
    public string SDS_ORG_DTL_NO   { get; set; } = string.Empty; //VARCHAR(4),
    public string ORG_SALE_UPRC    { get; set; } = string.Empty; //NUMERIC(10,2),
    public string ETC_AMT          { get; set; } = string.Empty; //NUMERIC(12,2),
    public string SVC_CODE         { get; set; } = string.Empty; //VARCHAR(1),
    public string TK_CPN_CODE      { get; set; } = string.Empty; //VARCHAR(3),
    public string DC_SEQ_TYPE      { get; set; } = string.Empty; //VARCHAR(50),
    public string DC_SEQ_AMT       { get; set; } = string.Empty; //VARCHAR(500),
    public string DC_SEQ_FLAG      { get; set; } = string.Empty; //VARCHAR(50),
    public string DC_SEQ_RATE      { get; set; } = string.Empty; //VARCHAR(500),
    public string REMARK           { get; set; } = string.Empty; //VARCHAR(30),
    public string SIDE_MENU_YN     { get; set; } = string.Empty; //VARCHAR(1),
    public string TOGO_CHARGE      { get; set; } = string.Empty; //NUMERIC(10,2),
    public string AFFILIATE_UPRC   { get; set; } = string.Empty; //NUMERIC(12,2),
    public string MCP_BAR_CODE     { get; set; } = string.Empty; //VARCHAR(40),
    public string STAMP_SALE_QTY   { get; set; } = string.Empty; //NUMERIC(9,0),
    public string STAMP_USE_YN     { get; set; } = string.Empty; //VARCHAR(1),
    public string STAMP_USE_QTY    { get; set; } = string.Empty; //NUMERIC(9,0),
    public string SDS_CLASS_CODE   { get; set; } = string.Empty; //VARCHAR(6),
    public string DOUBLE_CODE      { get; set; } = string.Empty; //VARCHAR(2),
    public string DOUBLE_AMT       { get; set; } = string.Empty; //NUMERIC(12,2),
    public string PRD_WEIGHT       { get; set; } = string.Empty; //NUMERIC(8,0),
    public string TAX_RFND_AMT     { get; set; } = string.Empty; //NUMERIC(12,2),
    public string TAX_RFND_FEE     { get; set; } = string.Empty; //NUMERIC(12,2),
    public string PRD_DC_FLAG      { get; set; } = string.Empty; //VARCHAR(1),
    public string IF_CPN_CODE      { get; set; } = string.Empty; //VARCHAR(20),
    public string DC_REASON_CODE   { get; set; } = string.Empty; //VARCHAR(3),
    public string DC_REASON_NAME   { get; set; } = string.Empty; //VARCHAR(200),
    public string SDS_PARENT_CODE  { get; set; } = string.Empty; //VARCHAR(26),
    public string DC_RULE_FLAG     { get; set; } = string.Empty; //VARCHAR(10),
    public string DEPOSIT_USE_YN   { get; set; } = string.Empty; //VARCHAR(1),
    public string DEPOSIT_TYPE     { get; set; } = string.Empty; //VARCHAR(1))
AS

            */
        }

        public ORDER_LIST(string shop_code, string sale_date, string order_no)
        {
            this.SHOP_CODE = shop_code;
            this.SALE_DATE = sale_date;
            this.ORDER_NO = order_no;
        }

        public string NO { get; set; } = string.Empty; //VARCHAR(26),
        public string SHOP_CODE { get; set; } = string.Empty; //VARCHAR(26),
        public string SALE_DATE { get; set; } = string.Empty; //VARCHAR(26),
        public string ORDER_NO { get; set; } = string.Empty; //VARCHAR(26),
        public string ORDER_DTL_NO { get; set; } = string.Empty; //VARCHAR(4),  // 김형석 추가 20230219 ORDER_DTL_NO
        public string PRD_CODE { get; set; } = string.Empty; //VARCHAR(26),
        public string PRD_NAME { get; set; } = string.Empty; //VARCHAR(50),
        public string ORDER_SEQ_NO { get; set; } = string.Empty; //VARCHAR(4),
        public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
        public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string SALE_MG_CODE { get; set; } = string.Empty; //VARCHAR(4),
        public string SALE_MG_NAME { get; set; } = string.Empty; //VARCHAR(30),
        public string DC_TYPE_FLAG { get; set; } = string.Empty; //VARCHAR(1),
        public string DLV_PACK_FLAG { get; set; } = string.Empty; //VARCHAR(1),
        public string TAX_YN { get; set; } = string.Empty; //VARCHAR(1),
        public string CORNER_CODE { get; set; } = string.Empty; //VARCHAR(2),
        public string ORG_SALE_MG_CODE { get; set; } = string.Empty; //VARCHAR(4),
        public string TIP_MENU_YN { get; set; } = string.Empty; //VARCHAR(1),
        public string SVC_TIP_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string VAT_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string SALE_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
        public string BRAND_CODE { get; set; } = string.Empty; //VARCHAR(8),
        public string PRD_TYPE_FLAG { get; set; } = string.Empty; //VARCHAR(1),
        public string DC_AMT_GEN { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_SVC { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_JCD { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_CPN { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_CST { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_FOD { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_CRD { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_PRM { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_PACK { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_LYT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string DC_AMT_TAX { get; set; } = string.Empty; //NUMERIC(12,2),
        public string CST_SALE_POINT { get; set; } = string.Empty; //NUMERIC(12,4),
        public string CST_USE_POINT { get; set; } = string.Empty; //NUMERIC(12,4),
        public string SDA_CODE { get; set; } = string.Empty; //VARCHAR(10),
        public string SDS_ORG_DTL_NO { get; set; } = string.Empty; //VARCHAR(4),
        public string ORG_SALE_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
        public string ETC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string SVC_CODE { get; set; } = string.Empty; //VARCHAR(1),
        public string TK_CPN_CODE { get; set; } = string.Empty; //VARCHAR(3),
        public string DC_SEQ_TYPE { get; set; } = string.Empty; //VARCHAR(50),
        public string DC_SEQ_AMT { get; set; } = string.Empty; //VARCHAR(500),
        public string DC_SEQ_FLAG { get; set; } = string.Empty; //VARCHAR(50),
        public string DC_SEQ_RATE { get; set; } = string.Empty; //VARCHAR(500),
        public string REMARK { get; set; } = string.Empty; //VARCHAR(30),
        public string SIDE_MENU_YN { get; set; } = string.Empty; //VARCHAR(1),
        public string TOGO_CHARGE { get; set; } = string.Empty; //NUMERIC(10,2),
        public string AFFILIATE_UPRC { get; set; } = string.Empty; //NUMERIC(12,2),
        public string MCP_BAR_CODE { get; set; } = string.Empty; //VARCHAR(40),
        public string STAMP_SALE_QTY { get; set; } = string.Empty; //NUMERIC(9,0),
        public string STAMP_USE_YN { get; set; } = string.Empty; //VARCHAR(1),
        public string STAMP_USE_QTY { get; set; } = string.Empty; //NUMERIC(9,0),
        public string SDS_CLASS_CODE { get; set; } = string.Empty; //VARCHAR(6),
        public string DOUBLE_CODE { get; set; } = string.Empty; //VARCHAR(2),
        public string DOUBLE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string PRD_WEIGHT { get; set; } = string.Empty; //NUMERIC(8,0),
        public string TAX_RFND_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
        public string TAX_RFND_FEE { get; set; } = string.Empty; //NUMERIC(12,2),
        public string PRD_DC_FLAG { get; set; } = string.Empty; //VARCHAR(1),
        public string IF_CPN_CODE { get; set; } = string.Empty; //VARCHAR(20),
        public string DC_REASON_CODE { get; set; } = string.Empty; //VARCHAR(3),
        public string DC_REASON_NAME { get; set; } = string.Empty; //VARCHAR(200),
        public string SDS_PARENT_CODE { get; set; } = string.Empty; //VARCHAR(26),
        public string DC_RULE_FLAG { get; set; } = string.Empty; //VARCHAR(10),
        public string DEPOSIT_USE_YN { get; set; } = string.Empty; //VARCHAR(1),
        public string DEPOSIT_TYPE { get; set; } = string.Empty; //VARCHAR(1))
    }
}
