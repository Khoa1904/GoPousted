using ESCPOS_NET.Emitters.BaseCommandValues;
using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.Views;
using GoPOS.Models;

using System;
using System.Collections.Generic;
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


namespace GoPOS.Common.Views
{
    public partial class SelectBoxView : UCViewBase, ISelectBoxView
    {
        public SelectBoxView()
        {
            InitializeComponent();
        }
        public Task<List<MST_COMM_CODE>> GetCommonCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<List<MST_INFO_POS>> GetCommonPos()
        {
            throw new NotImplementedException();
        }

        public void OmniRender(List<MST_COMM_CODE> common)
        {
            var deck = common.ToArray();
            if (deck.Count() > 6)
            {
                ButtonP.Visibility = Visibility.Visible;
                ButtonN.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonP.Visibility = Visibility.Hidden;
                ButtonN.Visibility = Visibility.Hidden;
            }

            var Btnlist = ButtonGrid.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("CommonCode")).ToList();
            for (int i = 0; i < Btnlist.Count; i++)
            {
                var SetText = ButtonGrid.Children.Cast<Border>();
                Btnlist[i].Tag = common[i].COM_CODE;
                TextBlock tb = Btnlist[i].Content as TextBlock;
                tb.Text = common[i].COM_CODE_NAME;
            }
        }

        public void POSRender(MST_INFO_POS[] Pos, int currentPage)
        {
            var Btnlist = ButtonGrid.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("Post")).ToList();
            int check = Btnlist.Count - Pos.Length;
            if (currentPage == 0)
            {
                if (Pos.Count() > 5)
                {
                    ButtonP.IsEnabled = true;
                    ButtonN.IsEnabled = true;
                }
                else
                {
                    ButtonP.IsEnabled = false;
                    ButtonN.IsEnabled = false;
                }

                Btnlist[0].Tag = "POS00";
                var tb = (TextBlock)Btnlist[0].Content;
                if (tb != null)
                {
                    tb.Text = "전체";
                }
                for (int i = 1; i <= Pos.Length - 1; i++)
                {
                    Btnlist[i].Tag = "POS" + Pos[i - 1].POS_NO;
                    tb = (TextBlock)Btnlist[i].Content;
                    if (tb != null)
                    {
                        tb.Text = Pos[i - 1].POS_NO;
                    }
                }
                if (check > 0)
                {
                    for (int i = Pos.Length + 1; i < Btnlist.Count; i++)
                    {
                        Btnlist[i].Tag = null;
                        tb = (TextBlock)Btnlist[i].Content;
                        if (tb != null)
                        {
                            tb.Text = null;
                        }
                    }
                }
            }
            else
            {
                if (Pos.Count() > 6)
                {
                    ButtonP.IsEnabled = true;
                    ButtonN.IsEnabled = true;
                }
                else
                {
                    ButtonP.IsEnabled = false;
                    ButtonN.IsEnabled = false;
                }

                for (int i = 0; i < Pos.Length; i++)
                {
                    Btnlist[i].Tag = "POS" + Pos[i].POS_NO;
                    var tb = (TextBlock)Btnlist[i].Content;
                    if (tb != null)
                    {
                        tb.Text = Pos[i].POS_NO;
                    }
                }
                if (check > 0)
                {
                    for (int i = Pos.Length + 1; i < Btnlist.Count; i++)
                    {
                        Btnlist[i].Tag = null;
                        var tb = (TextBlock)Btnlist[i].Content;
                        if (tb != null)
                        {
                            tb.Text = null;
                        }
                    }
                }
            }
        }

        public void COMMONRender(MST_COMM_CODE[] Common, int currentPage)
        {
            var count = Common.Count();
            if (count > 5)
            {
                ButtonP.IsEnabled = true;
                ButtonN.IsEnabled = true;
            }
            else
            {
                ButtonP.IsEnabled = false;
                ButtonN.IsEnabled = false;
            }

            var Btnlist = ButtonGrid.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("Post")).ToList();
            if (currentPage == 0)
            {
                Btnlist[0].Tag = "Common00";
                var tb = (TextBlock)Btnlist[0].Content;
                if (tb != null)
                {
                    tb.Text = "전체";
                }
                for (int i = 1; i <= Common.Length; i++)
                {
                    Btnlist[i].Tag = "Common" + Common[i - 1].COM_CODE;
                    tb = (TextBlock)Btnlist[i].Content;
                    if (tb != null)
                    {
                        tb.Text = Common[i - 1].COM_CODE_NAME;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Common.Length; i++)
                {
                    Btnlist[i].Tag = "Common" + Common[i].COM_CODE;
                    TextBlock tb = Btnlist[i].Content as TextBlock;
                    tb.Text = Common[i].COM_CODE_NAME;
                }
            }

        }

        public void SEQRender(SETT_POSACCOUNT[] Req, int currentPage)
        {
            var Btnlist = ButtonGrid.FindVisualChildren<Button>().Where(p => p.Name.StartsWith("Post")).ToList();
            int check = Btnlist.Count - Req.Length;
            if (currentPage == 0)
            {
                if (Req.Count() > 5)
                {
                    ButtonP.IsEnabled = true;
                    ButtonN.IsEnabled = true;
                }
                else
                {
                    ButtonP.IsEnabled = false;
                    ButtonN.IsEnabled = false;
                }

                Btnlist[0].Tag = "SEQ00";
                var tb = (TextBlock)Btnlist[0].Content;
                if (tb != null)
                {
                    tb.Text = "전체";
                }
                for (int i = 1; i <= Req.Length; i++)
                {
                    Btnlist[i].Tag = "SEQ" + Req[i - 1].REGI_SEQ;
                    tb = (TextBlock)Btnlist[i].Content;
                    if (tb != null)
                    {
                        tb.Text = Req[i - 1].REGI_SEQ;
                    }
                }
                if (check > 0)
                {
                    for (int i = Req.Length + 1; i < Btnlist.Count; i++)
                    {
                        Btnlist[i].Tag = null;
                        tb = (TextBlock)Btnlist[i].Content;
                        if (tb != null)
                        {
                            tb.Text = null;
                        }
                    }
                }
            }
            else
            {
                if (Req.Count() > 6)
                {
                    ButtonP.IsEnabled = true;
                    ButtonN.IsEnabled = true;
                }
                else
                {
                    ButtonP.IsEnabled = false;
                    ButtonN.IsEnabled = false;
                }

                for (int i = 0; i < Req.Length; i++)
                {
                    Btnlist[i].Tag = "POS" + Req[i].POS_NO;
                    var tb = (TextBlock)Btnlist[i].Content;
                    if (tb != null)
                    {
                        tb.Text = "POS " + Req[i].POS_NO;
                    }
                }
                if (check > 0)
                {
                    for (int i = Req.Length + 1; i < Btnlist.Count; i++)
                    {
                        Btnlist[i].Tag = null;
                        var tb = (TextBlock)Btnlist[i].Content;
                        if (tb != null)
                        {
                            tb.Text = null;
                        }
                    }
                }
            }
        }
    }
}
