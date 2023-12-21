using Caliburn.Micro;
using CoreWCF.Runtime;
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
using System.Web.Services.Description;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static GoPOS.Function;

/*
 영업관리 > 상품권판매
 */
/// <summary>
/// 화면명 : 상품권판매 - 110 
/// 작성자 : 김형석
/// 작성일 : 20230319
/// </summary>
namespace GoPOS.ViewModels
{

    public class SalesGiftSaleViewModel : Screen
    {
        private readonly ISalesGiftSaleService _salesGiftSaleService;

        //List<MST_COMM_CODE_SHOP> CustomerList = new List<MST_COMM_CODE_SHOP>();
        List<SALES_GIFT_SALE> pSALES_GIFT_SALE = new List<SALES_GIFT_SALE>();

        List<SALES_GIFT_SALE> pGIFTList1 = new List<SALES_GIFT_SALE>(); // 상단
        List<SALES_GIFT_SALE> pGIFTList2 = new List<SALES_GIFT_SALE>(); // 하단
        // 농심 상품권
        // 만원 상품권

        // 금액 할인 상품권 
        // 2월 500원 상품권 2월  //1,000월 상품권

        //테스트 상품권
        //10,000원 권 / 30,000원 권

        //List<SALES_GIFT_SALE> pSALES_GIFT_SALE = new List<SALES_GIFT_SALE>();

        private int currentPage1 = 1;
        private int totalPage1 = 0;
        private int pageCnt1 = 6;

        private int currentPage2 = 1;
        private int totalPage2 = 0;
        private int pageCnt2 = 6;


        private IEventAggregator _eventAggregator;


        public SalesGiftSaleViewModel(IEventAggregator eventAggregator, ISalesGiftSaleService salesGiftSaleService)
        {
            this._salesGiftSaleService = salesGiftSaleService;

            this._eventAggregator = eventAggregator;
            this._eventAggregator.SubscribeOnPublishedThread(this);

            Init();
        }

        private async void Init()
        {

            //1.고객수
            //await GetCustomerList();
            //2.요청사항
            await GetTableOrderList1();
            //3.테이블 주문자
            await GetTableOrderList2();

            //OnSelectedItemChanged(null);
            await Task.Delay(100);
        }

        #region 요청사항 GetReqMemoList
        private async Task GetTableOrderList1()
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);

                IoC.Get<SalesGiftSaleView>().Form1.Children.Clear();
                List<SALES_GIFT_SALE> pTmpGIFTList1 = new List<SALES_GIFT_SALE>(); // 상단

                (pTmpGIFTList1, SpResult spResult) = await _salesGiftSaleService.GetList1(param);
                //pGIFTList1.Clear();

                //pGIFTList1.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + iIndex).ToString(), TK_GFT_CODE = (1 + iIndex).ToString() + "0,000원 상품권" });
                for (int i = 0; i < 20; i++)
                    pGIFTList1.Add(pTmpGIFTList1[i]);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                if (pGIFTList1.Count > 0)
                {
                    GetTableOrderListSet1(1);
                }

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetTableOrderListSet1(int iSelected)
        {

            totalPage1 = 0;

            if (pGIFTList1.Count > 0)
            {
                totalPage1 = pGIFTList1.Count / pageCnt1;

                if (pGIFTList1.Count > 0)
                {
                    totalPage1 = totalPage1 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage1 <= totalPage1)
            {
                int fromCnt = ((currentPage1 - 1) * 6);
                int toCnt = fromCnt + 6;

                if (toCnt > pGIFTList1.Count - 1)
                {
                    toCnt = pGIFTList1.Count;
                }

                //POSITION_NO 1~6번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        TableOrderBtnSet1(pGIFTList1[i].NO
                                , pGIFTList1[i].TK_GFT_CODE
                                , iCnt
                                );
                    }
                    else
                    {
                        TableOrderBtnEmptySet1(pGIFTList1[i].NO
                                , pGIFTList1[i].TK_GFT_CODE
                                , iCnt
                                );

                    }
                }

                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 11)
                {
                    for (int i = toCnt - fromCnt; i < toCnt; i++)
                    {
                        TableOrderBtnEmptySet1(""
                                , ""
                                , iCnt + 1
                                );

                    }
                }

            }
        }

        private void TableOrderBtnEmptySet1(string sEMP_NO, string sEMP_NAME, int iCnt)
        {

            Button btnTableOrder1 = new Button();
            TextBlock txtTableOrder1 = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = (2 * iCnt) - 1;

            Grid.SetRow(btnTableOrder1, sRow);//0 1 
            Grid.SetColumn(btnTableOrder1, sCol);

            btnTableOrder1.Name = "btnTableOrder1_" + iCnt.ToString();
            btnTableOrder1.Tag = sEMP_NO;
            btnTableOrder1.Click += btnTableOrder1_Click;
            btnTableOrder1.Width = 90;
            btnTableOrder1.Height = 47;
            btnTableOrder1.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder1.Padding = new Thickness(0);
            btnTableOrder1.VerticalAlignment = VerticalAlignment.Top;
            btnTableOrder1.BorderThickness = new Thickness(2);

            btnTableOrder1.Foreground = null;
            btnTableOrder1.BorderBrush = Brushes.White;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btnTableOrder1.Resources.Add(typeof(Border), borderStyle);

            // Button.Background 생성 및 설정
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 1);
            gradientBrush.EndPoint = new Point(0, 0);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btnTableOrder1.Background = gradientBrush;

            txtTableOrder1.Text = sEMP_NAME;
            txtTableOrder1.Name = "txtTableOrder1_" + iCnt.ToString();
            //txtTableOrder1.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder1.FontSize = 14;
            txtTableOrder1.FontStyle = System.Windows.FontStyles.Normal;
            txtTableOrder1.FontWeight = System.Windows.FontWeights.Bold;
            txtTableOrder1.Foreground = Brushes.Black;
            txtTableOrder1.LineHeight = 14;
            txtTableOrder1.TextAlignment = TextAlignment.Center;

            btnTableOrder1.Content = txtTableOrder1;

            IoC.Get<SalesGiftSaleView>().Form1.Children.Add(btnTableOrder1);

        }

        private void TableOrderBtnSet1(string sEMP_NO, string sEMP_NAME, int iCnt)
        {
            Button btnTableOrder1 = new Button();
            TextBlock txtTableOrder1 = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = (2 * iCnt) - 1;

            Grid.SetRow(btnTableOrder1, sRow);//0 1 
            Grid.SetColumn(btnTableOrder1, sCol);

            btnTableOrder1.Name = "btnTableOrder1_" + iCnt.ToString();
            btnTableOrder1.Tag = sEMP_NO;
            btnTableOrder1.Click += btnTableOrder1_Click;
            btnTableOrder1.Width = 90;
            btnTableOrder1.Height = 47;
            btnTableOrder1.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder1.Padding = new Thickness(0);
            btnTableOrder1.VerticalAlignment = VerticalAlignment.Top;

            btnTableOrder1.Foreground = null;
            btnTableOrder1.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
            btnTableOrder1.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            borderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(2)));
            btnTableOrder1.Resources.Add(typeof(Border), borderStyle);

            txtTableOrder1.Text = sEMP_NAME;
            txtTableOrder1.Name = "txtTableOrder1_" + iCnt.ToString();
            //txtTableOrder1.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder1.FontSize = 14;
            txtTableOrder1.FontStyle = System.Windows.FontStyles.Normal;
            txtTableOrder1.FontWeight = System.Windows.FontWeights.Bold;
            txtTableOrder1.Foreground = Brushes.Black;
            txtTableOrder1.LineHeight = 14;
            txtTableOrder1.TextAlignment = TextAlignment.Center;

            btnTableOrder1.Content = txtTableOrder1;

            IoC.Get<SalesGiftSaleView>().Form1.Children.Add(btnTableOrder1);
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
                IoC.Get<SalesGiftSaleView>().Form1.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet1(1);
            }
        }

        public void ButtonDn1(object sender, RoutedEventArgs e)
        {
            if (currentPage1 >= totalPage1)
            {
                currentPage1 = totalPage1;
            }
            else
            {
                currentPage1 = currentPage1 + 1;
                IoC.Get<SalesGiftSaleView>().Form1.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet1(1);
            }
        }

        private async void btnTableOrder1_Click(object sender, RoutedEventArgs e)
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
                IoC.Get<SalesGiftSaleView>().form2.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet1(Convert.ToInt16(sBtnName.Split('_')[1]));
                await GetTableOrderList2(Convert.ToInt16(sBtnName.Split('_')[1]));
                GetTableOrderListSet2(Convert.ToInt16(1));

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

        #region 테이블 주문자 GetTableOrderList2
        private async Task GetTableOrderList2(int iIndex = 0)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@SHOP_CODE", DataLocals.AppConfig.PosInfo.StoreNo, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);

                (pGIFTList2, SpResult spResult) = await _salesGiftSaleService.GetList2(param);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }

                pGIFTList2.Clear();

                if (iIndex == 0)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        //result.Add(new PRODUCT_M() { NO = (i + 1).ToString(), PRD_CODE = "0000" + (i + 1).ToString(), PRD_NAME = "희얌케익_"+ (i + 1).ToString(), NORMAL_UPRC = (i + 1).ToString()+",000", SOLD_OUT_YN = "품절" });
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                    }
                }
                else if (iIndex == 1)
                {
                    for (int i = 0; i < 1; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }
                else if (iIndex == 2)
                {
                    for (int i = 0; i < 2; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }
                else if (iIndex == 3)
                {
                    for (int i = 0; i < 3; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }
                else if (iIndex == 4)
                {
                    for (int i = 0; i < 4; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }
                else if (iIndex == 5)
                {
                    for (int i = 0; i < 5; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }
                else
                {
                    for (int i = 0; i < 12; i++)
                        pGIFTList2.Add(new SALES_GIFT_SALE() { NO = "NAME_" + (1 + i).ToString(), TK_GFT_CODE = (1 + i).ToString() + "0,000원 상품권" });
                }

                if (pGIFTList2.Count > 0)
                {
                    GetTableOrderListSet2(1);
                }

                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("배달리스트 가져오기 오류 : " + ex.Message);
            }
        }

        private void GetTableOrderListSet2(int iSelected)
        {

            totalPage2 = 0;

            if (pGIFTList2.Count > 0)
            {
                totalPage2 = pGIFTList2.Count / pageCnt2;

                if (pGIFTList2.Count > 0)
                {
                    totalPage2 = totalPage2 + 1;
                }

            }

            int iCnt = 0;
            if (currentPage2 <= totalPage2)
            {
                int fromCnt = ((currentPage2 - 1) * 6);
                int toCnt = fromCnt + 6;

                if (toCnt > pGIFTList2.Count - 1)
                {
                    toCnt = pGIFTList2.Count;
                }

                //POSITION_NO 1~6번까지 셋팅 그이후 확장셋팅
                for (int i = fromCnt; i < toCnt; i++)
                {
                    iCnt++;
                    if (iCnt == iSelected)
                    {
                        TableOrderBtnSet2(pGIFTList2[i].NO
                                , pGIFTList2[i].TK_GFT_CODE
                                , iCnt
                                );
                    }
                    else
                    {
                        TableOrderBtnEmptySet2(pGIFTList2[i].NO
                                , pGIFTList2[i].TK_GFT_CODE
                                , iCnt
                                );

                    }
                }

                //비어있는 버튼
                if (toCnt - fromCnt > 0 && toCnt - fromCnt < 11)
                {
                    for (int i = toCnt - fromCnt; i < toCnt; i++)
                    {
                        TableOrderBtnEmptySet2(""
                                , ""
                                , iCnt + 1
                                );

                    }
                }

            }
        }

        private void TableOrderBtnEmptySet2(string sEMP_NO, string sEMP_NAME, int iCnt)
        {

            Button btnTableOrder2 = new Button();
            TextBlock txtTableOrder2 = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = (2 * iCnt) - 1;

            Grid.SetRow(btnTableOrder2, sRow);//0 1 
            Grid.SetColumn(btnTableOrder2, sCol);

            btnTableOrder2.Name = "btnTableOrder2_" + iCnt.ToString();
            btnTableOrder2.Tag = sEMP_NO;
            btnTableOrder2.Click += btnTableOrder2_Click;
            btnTableOrder2.Width = 90;
            btnTableOrder2.Height = 47;
            btnTableOrder2.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder2.Padding = new Thickness(0);
            btnTableOrder2.VerticalAlignment = VerticalAlignment.Top;
            btnTableOrder2.BorderThickness = new Thickness(2);

            btnTableOrder2.Foreground = null;
            btnTableOrder2.BorderBrush = Brushes.White;

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            btnTableOrder2.Resources.Add(typeof(Border), borderStyle);

            // Button.Background 생성 및 설정
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 1);
            gradientBrush.EndPoint = new Point(0, 0);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(249, 249, 249), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(234, 234, 234), 1));
            btnTableOrder2.Background = gradientBrush;

            txtTableOrder2.Text = sEMP_NAME;
            txtTableOrder2.Name = "txtTableOrder2_" + iCnt.ToString();
            //txtTableOrder2.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder2.FontSize = 14;
            txtTableOrder2.FontStyle = System.Windows.FontStyles.Normal;
            txtTableOrder2.FontWeight = System.Windows.FontWeights.Bold;
            txtTableOrder2.Foreground = Brushes.Black;
            txtTableOrder2.LineHeight = 14;
            txtTableOrder2.TextAlignment = TextAlignment.Center;

            btnTableOrder2.Content = txtTableOrder2;

            IoC.Get<SalesGiftSaleView>().form2.Children.Add(btnTableOrder2);

        }

        private void TableOrderBtnSet2(string sEMP_NO, string sEMP_NAME, int iCnt)
        {
            Button btnTableOrder2 = new Button();
            TextBlock txtTableOrder2 = new TextBlock();

            int sRow = 0;
            int sCol = 0;

            sCol = (2 * iCnt) - 1;

            Grid.SetRow(btnTableOrder2, sRow);//0 1 
            Grid.SetColumn(btnTableOrder2, sCol);

            btnTableOrder2.Name = "btnTableOrder2_" + iCnt.ToString();
            btnTableOrder2.Tag = sEMP_NO;
            btnTableOrder2.Click += btnTableOrder2_Click;
            btnTableOrder2.Width = 90;
            btnTableOrder2.Height = 47;
            btnTableOrder2.Margin = new Thickness(0, 4, 0, 0);
            btnTableOrder2.Padding = new Thickness(0);
            btnTableOrder2.VerticalAlignment = VerticalAlignment.Top;

            btnTableOrder2.Foreground = null;
            btnTableOrder2.BorderBrush = new SolidColorBrush(Color.FromRgb(29, 52, 3));
            btnTableOrder2.Background = new SolidColorBrush(Color.FromRgb(101, 139, 20));

            // Button.Resources에 스타일 추가
            Style borderStyle = new Style(typeof(Border));
            borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(2)));
            borderStyle.Setters.Add(new Setter(Border.BorderThicknessProperty, new Thickness(2)));
            btnTableOrder2.Resources.Add(typeof(Border), borderStyle);

            txtTableOrder2.Text = sEMP_NAME;
            txtTableOrder2.Name = "txtTableOrder2_" + iCnt.ToString();
            //txtTableOrder2.FontFamily = IoC.Get<OrderPayRightView>().txtMenuBase.FontFamily;
            txtTableOrder2.FontSize = 14;
            txtTableOrder2.FontStyle = System.Windows.FontStyles.Normal;
            txtTableOrder2.FontWeight = System.Windows.FontWeights.Bold;
            txtTableOrder2.Foreground = Brushes.Black;
            txtTableOrder2.LineHeight = 14;
            txtTableOrder2.TextAlignment = TextAlignment.Center;

            btnTableOrder2.Content = txtTableOrder2;

            IoC.Get<SalesGiftSaleView>().form2.Children.Add(btnTableOrder2);
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
                IoC.Get<SalesGiftSaleView>().form2.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet2(1);
            }
        }

        public void ButtonDn2(object sender, RoutedEventArgs e)
        {
            if (currentPage2 >= totalPage2)
            {
                currentPage2 = totalPage2;
            }
            else
            {
                currentPage2 = currentPage2 + 1;
                IoC.Get<SalesGiftSaleView>().form2.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet2(1);
            }
        }

        private async void btnTableOrder2_Click(object sender, RoutedEventArgs e)
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
                //IoC.Get<SalesGiftSaleView>().form2.Children.Clear();
                //3.버튼 리스트
                GetTableOrderListSet2(Convert.ToInt16(sBtnName.Split('_')[1]));
                GetList3(Convert.ToInt16(sBtnName.Split('_')[1]));
                //TableOrderList.Clear();
                //await GetTableOrderList2(Convert.ToInt16(sBtnName.Split('_')[1]));

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


        List<SALES_GIFT_SALE> tmpSALES_GIFT_SALE = new List<SALES_GIFT_SALE>();

        public async void GetList3(int iIndex)
        {
            // 선택된 항목 처리 로직 구현

            try
            {
                IoC.Get<SalesGiftSaleView>().PayList.Items.Refresh();

                DynamicParameters param = new DynamicParameters();
                //param.Add("@SHOP_CODE", selectedItem.SHOP_CODE, DbType.String, ParameterDirection.Input, DataLocals.AppConfig.PosInfo.StoreNo.Length);
                //param.Add("@SALE_DATE", selectedItem.SALE_DATE, DbType.String, ParameterDirection.Input, DataLocals.PosStatus.SALE_DATE.Length);
                //param.Add("@ORDER_NO", selectedItem.ORDER_NO, DbType.String, ParameterDirection.Input, selectedItem.ORDER_NO.Length);


                //tmpSALES_GIFT_SALE.Clear();

                (pSALES_GIFT_SALE, SpResult spResult) = await _salesGiftSaleService.GetList3(param, iIndex);

                if (spResult.ResultType != EResultType.SUCCESS)
                {
                    //WindowHelper.InfoMessage("사용자 List 가져오기", spResult.ResultMessage);
                    return;
                }


                //public string NORMAL_UPRC { get; set; } = string.Empty; //NUMERIC(10,2),
                //    public string SALE_QTY { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DC_AMT { get; set; } = string.Empty; //NUMERIC(12,2),
                //    public string DCM_SALE_AMT { get; set; } = string.Empty; //NUMERIC(12,2),

                for (int i = 0; i < pSALES_GIFT_SALE.Count; i++)
                {
                    //for (int i = 0; i < pSALES_GIFT_SALE.Count; i++)
                    //{
                    //동적버튼 생성 및 페이징 기능
                    //DriverList[i].EMP_NO
                    //DriverList[i].EMP_NAME

                    //상품권판매 NO
                    //상품권명 TK_GFT_CODE
                    //액면가 TK_GFT_UPRC
                    //단가 상품권-TK_GFT_UAMT
                    //할인 DC_RATE
                    //금액 TOT_TK_GFT_UAMT
                    //비고 REMARK

                }
                bool bFlag = false;
                double Tot = 0;
                for (int j = 0; j < tmpSALES_GIFT_SALE.Count; j++)
                {
                    if (tmpSALES_GIFT_SALE[j].TK_GFT_CODE == pSALES_GIFT_SALE[0].TK_GFT_CODE)
                    {
                        if (tmpSALES_GIFT_SALE[j].TK_GFT_UPRC == "")
                            tmpSALES_GIFT_SALE[j].TK_GFT_UPRC = "0";

                        Tot += CommaDel(tmpSALES_GIFT_SALE[j].TK_GFT_UPRC);

                        bFlag = true;
                    }
                    else
                    {
                        if (tmpSALES_GIFT_SALE[j].TK_GFT_UPRC == "")
                            tmpSALES_GIFT_SALE[j].TK_GFT_UPRC = "0";

                        Tot += CommaDel(tmpSALES_GIFT_SALE[j].TK_GFT_UPRC);

                    }
                }

                if (bFlag == false)
                {
                    tmpSALES_GIFT_SALE.Add(pSALES_GIFT_SALE[0]);

                    for (int i = 0; i < tmpSALES_GIFT_SALE.Count; i++)
                    {
                        tmpSALES_GIFT_SALE[i].NO = (i + 1).ToString();
                    }
                }

                IoC.Get<SalesGiftSaleView>().txtTotSaleAmt.Text = Comma(Tot);     // 총금액
                IoC.Get<SalesGiftSaleView>().txtTotDcAmt.Text = "0";       // 할인금액
                IoC.Get<SalesGiftSaleView>().txtTotExpPayAmt.Text = Comma(Tot);   // 받을금액
                IoC.Get<SalesGiftSaleView>().txtTotGstPayAmt.Text = "0";  // 받은금액
                IoC.Get<SalesGiftSaleView>().txtRetPayAmt.Text = "0";      // 거스름돈

                IoC.Get<SalesGiftSaleView>().PayList.ItemsSource = tmpSALES_GIFT_SALE;
                IoC.Get<SalesGiftSaleView>().PayList.Items.Refresh();

                //pSALES_GIFT_SALE[i].NO = (i + 1).ToString();
                //pSALES_GIFT_SALE[i].TK_GFT_CODE = pSALES_GIFT_SALE[i].TK_GFT_CODE;
                //pSALES_GIFT_SALE[i].TK_GFT_UPRC = Comma(pSALES_GIFT_SALE[i].TK_GFT_UPRC);
                //pSALES_GIFT_SALE[i].TK_GFT_UAMT = Comma(pSALES_GIFT_SALE[i].TK_GFT_UAMT);
                //pSALES_GIFT_SALE[i].DC_RATE = pSALES_GIFT_SALE[i].DC_RATE;
                //pSALES_GIFT_SALE[i].TOT_TK_GFT_UAMT = pSALES_GIFT_SALE[i].TOT_TK_GFT_UAMT;
                //pSALES_GIFT_SALE[i].REMARK = pSALES_GIFT_SALE[i].REMARK;


                //string jsonResult = JsonHelper.ModelToJson(resultList);

                //WindowHelper.InfoMessageRaw("조회결과", jsonResult.JsonPrettify());
                //LogHelper.Logger.Info("조회결과 : " + jsonResult);
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error("상품권 List 가져오기 오류 : " + ex.Message);
            }
        }

        public void ButtonMenu(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }


            //페이징1// 농심 상품권 / 금액 할인 상품권 // 테스트 상품권

            //페이징2// 농심 상품권 : 만원 상품권 / 금액 할인 상품권  ; 2월 500원 상품권 2월 1,000원 상품권 / 테스트 상품권 : 10,000 , 30,000원권

            //만원 상품권 -> 총금액 / 받을금액 각각 입력

            //하단 선택 // 전체 취소 할인 수량 변경 (우측 + 키 txt값 넣고) 적용 방식 + - 선택 상확에서 수량 변경  현금 처리 후 초기후 상태 유지

            //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");

            string sValue = (string)btn.Tag;
            if (sValue == "C")
            {
            }
        }

        public void ButtonClose(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
            {
                return;
            }

            string sValue = (string)btn.Tag;

            //IoC.Get<OrderPayMainView>().DockAllPop.Visibility = Visibility.Hidden;
            //IoC.Get<OrderPayMainViewModel>().ActiveForm("OrderPayRightViewModel");

        }
    }
}