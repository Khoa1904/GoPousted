using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Service.Common;
using GoPOS.Service;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using GoPOS.Helpers;
using System.Windows;
using Microsoft.IdentityModel.Xml;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace GoPOS.Payment.Services
{
    public class OrderPayPointStampService : IOrderPayPointStampService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<(List<MEMBER_GRADE>, string)> GetMEMBER_GRADEs()
        {
            List<MEMBER_GRADE> memberGrades = null;
            string errMessage = "";
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE
            };
            var w = await _apiRequest.Request("client/inquiry/member/grade").GetDatasBodyAsync(null, pairs);
            if (w.status == "200")
            {
                try
                {
                    var members = JsonHelper.JsonToModels<MEMBER_GRADE>(Convert.ToString(w.results));
                    if (members.result == ResultCode.Ok)
                    {
                        memberGrades = members.model;
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                }
            }
            else
            {
                errMessage = "서버에 요청한 자료를 정상 수신 받지 못하였습니다.\n" + w.ResultMsg.ToString();
            }

            return (memberGrades, errMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="memberInfo"></param>
        /// <param name="trnHeader"></param>
        /// <param name="payTenders"></param>
        /// <param name="trnPrdts"></param>
        /// <returns></returns>
        public async Task<(MEMBER_CLASH, object[])> RequestSavePoint(string billNo, MEMBER_CLASH memberInfo, TRN_HEADER trnHeader, TRN_TENDERSEQ[] payTenders, TRN_PRDT[] trnPrdts)
        {
            string errorMessage = string.Empty;
            var result = new SpResult()
            {
                ResultType = EResultType.ERROR,
                ResultCode = "9999"
            };

            List<object> paySeqsList = new List<object>();

            paySeqsList.AddRange(payTenders.Where(p => p.PAY_TYPE_FLAG != "00").Select(p => new
            {
                payType = p.PAY_TYPE_FLAG,
                payAmt = p.PAY_AMT
            }).ToArray());


            /// mstPrdt.STAMP_ACC_YN
            decimal accAmt = 0;
            decimal excAmt = 0;

            using (var context = new DataContext())
            {
                if (DataLocals.AppConfig.PosOption.PointStampFlag == "0")
                //    (DataLocals.AppConfig.PosOption.PointStampFlag == "1" &&
                //        DataLocals.AppConfig.PosOption.StampUseMethod == "0"))
                {
                    accAmt = (from trnPrdt in trnPrdts
                              join mstPrdt in context.mST_INFO_PRODUCTs on
                              new
                              {
                                  trnPrdt.SHOP_CODE,
                                  trnPrdt.PRD_CODE,
                              }
                              equals
                              new
                              {
                                  mstPrdt.SHOP_CODE,
                                  mstPrdt.PRD_CODE
                              }
                              into pd
                              from sybDev in pd.DefaultIfEmpty()
                              where sybDev?.STAMP_ACC_YN == "Y"
                              select new
                              {
                                  SALE_AMT = trnPrdt.SALE_AMT,
                                  DC_AMT = trnPrdt.DC_AMT
                              }).Sum(p => p.SALE_AMT - p.DC_AMT);
                    excAmt = (from trnPrdt in trnPrdts
                              join mstPrdt in context.mST_INFO_PRODUCTs on
                              new
                              {
                                  trnPrdt.SHOP_CODE,
                                  trnPrdt.PRD_CODE,
                              }
                              equals
                              new
                              {
                                  mstPrdt.SHOP_CODE,
                                  mstPrdt.PRD_CODE
                              }
                              into pd
                              from sybDev in pd.DefaultIfEmpty()
                              where sybDev?.STAMP_ACC_YN != "Y"
                              select new
                              {
                                  SALE_AMT = trnPrdt.SALE_AMT,
                                  DC_AMT = trnPrdt.DC_AMT
                              }).Sum(p => p.SALE_AMT - p.DC_AMT);

                }
                else
                {
                    accAmt = Convert.ToDecimal((from trnPrdt in trnPrdts
                                                join mstPrdt in context.mST_INFO_PRODUCTs on
                                                new
                                                {
                                                    trnPrdt.SHOP_CODE,
                                                    trnPrdt.PRD_CODE,
                                                }
                                                equals
                                                new
                                                {
                                                    mstPrdt.SHOP_CODE,
                                                    mstPrdt.PRD_CODE
                                                }
                                                into pd
                                                from sybDev in pd.DefaultIfEmpty()
                                                where sybDev?.STAMP_ACC_YN == "Y"
                                                select new
                                                {
                                                    SALE_QTY = trnPrdt.SALE_QTY,
                                                    ACC_QTY = sybDev.STAMP_ACC_QTY
                                                }).Sum(p => p.SALE_QTY * p.ACC_QTY));
                    excAmt = Convert.ToDecimal((from trnPrdt in trnPrdts
                                                join mstPrdt in context.mST_INFO_PRODUCTs on
                                                new
                                                {
                                                    trnPrdt.SHOP_CODE,
                                                    trnPrdt.PRD_CODE,
                                                }
                                                equals
                                                new
                                                {
                                                    mstPrdt.SHOP_CODE,
                                                    mstPrdt.PRD_CODE
                                                }
                                                into pd
                                                from sybDev in pd.DefaultIfEmpty()
                                                where sybDev?.STAMP_ACC_YN != "Y"
                                                select new
                                                {
                                                    SALE_QTY = trnPrdt.SALE_QTY
                                                }).Sum(p => p.SALE_QTY));
                }

            }

            MEMBER_CLASH accMemberInfo = null;
            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                //billNo = billNo,
                billNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4),
                mbrCode = memberInfo.mbrCode,
                totalSaleAmt = trnHeader.TOT_SALE_AMT,
                totalDcAmt = trnHeader.TOT_DC_AMT,
                accAmt = accAmt,
                excAmt = excAmt,
                createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                paySeq = paySeqsList.ToArray()
            };

            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                DataLocals.AppConfig.PosInfo.StoreNo,
                  DataLocals.AppConfig.PosInfo.PosNo,
                  DataLocals.TokenInfo.LICENSE_ID,
                  DataLocals.TokenInfo.LICENSE_KEY);
            var w = await _apiRequest.Request("client/inquiry/point/acml").PostBodyAsync(pairs);
            if (w.status == "200")
            {
                accMemberInfo = JsonHelper.JsonToModel<MEMBER_CLASH>(Convert.ToString(w.results)).model;
            }
            else
            {
                if (w.results != null)
                {
                    errorMessage = w.results.ToString();
                }
                else
                {
                    errorMessage = w.ResultMsg;
                }

            }
            return await Task.FromResult((accMemberInfo, new object[] { accAmt, excAmt, errorMessage }));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="memberInfo"></param>
        /// <param name="tRN_POINTUSE"></param>
        /// <returns>
        /// object[]: 
        ///     0: errorMessage, if null, no error
        ///     1: TRN_POINTUSE
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<(MEMBER_CLASH, object[])> RequestUsePointStamp(string billNo, MEMBER_CLASH memberInfo, TRN_POINTUSE trnPointUse, decimal usePointStampAmt, decimal useStampQty)
        {
            string errorMessage = string.Empty;

            // 01:포인트사용, 02:스탬프사용, 03:스탬프쿠폰
            string useType = DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "01" :
                            (DataLocals.AppConfig.PosOption.StampUseMethod == "0" ? "02" : "03");
            string typeText = DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? "포인트" : "스탬프" +
                        (DataLocals.AppConfig.PosOption.StampUseMethod == "0" ? "금액" : "수");
            int useTypeInt = DataLocals.AppConfig.PosOption.PointStampFlag == "0" ? 0 : (DataLocals.AppConfig.PosOption.StampUseMethod == "0" ? 1 : 2);
            if (memberInfo == null)
            {
                errorMessage = "회원을 선택해 주세요.";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }

            if (memberInfo.minUsePt > (useTypeInt == 2 ? useStampQty : usePointStampAmt) && memberInfo.minUsePt > 0)
            {
                errorMessage = $"결제할 {typeText}가 최소사용액보다 커야 합니다.";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }
            if (memberInfo.maxUsePt < (useTypeInt == 2 ? useStampQty : usePointStampAmt) && memberInfo.maxUsePt > 0)
            {
                errorMessage = $"결제할 {typeText}는(은) 최대사용액보다 적어야합니다.";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }
            // 포인트 사용은 avalidPoint 가용스탬프 체크
            if (useTypeInt == 0 && memberInfo.avalidPoint < usePointStampAmt)
            {
                errorMessage = $"가용{typeText}가 결제할 {typeText}보다 작습니다";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }
            // 스탬프 금액 사용은 avalidStampAmt 사용가능 스탬프 금액 체크
            if (useTypeInt == 1 && memberInfo.avalidStampAmt < usePointStampAmt)
            {
                errorMessage = $"가용{typeText}이 결제할 {typeText}보다 작습니다";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }
            // 스탬프 쿠폰 사용은 avalidPoint 가용스탬프 체크
            if (useTypeInt == 2 && memberInfo.avalidPoint < useStampQty)
            {
                errorMessage = $"가용{typeText}가 결제할 {typeText}보다 작습니다";
                return await Task.FromResult((memberInfo, new object[] { errorMessage }));
            }

            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            ApiResult apr = null;

            if (DataLocals.AppConfig.PosOption.PointStampFlag == "0" ||
                (DataLocals.AppConfig.PosOption.PointStampFlag == "1" &&
                    DataLocals.AppConfig.PosOption.StampUseMethod == "0"))
            {
                var pairs = new
                {
                    posNo = DataLocals.AppConfig.PosInfo.PosNo,
                    salesDt = DataLocals.PosStatus.SALE_DATE,
                    mbrCode = memberInfo.mbrCode,
                    billNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4),
                    totalSaleAmt = trnPointUse.TOT_SALE_AMT,
                    totalDcAmt = trnPointUse.TOT_DC_AMT,
                    totalPoint = decimal.Truncate(memberInfo.totalPoint),
                    avalidPoint = decimal.Truncate(memberInfo.avalidPoint),
                    usePoint = usePointStampAmt,
                    createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                };

                apr = await _apiRequest.Request("/client/inquiry/point/use").PostBodyAsync(pairs);
            }
            else
            {
                var pairs = new
                {
                    posNo = DataLocals.AppConfig.PosInfo.PosNo,
                    salesDt = DataLocals.PosStatus.SALE_DATE,
                    mbrCode = memberInfo.mbrCode,
                    billNo = DataLocals.PosStatus.BILL_NO.StrIntInc(4),
                    totalSaleAmt = trnPointUse.TOT_SALE_AMT,
                    totalDcAmt = trnPointUse.TOT_DC_AMT,
                    totalPoint = decimal.Truncate(memberInfo.totalPoint),
                    avalidPoint = decimal.Truncate(memberInfo.avalidPoint),
                    usePoint = usePointStampAmt,
                    useStampCnt = useStampQty,
                    createdAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                };

                apr = await _apiRequest.Request("/client/inquiry/point/use").PostBodyAsync(pairs);
            }

            if (apr.status == "200")
            {
                try
                {
                    var members = JsonHelper.JsonToModel<MEMBER_CLASH>(Convert.ToString(apr.results));
                    if (members.result == ResultCode.Ok)
                    {
                        memberInfo = members.model;
                    }

                    trnPointUse.CST_NO = memberInfo.mbrCode;
                    trnPointUse.TOT_USE_PNT = memberInfo.totalUsePoint;
                    trnPointUse.TOT_PNT = decimal.Truncate(memberInfo.totalPoint);
                    trnPointUse.LAST_PNT = decimal.Truncate(memberInfo.avalidPoint);
                    trnPointUse.USE_PNT = usePointStampAmt;
                    trnPointUse.USE_STAMP = memberInfo.useStampCnt; // useStampQty;
                    trnPointUse.USE_TYPE = useType;
                    trnPointUse.LEVEL = memberInfo.mbrGrdCode;
                    trnPointUse.CARD_NO = memberInfo.mbrCardno;
                }
                catch (Exception ex)
                {
                    errorMessage = "API 오류. 관리자에게 문의하세요.!" + Environment.NewLine;
                    errorMessage += ex.Message;
                }
            }
            else
            {
                if (apr.ResultMsg.Length > 0)
                {
                    errorMessage = apr.ResultMsg.ToString() + "\nError Code: " + apr.status;
                }
                else
                {
                    errorMessage = apr.results.ToString() + "\nError Code: " + apr.status;
                }
            }


            return await Task.FromResult((memberInfo, new object[] { errorMessage, trnPointUse }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posNo"></param>
        /// <param name="saleDate"></param>
        /// <param name="memberCode"></param>
        /// <returns></returns>
        public async Task<(string, MEMBER_CLASH)> GetMemberDetail(string posNo, string saleDate, string memberCode)
        {
            MEMBER_CLASH memberInfo = null;
            string errorMessage = string.Empty;
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);

            var pairs = new
            {
                posNo = posNo,
                salesDt = saleDate,
                mbrCode = memberCode
            };
            var check = DataLocals.TokenInfo.TOKEN;
            var w = await _apiRequest.Request("client/inquiry/member/detail").GetDatasBodyAsync(null, pairs);
            if (w.status == ResultCode.Success)
            {
                try
                {
                    var members = JsonHelper.JsonToModel<MEMBER_CLASH>(Convert.ToString(w.results));
                    if (members.result == ResultCode.Ok)
                    {
                        memberInfo = members.model;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "API 오류. 관리자에게 문의하세요!" + Environment.NewLine;
                    errorMessage += ex.ToFormattedString();
                }
            }
            else
            {
                errorMessage = w.results.ToString();
            }

            return await Task.FromResult((errorMessage, memberInfo));
        }

        public async Task<(string, List<MEMBER_CLASH>)> SearchMembers(string posNo, string saleDate, string cellNo, string cardNo, string memberNm)
        {
            List<MEMBER_CLASH> resList = null;
            string errorMessage = "";
            var _apiRequest = new ApiRequest(DataLocals.AppConfig.PosComm.SvrURLServer,
                   DataLocals.AppConfig.PosInfo.StoreNo,
                   DataLocals.AppConfig.PosInfo.PosNo,
                   DataLocals.TokenInfo.LICENSE_ID,
                   DataLocals.TokenInfo.LICENSE_KEY);
            var token = DataLocals.TokenInfo.TOKEN;
            var pairs = new
            {
                posNo = DataLocals.AppConfig.PosInfo.PosNo,
                salesDt = DataLocals.PosStatus.SALE_DATE,
                mbrCelno = cellNo,
                mbrCardno = cardNo,
                mbrNm = memberNm
            };
            var w = await _apiRequest.Request("client/inquiry/member").GetDatasBodyAsync(null, pairs);

            if (w.status == ResultCode.Success)
            {
                try
                {
                    var members = JsonHelper.JsonToModels<MEMBER_CLASH>(Convert.ToString(w.results));
                    if (members.result == ResultCode.Ok)
                    {
                        resList = members.model;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = "포인트 회원등록 API 응답 데이터.";
                    errorMessage += Environment.NewLine;
                    errorMessage += ex.ToFormattedString();
                }
            }
            else
            {
                errorMessage = w.results.ToString();
            }

            return await Task.FromResult((errorMessage, resList));
        }
    }
}
