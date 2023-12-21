using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoPOS.Models
{
    public class POS_ORD_ALLIANCE_CARD
    {
        /*
           CREATE TABLE POS_ORD_ALLIANCE_CARD (
           SHOP_CODE      VARCHAR(6) NOT NULL,
           SALE_DATE      VARCHAR(8) NOT NULL,
           ORDER_NO       VARCHAR(4) NOT NULL,
           LINE_NO        VARCHAR(4) NOT NULL,
           SEQ_NO         VARCHAR(2) NOT NULL,
           POS_NO         VARCHAR(2) NOT NULL,
           SALE_YN        VARCHAR(1) DEFAULT 'Y' NOT NULL,
           JCD_CODE       VARCHAR(4) DEFAULT '0000' NOT NULL,
           JCD_TYPE_FLAG  VARCHAR(1) NOT NULL,
           JCD_PROC_FLAG  VARCHAR(1) NOT NULL,
           APPR_LOG_NO    VARCHAR(5) NOT NULL,
           APPR_AMT       NUMERIC(12,2) DEFAULT 0 NOT NULL,
           JCD_DC_AMT     NUMERIC(12,2) DEFAULT 0 NOT NULL,
           JCD_OCC_POINT  NUMERIC(8,0) DEFAULT 0 NOT NULL,
           JCD_AVL_POINT  NUMERIC(8,0) DEFAULT 0 NOT NULL,
           JCD_USE_POINT  NUMERIC(8,0) DEFAULT 0 NOT NULL,
           JCD_REM_POINT  NUMERIC(8,0) DEFAULT 0 NOT NULL,
           INSERT_DT      VARCHAR(14) NOT NULL,
           EMP_NO         VARCHAR(4) NOT NULL,
           JCD_OCC_STEMP  NUMERIC(3,0),
           JCD_AVL_STEMP  NUMERIC(3,0),
           JCD_USE_STEMP  NUMERIC(3,0),
           JCD_REM_STEMP  NUMERIC(3,0),
           JCD_CUST_DC    NUMERIC(8,0),
           JCD_CUPN_DC    NUMERIC(8,0),
           JCD_CAMP_DC    NUMERIC(8,0),
           DEPOSIT_AMT    NUMERIC(12,2) DEFAULT 0 NOT NULL
       );
        //Primary keys                                
        //ALTER TABLE POS_ORD_ALLIANCE_CARD ADD CONSTRAINT POS_ODJCD_PK PRIMARY KEY(SHOP_CODE, SALE_DATE, ORDER_NO, LINE_NO, SEQ_NO);
        //COMMENT ON TABLE POS_ORD_ALLIANCE_CARD IS 'POS-주문결제-제휴카드';
        */

        string SHOP_CODE          { get; set; } = string.Empty; 
        string SALE_DATE          { get; set; } = string.Empty; 
        string ORDER_NO           { get; set; } = string.Empty; 
        string LINE_NO            { get; set; } = string.Empty; 
        string SEQ_NO             { get; set; } = string.Empty; 
        string POS_NO             { get; set; } = string.Empty; 
        string SALE_YN            { get; set; } = string.Empty; 
        string JCD_CODE           { get; set; } = string.Empty; 
        string JCD_TYPE_FLAG      { get; set; } = string.Empty; 
        string JCD_PROC_FLAG      { get; set; } = string.Empty; 
        string APPR_LOG_NO        { get; set; } = string.Empty; 
        string APPR_AMT           { get; set; } = string.Empty; 
        string JCD_DC_AMT         { get; set; } = string.Empty; 
        string JCD_OCC_POINT      { get; set; } = string.Empty; 
        string JCD_AVL_POINT      { get; set; } = string.Empty; 
        string JCD_USE_POINT      { get; set; } = string.Empty; 
        string JCD_REM_POINT      { get; set; } = string.Empty; 
        string INSERT_DT          { get; set; } = string.Empty; 
        string EMP_NO             { get; set; } = string.Empty; 
        string JCD_OCC_STEMP      { get; set; } = string.Empty; 
        string JCD_AVL_STEMP      { get; set; } = string.Empty; 
        string JCD_USE_STEMP      { get; set; } = string.Empty; 
        string JCD_REM_STEMP      { get; set; } = string.Empty; 
        string JCD_CUST_DC        { get; set; } = string.Empty; 
        string JCD_CUPN_DC        { get; set; } = string.Empty; 
        string JCD_CAMP_DC        { get; set; } = string.Empty; 
        string DEPOSIT_AMT        { get; set; } = string.Empty;
    }
}
