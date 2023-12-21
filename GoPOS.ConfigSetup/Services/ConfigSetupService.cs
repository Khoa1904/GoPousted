using Dapper;
using FirebirdSql.Data.FirebirdClient;
using GoPOS.Database;
using GoPOS.Helpers;
using GoPOS.Models;
using GoPOS.Models._0_Common;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace GoPOS.Services;

/// <summary>
/// 작성자: 
/// 화면명: POS 환경설정  생성 통합
/// 2023-03-19 
/// </summary>

public class ConfigSetupService : IConfigSetupService
{
    #region ConfigPosConfigSetupService

    /// <summary>
    /// ConfigMstrRecptnSetupView 메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<CMM_MASTER_RECV>, SpResult)> ConfigMstrRecptnSetup_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"
                           SELECT  RECV_ID
                               ,   REQT_RM
                               ,   LAST_SEQ
                               ,   UPDATE_DT
                               ,   CASE( SUBSTR( REQT_NAME, 1, 5 ) )
                                       WHEN 'CODEM' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'CRDCP' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'ENVDT' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'ENVHD' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'ENVPD' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'VANCM' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'PAYPR' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'     -- 2018-1102-LSH
                                       WHEN 'CSTCL' THEN 'HCS_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'CSTMS' THEN 'HCS_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'CSTPT' THEN 'HCS_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'DLTEL' THEN 'HCS_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'DLADR' THEN 'HCS_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'
                                       WHEN 'PAYCP' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'     -- 2021-1025-SSH-14076
                                       WHEN 'VANPM' THEN 'CCD_' || SUBSTR( REQT_NAME, 1, 5 ) || '_T'     -- 2021-1025-SSH-14076
                                       WHEN 'MCODE' THEN 'MST_COMM_CODE_SHOP'
                                       ELSE
                                           CASE ( SUBSTR( REQT_NAME, 1, 3 ) )
                                               WHEN 'SUN' THEN SUBSTR( REQT_NAME, 1, 9 ) || '_T'         -- 2019-1018-SSH-01
                                               ELSE 'SCD_' || SUBSTR(REQT_NAME, 1, 5) || '_T'
                                           END
                                   END AS TABLE_NAME
                           FROM CMM_MASTER_RECV
                           ORDER BY REQT_RM;
                           ";
            List<CMM_MASTER_RECV> result = await DapperORM.ReturnListAsync<CMM_MASTER_RECV>(SQL, param);

            /*
            RECV_ID     마스터일련번호
            REQT_ID     마스터수신-전문구문ID
            REQT_NAME   마스터수신-전문명
            LAST_SEQ    최종수신-일련번호
            REQT_RM     마스터수신-전문비고
            */
            //REQT_NAME { get; set; } = string.Empty;   // '마스터수신-전문명';
            //LAST_SEQ { get; set; } = string.Empty;   // '최종수신-일련번호';
            //REQT_RM { get; set; } = string.Empty;   // '마스터수신-전문비고';
            //UPDATE_DT { get; set; } = string.Empty;   // '최종수정일시'

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CMM_MASTER_RECV>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CMM_MASTER_RECV>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    //
    public async Task<(CMM_MASTER_RECV, SpResult)> SP_CMM_MASTER_RECV_U(DynamicParameters param)
    {
        try
        {
            List<CMM_MASTER_RECV> mainList = param.Get<List<CMM_MASTER_RECV>>("@MainList");

            using TransactionScope scope = new();            
            scope.Complete();

            for (int i = 0; i < mainList.Count; i++)
            {
                DynamicParameters dynamicParameters = new DynamicParameters();

                dynamicParameters.Add("@LAST_SEQ", mainList[i].LAST_SEQ.Replace(",", ""));
                dynamicParameters.Add("@RECV_ID", mainList[i].RECV_ID.Replace(",", "").Replace("건수", "").Trim());

                //SP_SCD_ENVPS_U 참고
                string SQL = @"UPDATE CMM_MASTER_RECV
                           SET LAST_SEQ  = @LAST_SEQ
                           WHERE RECV_ID = @RECV_ID";

                int result = await DapperORM.ExecuteAsync(SQL, dynamicParameters);

                scope.Complete();
                if (result > 0)
                    LogHelper.Logger.Info("CMM_MASTER_RECV Update Success {0}" + i.ToString());
                else
                    LogHelper.Logger.Error("CMM_MASTER_RECV Update Failed {0}" + i.ToString());
            }

            return DapperORM.ReturnResult(new CMM_MASTER_RECV(), EResultType.SUCCESS, "OK");
        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new CMM_MASTER_RECV(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new CMM_MASTER_RECV(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }

    }

    /// <summary>
    /// ConfigTrmnlCrtfcViewModel 메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<SHOP_VAN>, SpResult)> ConfigTrmnlCrtfc_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"SELECT A.BIZ_NO     
                                , A.CORNER_CODE     
                                , A.CORNER_NAME     
                                , A.OWNER_NAME     
                                , A.VAN_TERM_NO     
                                , A.VAN_SER_NO     
                                , ( SELECT VAN_CERT_SDT         
                                    FROM MST_INFO_POS         
                                    WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_CERT_SDT     
                                , ( SELECT VAN_CERT_CNT FROM MST_INFO_POS WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_CERT_CNT     
                                , ( SELECT VAN_SAM_ID FROM MST_INFO_POS   WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_SAM_ID     
                                , ( SELECT VAN_SAM_NO FROM MST_INFO_POS   WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_SAM_NO     
                                , ( SELECT WORK_INDEX FROM MST_INFO_POS   WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS WORK_INDEX     
                                , ( SELECT WORK_KEY FROM MST_INFO_POS     WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS WORK_KEY     
                                , ( SELECT VAN_CERT_YN FROM MST_INFO_POS  WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_CERT_YN     
                                , ( SELECT W_KEY FROM MST_INFO_POS        WHERE SHOP_CODE = A.SHOP_CODE AND   POS_NO  = @POS_NO ) AS VAN_SECU_YN
                           FROM SHOP_CORNER A
                           WHERE 
                           SHOP_CODE = @SHOP_CODE 
                           AND USE_YN  = 'Y'
                           AND CORNER_CODE = '00'";
            List<SHOP_VAN> result = await DapperORM.ReturnListAsync<SHOP_VAN>(SQL, param);
            

                //result.Add(new SHOP_VAN() { NO = (i + 1).ToString(), BIZ_NO = "6075180224", SHOP_CODE = "기본코너", OWNER_NAME = "강주원", VAN_TERM_NO = "2761626001", VAN_SER_NO = "", VAN_CERT_YN = "성공", VAN_STATUS = "성공" });

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_VAN>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<SHOP_VAN>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    /// <summary>
    /// 매출자료 수신 ConfigSellingDataRecptn 메인 리스트1
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<POS_ORD_M>, SpResult)> ConfigSellingDataRecptn_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"";
            //List<POS_ORD_M> result = await DapperORM.ReturnListAsync<POS_ORD_M>(SQL, param);
            List<POS_ORD_M> result = new List<POS_ORD_M>();

            for (int i = 0; i < 3; i++)
            {
                Random rnd = new Random();
                int rIndex1 = rnd.Next();

                int rIndex2 = rnd.Next(1, 30);

                /*
                SALE_QTY { get; set; } = ""; // 영수건수
                TOT_SALE_AMT { get; set; } = "";
                
                DCM_SALE_AMT { get; set; } = "";
                VAT_SALE_AMT { get; set; } = "";
                
                VAT_AMT { get; set; } = "";
                TOT_AMT { get; set; } = "";
                */

                string dt = DateTime.Now.AddDays(i).ToString("yyyyMMdd");
                //result.Add(new POS_ORD_M() { NO = (i + 1).ToString(), BIZ_NO = "사업자번호_" + (i + 1).ToString(),  = rIndex1.ToString(), REQT_RM = rIndex2.ToString(), UPDATE_DT = dt });

                result.Add(new POS_ORD_M() { NO = (i + 1).ToString(), SALE_QTY = rIndex2.ToString(), TOT_SALE_AMT = rIndex2.ToString() + "10,000", DCM_SALE_AMT = rIndex2.ToString() + "200", TOT_AMT = rIndex2.ToString() + "9,800", SALE_QTY_L = rIndex2.ToString(), TOT_SALE_AMT_L = rIndex2.ToString() + "10,000", DCM_SALE_AMT_L = rIndex2.ToString() + "200", TOT_AMT_L = rIndex2.ToString() + "9,800" });

            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    /// <summary>
    /// 매출자료 수신 ConfigSellingDataRecptn 서브 리스트2
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<POS_ORD_M>, SpResult)> ConfigSellingDataRecptn_GetList2(DynamicParameters param)
    {
        try
        {

            string SQL = @"";
            //List<POS_ORD_M> result = await DapperORM.ReturnListAsync<POS_ORD_M>(SQL, param);
            List<POS_ORD_M> result = new List<POS_ORD_M>();

            for (int i = 0; i < 5; i++)
            {
                Random rnd = new Random();
                int rIndex1 = rnd.Next();

                int rIndex2 = rnd.Next(1, 10);

                /*
                */
                string dt = DateTime.Now.AddDays(i).ToString("yyyyMMdd");
                //result.Add(new POS_ORD_M() { NO = (i + 1).ToString(), BIZ_NO = "사업자번호_" + (i + 1).ToString(),  = rIndex1.ToString(), REQT_RM = rIndex2.ToString(), UPDATE_DT = dt });

                result.Add(new POS_ORD_M() { POS_NO = "01", BILL_NO = "000" + (i + 1).ToString(), SALE_YN = "매출", TOT_AMT = rIndex2.ToString() + "10,000", TOT_DC_AMT = rIndex2.ToString() + "100", TOT_SALE_AMT = rIndex2.ToString() + "9,900", CASH_AMT = rIndex2.ToString() + "9,900", CASH_BILL_AMT = "", CRD_CARD_AMT = rIndex2.ToString() + ",000", TOT_ETC_AMT = "" });

                //REQT_NAME { get; set; } = string.Empty;   // '마스터수신-전문명';
                //LAST_SEQ { get; set; } = string.Empty;   // '최종수신-일련번호';
                //REQT_RM { get; set; } = string.Empty;   // '마스터수신-전문비고';
                //UPDATE_DT { get; set; } = string.Empty;   // '최종수정일시'
            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    /// </summary>
    /// 포스데이터관리 ConfigPosDataMng 메인 리스트1
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<POS_ORD_M>, SpResult)> ConfigPosDataMng_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"";
            //List<POS_ORD_M> result = await DapperORM.ReturnListAsync<POS_ORD_M>(SQL, param);
            List<POS_ORD_M> result = new List<POS_ORD_M>();

            for (int i = 0; i < 20; i++)
            {
                Random rnd = new Random();
                int rIndex1 = rnd.Next();

                int rIndex2 = rnd.Next(0, 10);

                /*
          public string SALE_QTY { get; set; } = ""; // 영수건수
        public string TOT_SALE_AMT { get; set; } = "";

        public string DCM_SALE_AMT { get; set; } = "";
        public string VAT_SALE_AMT { get; set; } = "";

        public string VAT_AMT { get; set; } = "";
        public string TOT_AMT { get; set; } = "";
                */

                string dt = DateTime.Now.AddDays(-i).ToString("yyyyMMdd");
                //result.Add(new POS_ORD_M() { NO = (i + 1).ToString(), BIZ_NO = "사업자번호_" + (i + 1).ToString(),  = rIndex1.ToString(), REQT_RM = rIndex2.ToString(), UPDATE_DT = dt });

                result.Add(new POS_ORD_M() { NO = (i + 1).ToString(), SALE_DATE = dt, SALE_QTY = ((1 + i) * 2).ToString(), TOT_SALE_AMT = (rIndex2 + 1).ToString() + "0,00", DCM_SALE_AMT = (rIndex2 + 1).ToString() + "0,000", VAT_SALE_AMT = (rIndex2 + 1).ToString() + "0,000", VAT_AMT = (i + 1).ToString() + "0,000", TOT_AMT = (i + 1).ToString() + "0,00" });

            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_ORD_M>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    /// </summary>
    /// 보안리더기 무결성 점검 ConfigScrtyRdrIntgrtyChck 메인 리스트1
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<POS_LGSRC_T>, SpResult)> ConfigScrtyRdrIntgrtyChck_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"SELECT                            
                           SALE_DATE,
                           POS_NO,
                           INTEGRITY_SEQ,
                           INTEGRITY_FLAG,
                           INTEGRITY_DATE,
                           INTEGRITY_YN
                           FROM POS_LOG_MSR_INTEGRITY
                           WHERE SHOP_CD   = @SHOP_CD
                           AND   SALE_DATE = @SALE_DATE
                           AND   POS_NO    = @POS_NO";
            List<POS_LGSRC_T> result = await DapperORM.ReturnListAsync<POS_LGSRC_T>(SQL, param);
            

            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_LGSRC_T>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<POS_LGSRC_T>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }

    /// </summary>
    /// 환경설정	매출자료전송 (송신) ConfigSellingDataTrnsmis
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<(List<CONFIG_SETUP_COM>, SpResult)> ConfigSellingDataTrnsmis_GetList1(DynamicParameters param)
    {
        try
        {

            string SQL = @"";
            //List<CONFIG_SETUP_COM> result = await DapperORM.ReturnListAsync<CONFIG_SETUP_COM>(SQL, param);
            List<CONFIG_SETUP_COM> result = new List<CONFIG_SETUP_COM>();

            for (int i = 0; i < 10; i++)
            {
                Random rnd = new Random();
                int rIndex1 = rnd.Next();
                int rIndex2 = rnd.Next(1, 10);

                /*
                // 매출자료 송신
                public string BILL_NO { get; set; } = string.Empty;  // 영수증번호
                public string TOT_SALE_AMT { get; set; } = string.Empty; // 판매금액
                public string SALE_YN { get; set; } = string.Empty;   // 판매구분 반품 / 정상
                public string SEND_FLAG { get; set; } = string.Empty; // 전송구분
                
                // 추가 공통
                public string NO { get; set; } = string.Empty;
                */

                string dt = DateTime.Now.AddDays(i).ToString("yyyyMMdd");
                //result.Add(new CONFIG_SETUP_COM() { NO = (i + 1).ToString(), BIZ_NO = "사업자번호_" + (i + 1).ToString(),  = rIndex1.ToString(), REQT_RM = rIndex2.ToString(), UPDATE_DT = dt });

                result.Add(new CONFIG_SETUP_COM() { NO = (i + 1).ToString(), POS_NO = "01", BILL_NO = "0000" + (i + 1).ToString(), TOT_SALE_AMT = (rIndex2 + 1).ToString() + ",000", SALE_YN = "판매", SEND_FLAG = "송신" });
            }
            return DapperORM.ReturnResult(result, EResultType.SUCCESS, "OK");

        }
        catch (FbException ex)
        {
            LogHelper.Logger.Error($"Database Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CONFIG_SETUP_COM>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
        catch (Exception ex)
        {
            LogHelper.Logger.Error($"Services Error : {ex.Message.ReplacePlainText()}");
            return DapperORM.ReturnResult(new List<CONFIG_SETUP_COM>(), EResultType.ERROR, ex.Message.ReplacePlainText());
        }
    }
    #endregion
}