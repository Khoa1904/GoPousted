using GoPOS.Common.Helpers;
using GoPOS.Common.Views;
using GoPOS.Common.Views.Controls;
using GoPOS.Interface;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoPOS.Models;
using GoPOS.Models.Custom.SellingStatus;
using GoPOS.ViewModels;
using static log4net.Appender.ColoredConsoleAppender;
using static System.Net.WebRequestMethods;
using Colors = System.Windows.Media.Colors;
using Caliburn.Micro;
using GoPOS.Common.Helpers.Controls;
using LiveCharts;
using static GoPOS.Function;


namespace GoPOS.Views
{
    /// <summary>
    /// MtSelngSttusView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MtSelngSttusView : UCViewBase, IMtSelngSttusView
    {

        private ChartValues<double> CharValues = new ChartValues<double>();
        private int[] Days = new int[42];

        public MtSelngSttusView()
        {
            InitializeComponent();
        }
        public void RenderCalendarDays(ObservableCollection<int> CalendarDays, List<SELLING_STATUS_INFO> infos, string date)
        {
            CharValues = new ChartValues<double>();
            Days = new int[43];


            var allDays = CalendarDays.ToArray();
            int Coords = 0;
            int Coords2 = 0;
            for(int i=1;i<allDays.Length+1; i++)
            {
                var day = CalendarDays[Coords];
                // Set Colspan , RowSpan
                int col = Coords % 7;
                int row = ((Coords / 7) * 2) + 1;
                var setData = CalendarGrid.FindVisualChildren<Button>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col);

                if (setData == null)
                {
                    continue;
                }

                TextBlock tb = setData.Content as TextBlock;
                Grid.SetColumnSpan(setData, 1);
                Grid.SetRowSpan(setData, 1);

                // Filter unbelonging days
                if ((Grid.GetRow(setData) == 1 && day > 8) || (Grid.GetRow(setData) == 2 && day > 8) || (Grid.GetRow(setData) == 11 && day < 14) || (Grid.GetRow(setData) == 9 && day < 14))
                {
                    tb.Tag = "NA";
                    tb.Text = day.ToString();
                    tb.Foreground = new SolidColorBrush(Colors.LightGray);
                }
                else    // Normal days
                {
                    tb.Tag = day.ToString();
                    tb.Text = day.ToString();
                    if (((Grid.GetColumn(setData) + 1) % 7 == 1))
                    {
                        tb.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else if (((Grid.GetColumn(setData) + 1) % 7 == 0))
                    {
                        tb.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        tb.Foreground = new SolidColorBrush(Color.FromRgb(70, 70, 70));
                    }
                }
                setData.Click += Last_Click;
                if (LastSelected == null)
                {
                    LastSelected = setData;
                    setData.Background = new SolidColorBrush(Colors.White);
                }
                Coords++;




                var setDtestata = CalendarGrid.FindVisualChildren<Border>();
                int col1 = Coords2 % 7;
                int row2 = ((Coords2 / 7) * 2) + 1;
                var xx = setDtestata.Where(x => (string)x.Tag == "Total");
                var setData2 = xx.First(e => Grid.GetRow(e) == row2 + 1 && Grid.GetColumn(e) == col1);
                var textBoxInBorder = setData2.FindVisualChildren<TextBlock>().FirstOrDefault();


                if (setData2 == null)
                {
                    continue;
                }
                
                TextBlock tb2 = textBoxInBorder;
                Grid.SetColumnSpan(setData2, 1);
                Grid.SetRowSpan(setData2, 1);
                // Filter unbelonging days
                if ((Grid.GetRow(setData2) == 1 && day > 8) || (Grid.GetRow(setData2) == 2 && day > 8) || (Grid.GetRow(setData2) == 12 && day < 14) || (Grid.GetRow(setData2) == 10 && day < 14))
                {
                    tb2.Tag = "N/A";
                    tb2.Text = "0";
                    Days[i] = 0;
                    tb2.Foreground = new SolidColorBrush(Colors.LightGray);
                }
                else    // Normal days
                {
                    var xxx= day < 10 ? $"0{day}" : day.ToString();
                    var Text = infos.FirstOrDefault(
                        x => x.SALE_DATE[^2..] == xxx.ToString());

                    if (Text != null)
                    {
                        tb2.Text = Text.TOT_SALE_AMT;
                        Days[i] = ChangeToNumber(Text.TOT_SALE_AMT);

                        string formattedDay = day < 10 ? $"0{day}" : day.ToString();
                        string result = $"{date}{formattedDay}";

                        tb2.Tag = result;
                        CharValues.Add(ChangeToNumber(Text.TOT_SALE_AMT));
                    }
                    else
                    {
                        Days[i] = 0;
                        CharValues.Add(0);

                        string formattedDay = day < 10 ? $"0{day}" : day.ToString();
                        string result = $"{date}{formattedDay}";
                        tb2.Tag = result;

                        tb2.Text = "0";
                    }


                    tb.Tag = day.ToString();

                    if (((Grid.GetColumn(setData2) + 1) % 7 == 1))
                    {
                        tb2.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else if (((Grid.GetColumn(setData2) + 1) % 7 == 0))
                    {
                        tb2.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        tb2.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }

                if (LastSelected == null)
                {
                    setData2.Background = new SolidColorBrush(Colors.White);
                }
                Coords2++;
            }


            var mtSelngSttusViewModel = IoC.Get<MtSelngSttusViewModel>();

            mtSelngSttusViewModel.CharValues = CharValues;
            CalculatePerWeek(mtSelngSttusViewModel);
        }


        private List<string> _oldText = new List<string>();

        private void CalculatePerWeek(MtSelngSttusViewModel? model)
        {
            model.DataModel = null;
            model.DataModel = new MtSelngSttusModel();
            model.DrawnGraph = new DrawnGraph();
            double Week1 = Days[1] + Days[2] + Days[3] + Days[4] + Days[5] +
                           Days[6] + Days[7];
            double Week2 = Days[8] + Days[9] + Days[10] + Days[11] + Days[12] +
                           Days[13] + Days[14];
            double Week3 = Days[15] + Days[16] + Days[17] + Days[18] + Days[19] +
                           Days[20] + Days[21];
            double Week4 = Days[22] + Days[23] + Days[24] + Days[25] + Days[26] +
                           Days[27] +Days[28];
            double Week5 = Days[29] + Days[30] + Days[31] + Days[32] + Days[33] +
                           Days[34] + Days[35];
            double Week6 = Days[36] + Days[37] + Days[38] + Days[39] + Days[40] +
                           Days[41] + Days[42];

            model.DrawnGraph.Week1 = new ChartValues<double>
            {
                Days[1], Days[2], Days[3], Days[4], Days[5],
                Days[6], Days[7]
            };

            model.DrawnGraph.Week2 = new ChartValues<double>
            {
                Days[8] , Days[9], Days[10], Days[11], Days[12],
                Days[13], Days[14]
        };

            model.DrawnGraph.Week3 = new ChartValues<double>
            {
                Days[15] , Days[16], Days[17], Days[18], Days[19],
                Days[20], Days[21]
            };

            model.DrawnGraph.Week4 = new ChartValues<double>
            {
                Days[22] , Days[23], Days[24], Days[25], Days[26],
                Days[27], Days[28]
            };

            model.DrawnGraph.Week5 = new ChartValues<double>
            {
                Days[29] , Days[30], Days[31], Days[32], Days[33],
                Days[34], Days[35]
            };

            model.DrawnGraph.Week6 = new ChartValues<double>
            {
                Days[36] , Days[37], Days[38], Days[39], Days[40],
                Days[41], Days[42]
            };



            model.DataModel.Week1 =
                $"{Week1:#,0}";
            model.DataModel.Week2 =
                $"{Week2:#,0}";
            model.DataModel.Week3 =
                $"{Week3:#,0}";
            model.DataModel.Week4 =
                $"{Week4:#,0}";
            model.DataModel.Week5 =
                $"{Week5:#,0}";
            model.DataModel.Week6 =
                $"{Week6:#,0}";


            double Sunday = Days[1] + Days[8] + Days[15] + Days[22] + Days[29] +
                            Days[36];
            double Monday = Days[2] + Days[9] + Days[16] + Days[23] + Days[30] +
                            Days[37];
            double Tuesday = Days[3] + Days[10] + Days[17] + Days[24] + Days[31] +
                             Days[38];
            double Wednesday = Days[4] + Days[11] + Days[18] + Days[25] +
                               Days[32] + Days[39];
            double Thursday = Days[5] + Days[12] + Days[19] + Days[26] +
                              Days[33] + Days[40];
            double Friday = Days[6] + Days[13] + Days[20] + Days[27] + Days[34] +
                            Days[41];
            double Saturday = Days[7] + Days[14] + Days[21] + Days[28] +
                              Days[34] + Days[42];


            model.DrawnGraph.Class1 = new ChartValues<double>
            {
                Days[1] , Days[8] , Days[15] , Days[22] , Days[29] ,
                Days[36]
            };

            model.DrawnGraph.Class2 = new ChartValues<double>
            {
                Days[2] , Days[9] , Days[16] , Days[23] , Days[30] ,
                Days[37]
            };

            model.DrawnGraph.Class3 = new ChartValues<double>
            {
                Days[3] , Days[10] , Days[17] , Days[24] , Days[31] ,
                Days[38]
            };

            model.DrawnGraph.Class4 = new ChartValues<double>
            {
                Days[4] , Days[11] , Days[18] , Days[25] ,
                Days[32] , Days[39]
            };

            model.DrawnGraph.Class5 = new ChartValues<double>
            {
                Days[5] , Days[12] , Days[19] , Days[26] ,
                Days[33] , Days[40]
            };

            model.DrawnGraph.Class6 = new ChartValues<double>
            {
                Days[6] , Days[13] , Days[20] , Days[27] , Days[34] ,
                Days[41]
            };

            model.DrawnGraph.Class7 = new ChartValues<double>
            {
                Days[7] , Days[14] , Days[21] , Days[28] ,
                Days[35] , Days[42]
            };


            model.DataModel.Sunday = $"{Sunday:#,0}";
            model.DataModel.Monday = $"{Monday:#,0}";
            model.DataModel.Tuesday = $"{Tuesday:#,0}";
            model.DataModel.Wednesday = $"{Wednesday:#,0}";
            model.DataModel.Thursday = $"{Thursday:#,0}";
            model.DataModel.Friday = $"{Friday:#,0}";
            model.DataModel.Saturday = $"{Saturday:#,0}";


            model.DataModel.Total = $"{ChangeToNumber(model.DataModel.Week1) + ChangeToNumber(model.DataModel.Week2) + ChangeToNumber(model.DataModel.Week3) + ChangeToNumber(model.DataModel.Week4) + ChangeToNumber(model.DataModel.Week5) + ChangeToNumber(model.DataModel.Week6):#,0}";

            model.DrawnGraph.Total = new ChartValues<double>
            {
                ChangeToNumber(model.DataModel.Week1),ChangeToNumber(model.DataModel.Week2),ChangeToNumber(model.DataModel.Week3),ChangeToNumber(model.DataModel.Week4),ChangeToNumber(model.DataModel.Week5),ChangeToNumber(model.DataModel.Week6)
            };

            //}
        }


     
        private Button LastSelected = null;
        private void Last_Click(object sender, RoutedEventArgs e)
        {
            LastSelected = null;
            if (LastSelected != null)
            {
                LastSelected.Background = new SolidColorBrush(Colors.LightBlue);
            }
            Button btn = (Button)sender;
            btn.Background = new SolidColorBrush(Colors.LightBlue);
            LastSelected = btn;
        }
        public void GenerateWeekdaySale()
        {

        }
        public Point buttonPosition { get; set; }

        TextBlock IMtSelngSttusView.day1 => Day1;

        public TextBlock day8 => Day8;
        public TextBlock day15 => Day15;
        public TextBlock day22 => Day22;
        public TextBlock day29 => Day29;
        public TextBlock day36 => Day36;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            buttonPosition = PointHelper.GetPointOfButton(btn, this);
        }


    }

   
}
