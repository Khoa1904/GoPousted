using Caliburn.Micro;
using Dapper;
using GoPOS.Common.Service;
using GoPOS.Common.ViewModels;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Sales.Services;
using GoPOS.Service.Service;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Events;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static GoPOS.Function;
using static log4net.Appender.RollingFileAppender;

/*
 영업관리 > 시재입출금
 */

/*             
   ////시재 동적 구성
   Task<(List<SHOP_ACCOUNT>, SpResult)> GetAccountInfo(DynamicParameters param);
   ////시재입출금 현황 리스트
   Task<(List<SHOP_ACCOUNT>, SpResult)> GetInOutAccountList(DynamicParameters param);
   ////시재입출금 등록
   Task<(SHOP_ACCOUNT, SpResult)> InsertInOutAccount(DynamicParameters param);
 */

namespace GoPOS.ViewModels
{

    public class SalesTmPrRcppayViewModel : OrderPayChildViewModel
    {
        //**----------------------------------------------------------------------------------

        #region Contructor
        public SalesTmPrRcppayViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, ISalesTmPrRcppayService payServant,
        ISalesMngService salesMngService, IPOSPrintService pOSPrintService) : base(windowManager, eventAggregator)
        {
            this.salesTmPrRcppayService = payServant;
            _sDate = DataLocals.PosStatus.SALE_DATE?.S2Date() ?? DateTime.Now;
            NotifyOfPropertyChange(() => SearchDate);
        }

        #endregion

        //**----------------------------------------------------------------------------------

        #region Member

        #region Property

        public string SSearchDate { get => _sDate.GetValueOrDefault().ToString(Formats.SystemShowDate); set { _sDate = Function.S2Date(value, Formats.SystemShowDate); } }
        public DateTime SearchDate { get => _sDate.GetValueOrDefault(); set { _sDate = value; NotifyOfPropertyChange(() => SearchDate); NotifyOfPropertyChange(() => SSearchDate); } }
        public string SettlementOrder { get => _settlementOrder; set { _settlementOrder = value; NotifyOfPropertyChange(() => SettlementOrder); } }

        #endregion

        //**----------------------------------------------------------------------------------

        private DateTime? _sDate = DateTime.Now;
        private string _settlementOrder = string.Empty;

        //**----------------------------------------------------------------------------------

        private readonly IWindowManager _manager = new WindowManager();
        private readonly ISalesTmPrRcppayService salesTmPrRcppayService;

        List<SHOP_ACCOUNT> AccountInfo1 = new List<SHOP_ACCOUNT>();
        List<SHOP_ACCOUNT> AccountInfo2 = new List<SHOP_ACCOUNT>();
        List<SHOP_ACCOUNT> InOutAccountList = new List<SHOP_ACCOUNT>();

        private int currentPage1 = 1;
        private int totalPage1 = 0;
        private int pageCnt1 = 6;

        private int currentPage2 = 1;
        private int totalPage2 = 0;
        private int pageCnt2 = 6;
        //private IEventAggregator _eventAggregator;
        #endregion

        //**----------------------------------------------------------------------------------

        #region Public Method
        private async void Init()
        {
            //await Task.Delay(100);

            //1. 클리어 AllClear
            AllClear();
            //2. 개시일자 / 정산차수input ControlSet
            ControlSet();

            //IoC.Get<SalesTmPrRcppayView>().form.Children.Clear();
            //IoC.Get<SalesTmPrRcppayView>().form2.Children.Clear();

            //3. 시재입출금 동적 구성 GetAccountInfo
            await GetAccountInfo("0");
            await GetAccountInfo("1");
            //4. 시재입출금 현황 GetInOutAccountList
            await GetInOutAccountList();

        }

        #endregion

        //**----------------------------------------------------------------------------------

        #region Protected
        protected virtual void OnCalendaShow(object send, EventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.MinWidth = SystemHelper.CalendarMinSize.Width;
            settings.MaxHeight = SystemHelper.CalendarMaxSize.Height;
            settings.WindowStyle = WindowStyle.None;

            var calendar = IoC.Get<CalendarViewModel>();
            calendar.CurrentDate = SearchDate;
            _manager.ShowDialogAsync(calendar, null, settings);
            SearchDate = calendar.SelectDate.GetValueOrDefault();
        }
        protected virtual void OnSearch(object send, EventArgs e)
        {

        }
        protected virtual void OnDelete(object send, EventArgs e) {
            if (DialogHelper.MessageBox("시재 입출금을 삭제하시겠습니까?") != MessageBoxResult.OK)
            {
                return;
            }
        }
        protected virtual async void OnInsert(object send, EventArgs e)
        {

            try
            {
                /*
                 * SP_POS_CASH_IO_AUD
                     SHOP_CODE VARCHAR(6),
                     SALE_DATE VARCHAR(8),
                     POS_NO VARCHAR(2),
                     REGI_SEQ VARCHAR(2),
                     ACCNT_FLAG VARCHAR(1),
                     ACCNT_CODE VARCHAR(2),
                     ACCNT_AMT NUMERIC(12,2),
                     REMARK VARCHAR(100),
                     EMP_NO VARCHAR(4))
                 */


                SHOP_ACCOUNT result = new SHOP_ACCOUNT();
                DynamicParameters param = new();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo);
                param.Add("@SALE_DATE", DataLocals.PosStatus.SALE_DATE);
                param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo);
                param.Add("@REGI_SEQ", DataLocals.PosStatus.REGI_SEQ);
                //param.Add("@ACCNT_FLAG", IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG.Text);
                //param.Add("@ACCNT_CODE", IoC.Get<SalesTmPrRcppayView>().txtACCNT_CODE.Text);
                //param.Add("@ACCNT_AMT", CommaDelStr(IoC.Get<SalesTmPrRcppayView>().txtACCNT_AMT.Text));
                //param.Add("@REMARK", IoC.Get<SalesTmPrRcppayView>().txtREMARK.Text);
                param.Add("@EMP_NO", DataLocals.Employee.EMP_NO);

                (result, SpResult spResult) = await salesTmPrRcppayService.InsertInOutAccount(param);


                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    LogHelper.Logger.Error("Insert" + spResult.ResultType);
                    return;
                }

                await GetInOutAccountList();

            }
            catch (Exception a)
            {
                //Console.WriteLine(a.Message);
                LogHelper.Logger.Error(a.Message);
            }
        }

        #endregion

        //**----------------------------------------------------------------------------------

        #region Private method
        //1. 클리어 AllClear   
        private void AllClear()
        {
            //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG.Text = "";
            //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG_NAME.Text = "";
            //IoC.Get<SalesTmPrRcppayView>().txtACCNT_CODE.Text = "";
            //IoC.Get<SalesTmPrRcppayView>().txtACCNT_NAME.Text = "";
            //IoC.Get<SalesTmPrRcppayView>().txtACCNT_AMT.Text = "";
            //IoC.Get<SalesTmPrRcppayView>().txtREMARK.Text = "";
        }

        //2. 개시일자 / 정산차수input ControlSet
        private void ControlSet()
        {
            //IoC.Get<SalesTmPrRcppayView>().txtSALE_DATE.Text = ChgData(DataLocals.PosStatus.SALE_DATE);
            //IoC.Get<SalesTmPrRcppayView>().txtREGI_SEQ.Text = DataLocals.PosStatus.REGI_SEQ;
        }

        //3. 시재입출금 동적 구성 GetAccountInfo
        private async Task GetAccountInfo(string sAccntFlag)
        {
            try
            {

                if (sAccntFlag == "0")
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                    param.Add("@ACCNT_FLAG", sAccntFlag, DbType.String, ParameterDirection.Input, sAccntFlag.Length);

                    (AccountInfo1, SpResult spResult) = await salesTmPrRcppayService.GetAccountInfo(param);

                    if (spResult.ResultType != EResultType.SUCCESS)
                    {
                        //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                        return;
                    }

                    if (AccountInfo1.Count > 0)
                    {
                        GetAccountInfoSet1(1, sAccntFlag);
                    }

                }
                else
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                    param.Add("@ACCNT_FLAG", sAccntFlag, DbType.String, ParameterDirection.Input, sAccntFlag.Length);

                    (AccountInfo2, SpResult spResult) = await salesTmPrRcppayService.GetAccountInfo(param);

                    if (spResult.ResultType != EResultType.SUCCESS)
                    {
                        //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                        return;
                    }

                    if (AccountInfo2.Count > 0)
                    {
                        GetAccountInfoSet2(1, sAccntFlag);
                    }
                }

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("시재입출금 List 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetAccountInfoSet1(int iSelected, string sAccntFlag)
        {

            totalPage1 = 0;

            if (AccountInfo1.Count > 0)
            {
                totalPage1 = AccountInfo1.Count / pageCnt1;
                int remainCnt = AccountInfo1.Count % pageCnt1;

                if (remainCnt > 0)
                {
                    totalPage1 = totalPage1 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage1 <= totalPage1)
            {
                int fromCnt = ((currentPage1 - 1) * 6);
                int toCnt = fromCnt + 6;

                if (toCnt > AccountInfo1.Count - 1)
                {
                    toCnt = AccountInfo1.Count;
                }

                //POSITION_NO 1~5번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        BtnSet(AccountInfo1[i].ACCNT_CODE
                                , AccountInfo1[i].ACCNT_NAME
                                , iCnt
                                , sAccntFlag
                                , i
                                );
                    }
                    else
                    {
                        BtnEmptySet(AccountInfo1[i].ACCNT_CODE
                                , AccountInfo1[i].ACCNT_NAME
                                , iCnt
                                , sAccntFlag
                                , i
                                );

                    }
                }

                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 6)
                {
                    for (int i = toCnt - fromCnt; i < 6; i++)
                    {
                        BtnEmptySet(""
                                , ""
                                , i + 1
                                , sAccntFlag
                                , i
                                );

                    }
                }

            }
        }

        private void GetAccountInfoSet2(int iSelected, string sAccntFlag)
        {

            totalPage2 = 0;

            if (AccountInfo2.Count > 0)
            {
                totalPage2 = AccountInfo2.Count / pageCnt2;
                int remainCnt = AccountInfo2.Count % pageCnt2;

                if (remainCnt > 0)
                {
                    totalPage2 = totalPage2 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage2 <= totalPage2)
            {
                int fromCnt = ((currentPage2 - 1) * 6);
                int toCnt = fromCnt + 6;

                if (toCnt > AccountInfo2.Count - 1)
                {
                    toCnt = AccountInfo2.Count;
                }

                //POSITION_NO 1~5번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        BtnSet(AccountInfo2[i].ACCNT_CODE
                                , AccountInfo2[i].ACCNT_NAME
                                , iCnt
                                , sAccntFlag
                                , i
                                );
                    }
                    else
                    {
                        BtnEmptySet(AccountInfo2[i].ACCNT_CODE
                                , AccountInfo2[i].ACCNT_NAME
                                , iCnt
                                , sAccntFlag
                                , i
                                );

                    }


                }

                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 6)
                {
                    for (int i = toCnt - fromCnt; i < 6; i++)
                    {
                        BtnEmptySet(""
                                , ""
                                , i + 1
                                , sAccntFlag
                                , i
                                );

                    }
                }

            }
        }

        private void BtnEmptySet(string sCODE, string sNAME, int iCnt, string sAccntFlag, int index)
        {
            Button btn = new Button();
            TextBlock txt = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            double dCnt = double.Parse(iCnt.ToString());
            int iRowCnt = (int)Math.Truncate((dCnt - 1) / 6);
            int iColCnt = iCnt % 6;

            if (iColCnt == 0)
                iColCnt = 6;

            sRow = 0; //0*2+1 1,3,5
            sCol = (2 * iColCnt) - 1; //홀수

            Grid.SetRow(btn, sRow);//0 1 
            Grid.SetColumn(btn, sCol);
            btn.Name = "btnEmpty_" + iCnt.ToString() + "_" + sAccntFlag;
            btn.Tag = iCnt.ToString() + "|" + sCODE + "|" + sAccntFlag + "|" + index.ToString();
            btn.Click += btn_Click;
            btn.Margin = new Thickness(0, 2, 0, 0);
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Width = 85;
            btn.Height = 47;
            btn.BorderThickness = new Thickness(2);
            btn.Padding = new Thickness(0);
            btn.BorderBrush = new SolidColorBrush(Colors.White);
            btn.Foreground = null;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btn.Resources.Add(typeof(Border), borderStyle);

            // Button Background 설정
            LinearGradientBrush myButtonBackground = new LinearGradientBrush();
            myButtonBackground.StartPoint = new Point(0, 1);
            myButtonBackground.EndPoint = new Point(0, 0);
            myButtonBackground.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            myButtonBackground.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btn.Background = myButtonBackground;


            txt.Text = sNAME;
            txt.Name = "txtEmpty_" + iCnt.ToString();
            //txt.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txt.FontSize = 14;
            txt.FontStyle = System.Windows.FontStyles.Normal;
            txt.FontWeight = System.Windows.FontWeights.Bold;
            txt.Foreground = System.Windows.Media.Brushes.Black;
            txt.LineHeight = 14;
            txt.TextAlignment = TextAlignment.Left;

            //btn.Content = txt;

            //if (sAccntFlag == "0")
            //    IoC.Get<SalesTmPrRcppayView>().form.Children.Add(btn);
            //else
            //    IoC.Get<SalesTmPrRcppayView>().form2.Children.Add(btn);
        }

        private void BtnSet(string sCODE, string sNAME, int iCnt, string sAccntFlag, int index)
        {
            Button btn = new Button();
            TextBlock txt = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            double dCnt = double.Parse(iCnt.ToString());
            int iRowCnt = (int)Math.Truncate((dCnt - 1) / 6);
            int iColCnt = iCnt % 6;

            if (iColCnt == 0)
                iColCnt = 6;

            sRow = 0; //0*2+1 1,3,5
            sCol = (2 * iColCnt) - 1; //홀수


            Grid.SetRow(btn, sRow);//0 1 
            Grid.SetColumn(btn, sCol);

            btn.Name = "btn_" + iCnt.ToString();
            btn.Tag = iCnt.ToString() + "|" + sCODE + "|" + sAccntFlag + "|" + index.ToString();
            btn.Click += btn_Click;
            btn.Margin = new Thickness(0, 2, 0, 0);
            btn.VerticalAlignment = VerticalAlignment.Top;
            btn.Width = 85;
            btn.Height = 47;
            btn.BorderThickness = new Thickness(2);
            btn.Padding = new Thickness(0);
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(96, 127, 49)); // #607f31 색상과 동일한 색상 설정

            // Button Background 설정
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            radialGradientBrush.RadiusX = 1;
            radialGradientBrush.RadiusY = 1;
            radialGradientBrush.Center = new Point(0.5, 0.5);
            radialGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(175, 214, 85), 0)); // #AFD655 색상과 동일한 색상 설정
            radialGradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(150, 186, 71), 1.13)); // #96BA47 색상과 동일한 색상 설정
            btn.Background = radialGradientBrush;

            txt.Text = sNAME;
            txt.Name = "txt_" + iCnt.ToString() + "_" + sAccntFlag;
            //txt.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txt.FontSize = 14;
            txt.FontStyle = System.Windows.FontStyles.Normal;
            txt.FontWeight = System.Windows.FontWeights.Bold;
            txt.Foreground = System.Windows.Media.Brushes.Black;
            txt.LineHeight = 14;
            txt.TextAlignment = TextAlignment.Left;

            btn.Content = txt;

            //if (sAccntFlag == "0")
            //    IoC.Get<SalesTmPrRcppayView>().form.Children.Add(btn);
            //else
            //    IoC.Get<SalesTmPrRcppayView>().form2.Children.Add(btn);
        }

        //4. 시재입출금 현황 GetInOutAccountList
        private async Task GetInOutAccountList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                //param.Add("@SALE_DATE", IoC.Get<SalesTmPrRcppayView>().txtSALE_DATE.Text.Replace("-",""), DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                param.Add("@POS_NO", DataLocals.AppConfig.PosInfo.PosNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.PosNo.Length);
                //param.Add("@REGI_SEQ", IoC.Get<SalesTmPrRcppayView>().txtREGI_SEQ.Text, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.REGI_SEQ.Length);
                //param.Add("@ACCNT_CODE", sAccntCode, DbType.String, ParameterDirection.Input, sAccntCode.Length);
                //param.Add("@ACCNT_FLAG", sAccntFlag, DbType.String, ParameterDirection.Input, sAccntFlag.Length);



                /*
                    WHERE PCI.SHOP_CODE = @SHOP_CODE
                      AND PCI.SALE_DATE = @SALE_DATE
                      AND PCI.POS_NO = @POS_NO
                      AND PCI.REGI_SEQ = @REGI_SEQ
                      AND PCI.ACCNT_CODE = @ACCNT_CODE
                      AND PCI.ACCNT_FLAG = @ACCNT_FLAG
                 */
                (InOutAccountList, SpResult spResult) = await salesTmPrRcppayService.GetInOutAccountList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                for (int i = 0; i < InOutAccountList.Count; i++)
                {
                    //No
                    InOutAccountList[i].NO = (i + 1).ToString();
                    //일시
                    InOutAccountList[i].INSERT_DT = ChgDataTime(InOutAccountList[i].INSERT_DT);
                    //계정명
                    //입금액
                    //출금액
                    if (InOutAccountList[i].ACCNT_FLAG == "0")
                    {
                        InOutAccountList[i].ACCNT_AMT_IN = Comma(InOutAccountList[i].ACCNT_AMT);
                        InOutAccountList[i].ACCNT_AMT_OUT = "0";
                    }
                    else
                    {
                        InOutAccountList[i].ACCNT_AMT_IN = "0";
                        InOutAccountList[i].ACCNT_AMT_OUT = Comma(InOutAccountList[i].ACCNT_AMT);
                    }

                    //판매원
                    //비고
                    //SHOP_CODE
                    //SALE_DATE
                    //POS_NO
                    //CSH_IO_SEQ
                }

                //IoC.Get<SalesTmPrRcppayView>().InOutAccountList.ItemsSource = InOutAccountList;

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox(ex.Message);
                LogHelper.Logger.Error("GetInOutAccountList 가져오기 오류 : " + ex.Message);
            }
        }


        #endregion

        //**----------------------------------------------------------------------------------

        #region Event

        #endregion

        //**----------------------------------------------------------------------------------

        public void ButtonUp1(object sender, RoutedEventArgs e)
        {
            if (currentPage1 <= 1)
            {
                currentPage1 = 1;
            }
            else
            {
                currentPage1 = currentPage1 - 1;
                //IoC.Get<SalesTmPrRcppayView>().form.Children.Clear();
                //3.버튼 리스트
                GetAccountInfoSet1(1, "0");
            }
        }

        public void ButtonDown1(object sender, RoutedEventArgs e)
        {
            if (currentPage1 >= totalPage1)
            {
                currentPage1 = totalPage1;
            }
            else
            {
                currentPage1 = currentPage1 + 1;
                //IoC.Get<SalesTmPrRcppayView>().form.Children.Clear();
                //3.버튼 리스트
                GetAccountInfoSet1(1, "0");
            }
        }

        public void ButtonUp2(object sender, RoutedEventArgs e)
        {
            if (currentPage2 <= 1)
            {
                currentPage2 = 1;
            }
            else
            {
                currentPage2 = currentPage2 - 1;
                //IoC.Get<SalesTmPrRcppayView>().form2.Children.Clear();
                //3.버튼 리스트
                GetAccountInfoSet2(1, "1");
            }
        }

        public void ButtonDown2(object sender, RoutedEventArgs e)
        {
            if (currentPage2 >= totalPage2)
            {
                currentPage2 = totalPage2;
            }
            else
            {
                currentPage2 = currentPage2 + 1;
                //IoC.Get<SalesTmPrRcppayView>().form2.Children.Clear();
                //3.버튼 리스트
                GetAccountInfoSet2(1, "1") ;
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            //btnBase.Tag = "MENU_SUB|" + sCode + "|" + sClassCode + "|" + sMenuNm + "|" + sPrice;

            string sBtnName = (string)btn.Tag;

            if (sBtnName != "")
            {
                string[] sValue = sBtnName.Split('|');

                string sSelected = sValue[0];
                string sAccntCode = sValue[1];
                string sAccntFlag = sValue[2];
                int index = Convert.ToInt32(sValue[3]);

                if (sAccntFlag == "0")
                {
                    //IoC.Get<SalesTmPrRcppayView>().form.Children.Clear();
                    //3.버튼 리스트
                    GetAccountInfoSet1(Convert.ToInt16(sSelected), sAccntFlag);

                    if (sAccntCode != "")
                    {
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG.Text = sAccntFlag;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG_NAME.Text = "입금";
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_CODE.Text = AccountInfo1[index].ACCNT_CODE;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_NAME.Text = AccountInfo1[index].ACCNT_NAME;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_AMT.Text = AccountInfo1[index].ACCNT_AMT;
                        //IoC.Get<SalesTmPrRcppayView>().txtREMARK.Text = AccountInfo1[index].REMARK;
                    }
                }
                else
                {
                    //IoC.Get<SalesTmPrRcppayView>().form2.Children.Clear();
                    //3.버튼 리스트
                    GetAccountInfoSet2(Convert.ToInt16(sSelected), sAccntFlag);

                    if (sAccntCode != "")
                    {
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG.Text = sAccntFlag;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_FLAG_NAME.Text = "출금";
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_CODE.Text = AccountInfo2[index].ACCNT_CODE;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_NAME.Text = AccountInfo2[index].ACCNT_NAME;
                        //IoC.Get<SalesTmPrRcppayView>().txtACCNT_AMT.Text = AccountInfo2[index].ACCNT_AMT;
                        //IoC.Get<SalesTmPrRcppayView>().txtREMARK.Text = AccountInfo2[index].REMARK;
                    }
                }

            }

            //if (sBtnName == "OrderPayCashViewModel")
            //{
            //    ActiveItem = IoC.Get<OrderPayCashViewModel>();
            //}
            //MST_INFO_EMP data = new MST_INFO_EMP();

            //ShellViewModel.menu_nm = "OrderPayMainViewModel";//어느 페이지로 이동할지 셋팅
            //ShellViewModel.menu_nm_sub1 = "OrderPayCashViewModel";//어느 페이지로 이동할지 셋팅

            //_eventAggregator.PublishOnUIThreadAsync(data);

        }

        //등록 삭제 인쇄
        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }
            string sValue = (string)btn.Tag;
            switch (sValue)
            {
                case EventCode.Calendar:
                    OnCalendaShow(sender,e);
                    break;
                case EventCode.Delete:
                    OnDelete(sender, e);
                    break;
                case EventCode.Insert:
                    OnInsert(sender, e);
                    break;
                case EventCode.Search:
                    OnSearch(sender, e);
                    break;
                default:
                    break;
            }
        }
        private async void Delete(int iSelectedIndex)
        {
            try
            {
                /*
                 * SP_POS_CASH_IO_D
                   SHOP_CODE VARCHAR(6),
                   SALE_DATE VARCHAR(8),
                   POS_NO VARCHAR(2),
                   CSH_IO_SEQ VARCHAR(3),
                   EMP_NO VARCHAR(4))
                 */


                SHOP_ACCOUNT result = new SHOP_ACCOUNT();
                DynamicParameters param = new();
                param.Add("@SHOP_CODE", InOutAccountList[iSelectedIndex].SHOP_CODE);
                param.Add("@SALE_DATE", InOutAccountList[iSelectedIndex].SALE_DATE);
                param.Add("@POS_NO", InOutAccountList[iSelectedIndex].POS_NO);
                param.Add("@CSH_IO_SEQ", InOutAccountList[iSelectedIndex].CSH_IO_SEQ);
                param.Add("@EMP_NO", InOutAccountList[iSelectedIndex].EMP_NO);

                (result, SpResult spResult) = await salesTmPrRcppayService.DeleteInOutAccount(param);


                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    LogHelper.Logger.Error("Insert" + spResult.ResultType);
                    return;
                }

                await GetInOutAccountList();

            }
            catch (Exception a)
            {
                //Console.WriteLine(a.Message);
                LogHelper.Logger.Error(a.Message);
            }
        }
    }
}