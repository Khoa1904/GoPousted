using Caliburn.Micro;
using Dapper;
using GoPOS.Helpers;
using GoPOS.Helpers.CommandHelper;
using GoPOS.Models;
using GoPOS.Models.Common;
using GoPOS.Services;

using GoPOS.Views;
using GoShared.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;


/*
 주문 > 확장메뉴 > 주문접수

 */

namespace GoPOS.ViewModels
{

    public class OrderPayReceiptViewModel : Screen
    {
        private readonly ICommonService commonService;

        List<MST_COMM_CODE_SHOP> CustomerList = new List<MST_COMM_CODE_SHOP>();
        List<SHOP_KITCHEN_MEMO> ReqMemoList = new List<SHOP_KITCHEN_MEMO>();
        List<MST_INFO_EMP> TableOrderList = new List<MST_INFO_EMP>();

        private IEventAggregator _eventAggregator;

        private int currentPage1 = 1;
        private int totalPage1 = 0;
        private int pageCnt1 = 3;

        private int currentPage2 = 1;
        private int totalPage2 = 0;
        private int pageCnt2 = 5;


        public OrderPayReceiptViewModel(IEventAggregator eventAggregator, ICommonService service)
        {
            commonService = service;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            //_activeItem = (object)"";
            //ActiveItem = IoC.Get<OrderPayCalDlvrViewModel>();

            Init();
        }

        //private object _activeItem;
        //public object ActiveItem
        //{
        //    get { return _activeItem; }
        //    set
        //    {
        //        _activeItem = value;
        //        NotifyOfPropertyChange(() => ActiveItem);
        //    }
        //}

        private async void Init()
        {
            //1.고객수
            await GetCustomerList();
            //2.요청사항
            await GetReqMemoList();
            //3.테이블 주문자
            await GetTableOrderList();
        }

        #region 고객수 GetCustomerList
        private async Task GetCustomerList()
        {
            try
            {

                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                param.Add("@COM_CODE_FLAG", "204", DbType.String, ParameterDirection.Input, 3);
                //param.Add("@ORDER_NO", DataLocals.PosStatus.ORDER_NO, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.ORDER_NO.Length);

                (CustomerList, SpResult spResult) = await commonService.GetCommonList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }


                if (CustomerList.Count > 0)
                {
                    GetCustomerListSet(1);
                }

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox(ex.Message);
                LogHelper.Logger.Error("배달리스트 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetCustomerListSet(int iSelected)
        {
            IoC.Get<OrderPayReceiptView>().formCustomer.Children.Clear();

            for (int i = 0; i < CustomerList.Count; i++)
            {
                CustomerBtnSet(CustomerList[i].COM_CODE
                        , CustomerList[i].COM_CODE_NAME
                        , i + 1
                        );
            }

            //비어있는 버튼
            if (CustomerList.Count > 0 && CustomerList.Count < 4)
            {
                for (int i = CustomerList.Count; i < 4; i++)
                {
                    CustomerBtnSet(""
                            , ""
                            , i + 1
                            );
                }
            }
        }


        private void CustomerBtnSet(string sCODE, string sNAME, int iCnt)
        {
            Button btnCustomer = new Button();
            TextBlock txtCustomer = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = 2 * iCnt - 1;

            Grid.SetRow(btnCustomer, sRow);//0 1 
            Grid.SetColumn(btnCustomer, sCol);

            btnCustomer.Name = "btnCustomer_" + iCnt.ToString();
            btnCustomer.Tag = sCODE;
            btnCustomer.Click += btnCustomer_Click;
            btnCustomer.Width = 90;
            btnCustomer.Height = 47;
            btnCustomer.Margin = new Thickness(0, 4, 0, 0);
            btnCustomer.Padding = new Thickness(0);
            btnCustomer.VerticalAlignment = VerticalAlignment.Top;
            btnCustomer.BorderThickness = new Thickness(2);
            //btnCustomer.IsEnabled = btnEnable;

            btnCustomer.Foreground = null;
            btnCustomer.BorderBrush = Brushes.White;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btnCustomer.Resources.Add(typeof(Border), borderStyle);

            // Button.Background 생성 및 설정
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 1);
            gradientBrush.EndPoint = new Point(0, 0);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btnCustomer.Background = gradientBrush;

            txtCustomer.Text = sNAME;
            txtCustomer.Name = "txtCustomer_" + iCnt.ToString();
            //txtCustomer.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtCustomer.FontSize = 14;
            txtCustomer.FontStyle = FontStyles.Normal;
            txtCustomer.FontWeight = FontWeights.Bold;
            txtCustomer.Foreground = Brushes.Black;
            txtCustomer.LineHeight = 14;
            txtCustomer.TextAlignment = TextAlignment.Center;

            btnCustomer.Content = txtCustomer;

            IoC.Get<OrderPayReceiptView>().formCustomer.Children.Add(btnCustomer);
        }

        private void btnCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            //btnBase.Tag = "MENU_SUB|" + sCode + "|" + sClassCode + "|" + sMenuNm + "|" + sPrice;

            string sBtnTag = (string)btn.Tag;
            string sBtnName = btn.Name.Split('_')[1];

            if (sBtnTag != null && sBtnTag != "")
            {
                if (sBtnName == "1")
                    IoC.Get<OrderPayReceiptView>().txtCustomer1.Text = (ChgNum(IoC.Get<OrderPayReceiptView>().txtCustomer1.Text) + 1).ToString();
                else if (sBtnName == "2")
                    IoC.Get<OrderPayReceiptView>().txtCustomer2.Text = (ChgNum(IoC.Get<OrderPayReceiptView>().txtCustomer2.Text) + 1).ToString();
                else if (sBtnName == "3")
                    IoC.Get<OrderPayReceiptView>().txtCustomer3.Text = (ChgNum(IoC.Get<OrderPayReceiptView>().txtCustomer3.Text) + 1).ToString();
                else if (sBtnName == "4")
                    IoC.Get<OrderPayReceiptView>().txtCustomer4.Text = (ChgNum(IoC.Get<OrderPayReceiptView>().txtCustomer4.Text) + 1).ToString();
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
        #endregion

        #region 요청사항 GetReqMemoList
        private async Task GetReqMemoList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);

                (ReqMemoList, SpResult spResult) = await commonService.GetKitchenMemoList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                if (ReqMemoList.Count > 0)
                {
                    GetReqMemoListSet(1);
                }


                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox("배달리스트 가져오기 오류" + ex.Message);
                LogHelper.Logger.Error("배달리스트 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetReqMemoListSet(int iSelected)
        {
            ReqMemoClear();

            totalPage1 = 0;

            if (ReqMemoList.Count > 0)
            {
                totalPage1 = ReqMemoList.Count / pageCnt1;
                int remainCnt = ReqMemoList.Count % pageCnt1;

                if (remainCnt > 0)
                {
                    totalPage1 = totalPage1 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage1 <= totalPage1)
            {
                int fromCnt = (currentPage1 - 1) * 3;
                int toCnt = fromCnt + 3;

                if (toCnt > ReqMemoList.Count - 1)
                {
                    toCnt = ReqMemoList.Count;
                }

                //POSITION_NO 1~5번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        ReqMemoBtnSet(ReqMemoList[i].MEMO_CODE
                                , ReqMemoList[i].MEMO_NAME
                                , iCnt
                                );
                    }
                    else
                    {
                        ReqMemoBtnEmptySet(ReqMemoList[i].MEMO_CODE
                                , ReqMemoList[i].MEMO_NAME
                                , iCnt
                                );

                    }
                }


                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 9)
                {
                    for (int i = toCnt - fromCnt; i < 9; i++)
                    {
                        ReqMemoBtnEmptySet(""
                                , ""
                                , i + 1
                                );

                    }
                }
            }
        }

        private void ReqMemoBtnEmptySet(string sCODE, string sNAME, int iCnt)
        {
            Button btnReqMemo = new Button();
            TextBlock txtReqMemo = new TextBlock();

            int sRow = 0; //1, 3, 5, 7
            int sCol = 0;

            double dCnt = double.Parse(iCnt.ToString());
            int iRowCnt = (int)Math.Truncate((dCnt - 1) / 3);
            int iColCnt = iCnt % 3; //1-1 2-3 3-5 4-1 5-3

            if (iColCnt == 0)
                iColCnt = 3;

            sRow = iRowCnt + 1; //0*2+1 1,3,5
            sCol = 2 * iColCnt - 1; //홀수

            Grid.SetRow(btnReqMemo, sRow);//0 1 
            Grid.SetColumn(btnReqMemo, sCol);

            btnReqMemo.Name = "btnReqMemoEmpty_" + iCnt.ToString();
            btnReqMemo.Tag = sCODE;
            btnReqMemo.Click += btnReqMemo_Click;
            btnReqMemo.Width = 122;
            btnReqMemo.Height = 47;
            btnReqMemo.Margin = new Thickness(0, 4, 0, 0);
            btnReqMemo.Padding = new Thickness(0);
            btnReqMemo.BorderThickness = new Thickness(2);

            btnReqMemo.Foreground = null;
            btnReqMemo.BorderBrush = Brushes.White;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btnReqMemo.Resources.Add(typeof(Border), borderStyle);

            // Button.Background 생성 및 설정
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 1);
            gradientBrush.EndPoint = new Point(0, 0);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btnReqMemo.Background = gradientBrush;

            txtReqMemo.Text = sNAME;
            txtReqMemo.Name = "txtReqMemoEmpty_" + iCnt.ToString();
            ////txtReqMemo.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtReqMemo.FontSize = 14;
            txtReqMemo.FontStyle = FontStyles.Normal;
            txtReqMemo.FontWeight = FontWeights.Bold;
            txtReqMemo.Foreground = Brushes.Black;
            txtReqMemo.LineHeight = 14;
            txtReqMemo.TextAlignment = TextAlignment.Center;

            btnReqMemo.Content = txtReqMemo;

            IoC.Get<OrderPayReceiptView>().formReqMemo.Children.Add(btnReqMemo);
        }

        private void ReqMemoBtnSet(string sCODE, string sNAME, int iCnt)
        {

            Button btnReqMemo = new Button();
            TextBlock txtReqMemo = new TextBlock();

            int sRow = 0; //1, 3, 5, 7
            int sCol = 0;

            double dCnt = double.Parse(iCnt.ToString());
            int iRowCnt = (int)Math.Truncate((dCnt - 1) / 3);
            int iColCnt = iCnt % 3; //1-1 2-3 3-5 4-1 5-3

            if (iColCnt == 0)
                iColCnt = 3;

            sRow = iRowCnt + 1; //0*2+1 1,3,5
            sCol = 2 * iColCnt - 1; //홀수

            Grid.SetRow(btnReqMemo, sRow);//0 1 
            Grid.SetColumn(btnReqMemo, sCol);

            btnReqMemo.Name = "btnReqMemoEmpty_" + iCnt.ToString();
            btnReqMemo.Tag = sCODE;
            btnReqMemo.Click += btnReqMemo_Click;
            btnReqMemo.Width = 122;
            btnReqMemo.Height = 47;
            btnReqMemo.Margin = new Thickness(0, 4, 0, 0);
            btnReqMemo.Padding = new Thickness(0);

            btnReqMemo.Foreground = null;
            btnReqMemo.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
            btnReqMemo.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            borderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(2)));
            btnReqMemo.Resources.Add(typeof(Border), borderStyle);

            txtReqMemo.Text = sNAME;
            txtReqMemo.Name = "txtReqMemoEmpty_" + iCnt.ToString();
            //txtReqMemo.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtReqMemo.FontSize = 14;
            txtReqMemo.FontStyle = FontStyles.Normal;
            txtReqMemo.FontWeight = FontWeights.Bold;
            txtReqMemo.Foreground = Brushes.Black;
            txtReqMemo.LineHeight = 14;
            txtReqMemo.TextAlignment = TextAlignment.Center;

            btnReqMemo.Content = txtReqMemo;

            IoC.Get<OrderPayReceiptView>().formReqMemo.Children.Add(btnReqMemo);

        }

        public void ButtonUp1(object sender, RoutedEventArgs e)
        {
            if (currentPage1 <= 1)
            {
                currentPage1 = 1;
            }
            else
            {
                currentPage1 = currentPage1 - 1;
                //3.버튼 리스트
                GetReqMemoListSet(1);
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
                //3.버튼 리스트
                GetReqMemoListSet(1);
            }
        }

        private void btnReqMemo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            //btnBase.Tag = "MENU_SUB|" + sCode + "|" + sClassCode + "|" + sMenuNm + "|" + sPrice;

            string sBtnTag = (string)btn.Tag;
            string sBtnName = btn.Name;

            if (sBtnTag != null && sBtnTag != "")
            {
                //3.버튼 리스트
                GetReqMemoListSet(Convert.ToInt16(sBtnName.Split('_')[1]));
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

        private void ReqMemoClear()
        {
            IEnumerable<Button> buttons = IoC.Get<OrderPayReceiptView>().formReqMemo.Children.OfType<Button>();

            foreach (Button button in buttons.ToList())
            {
                IoC.Get<OrderPayReceiptView>().formReqMemo.Children.Remove(button);
            }
        }
        #endregion

        #region 테이블 주문자 GetTableOrderList
        private async Task GetTableOrderList()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);

                (TableOrderList, SpResult spResult) = await commonService.GetEmpList(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                if (TableOrderList.Count > 0)
                {
                    GetTableOrderListSet(1);
                }

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                DialogHelper.MessageBox(ex.Message);
                LogHelper.Logger.Error("배달리스트 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetTableOrderListSet(int iSelected)
        {

            totalPage2 = 0;

            if (TableOrderList.Count > 0)
            {
                totalPage2 = TableOrderList.Count / pageCnt2;

                if (TableOrderList.Count > 0)
                {
                    totalPage2 = totalPage2 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage2 <= totalPage2)
            {
                int fromCnt = (currentPage2 - 1) * 5;
                int toCnt = fromCnt + 5;

                if (toCnt > TableOrderList.Count - 1)
                {
                    toCnt = TableOrderList.Count;
                }

                //POSITION_NO 1~5번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        TableOrderBtnSet(TableOrderList[i].EMP_NO
                                , TableOrderList[i].EMP_NAME
                                , iCnt
                                );
                    }
                    else
                    {
                        TableOrderBtnEmptySet(TableOrderList[i].EMP_NO
                                , TableOrderList[i].EMP_NAME
                                , iCnt
                                );

                    }
                }

                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 9)
                {
                    for (int i = toCnt - fromCnt; i < 9; i++)
                    {
                        TableOrderBtnEmptySet(""
                                , ""
                                , iCnt + 1
                                );

                    }
                }

            }
        }

        private void TableOrderBtnEmptySet(string sEMP_NO, string sEMP_NAME, int iCnt)
        {

            Button btnTableOrder = new Button();
            TextBlock txtTableOrder = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = 2 * iCnt - 1;

            Grid.SetRow(btnTableOrder, sRow);//0 1 
            Grid.SetColumn(btnTableOrder, sCol);

            btnTableOrder.Name = "btnTableOrder_" + iCnt.ToString();
            btnTableOrder.Tag = sEMP_NO;
            btnTableOrder.Click += btnTableOrder_Click;
            btnTableOrder.Width = 90;
            btnTableOrder.Height = 47;
            btnTableOrder.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder.Padding = new Thickness(0);
            btnTableOrder.VerticalAlignment = VerticalAlignment.Top;
            btnTableOrder.BorderThickness = new Thickness(2);

            btnTableOrder.Foreground = null;
            btnTableOrder.BorderBrush = Brushes.White;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btnTableOrder.Resources.Add(typeof(Border), borderStyle);

            // Button.Background 생성 및 설정
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 1);
            gradientBrush.EndPoint = new Point(0, 0);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btnTableOrder.Background = gradientBrush;

            txtTableOrder.Text = sEMP_NAME;
            txtTableOrder.Name = "txtTableOrder_" + iCnt.ToString();
            //txtTableOrder.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder.FontSize = 14;
            txtTableOrder.FontStyle = FontStyles.Normal;
            txtTableOrder.FontWeight = FontWeights.Bold;
            txtTableOrder.Foreground = Brushes.Black;
            txtTableOrder.LineHeight = 14;
            txtTableOrder.TextAlignment = TextAlignment.Center;

            btnTableOrder.Content = txtTableOrder;

            IoC.Get<OrderPayReceiptView>().formTableOrder.Children.Add(btnTableOrder);

        }

        private void TableOrderBtnSet(string sEMP_NO, string sEMP_NAME, int iCnt)
        {
            Button btnTableOrder = new Button();
            TextBlock txtTableOrder = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = 2 * iCnt - 1;

            Grid.SetRow(btnTableOrder, sRow);//0 1 
            Grid.SetColumn(btnTableOrder, sCol);

            btnTableOrder.Name = "btnTableOrder_" + iCnt.ToString();
            btnTableOrder.Tag = sEMP_NO;
            btnTableOrder.Click += btnTableOrder_Click;
            btnTableOrder.Width = 90;
            btnTableOrder.Height = 47;
            btnTableOrder.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder.Padding = new Thickness(0);
            btnTableOrder.VerticalAlignment = VerticalAlignment.Top;

            btnTableOrder.Foreground = null;
            btnTableOrder.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
            btnTableOrder.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            borderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(2)));
            btnTableOrder.Resources.Add(typeof(Border), borderStyle);

            txtTableOrder.Text = sEMP_NAME;
            txtTableOrder.Name = "txtTableOrder_" + iCnt.ToString();
            //txtTableOrder.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder.FontSize = 14;
            txtTableOrder.FontStyle = FontStyles.Normal;
            txtTableOrder.FontWeight = FontWeights.Bold;
            txtTableOrder.Foreground = Brushes.Black;
            txtTableOrder.LineHeight = 14;
            txtTableOrder.TextAlignment = TextAlignment.Center;

            btnTableOrder.Content = txtTableOrder;

            IoC.Get<OrderPayReceiptView>().formTableOrder.Children.Add(btnTableOrder);
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
                IoC.Get<OrderPayReceiptView>().formTableOrder.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet(1);
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
                IoC.Get<OrderPayReceiptView>().formTableOrder.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet(1);
            }
        }

        private void btnTableOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            //btnBase.Tag = "MENU_SUB|" + sCode + "|" + sClassCode + "|" + sMenuNm + "|" + sPrice;

            string sBtnTag = (string)btn.Tag;
            string sBtnName = btn.Name;

            if (sBtnTag != null && sBtnTag != "")
            {
                IoC.Get<OrderPayReceiptView>().formTableOrder.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet(Convert.ToInt16(sBtnName.Split('_')[1]));

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
        #endregion

    }
}