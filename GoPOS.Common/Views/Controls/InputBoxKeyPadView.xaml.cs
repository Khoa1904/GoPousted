using GoPOS.Common.Helpers;
using GoPOS.Common.Interface.View;
using GoPOS.Common.ViewModels;
using GoPOS.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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

namespace GoPOS.Common.Views.Controls
{
    /// <summary>
    /// Interaction logic for InputKeyPad.xaml
    /// </summary>
    public partial class InputBoxKeyPadView : UCViewBase, IKeyPadView
    {
        private string _oldText = string.Empty;
        public event EventHandler? OnBack;
        public event EventHandler? OnEnter;
        public event EventHandler? OnClear;
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(InputBoxKeyPadView), new PropertyMetadata(string.Empty, TextChangedCallback));
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(InputBoxKeyPadView), new PropertyMetadata(0, MaxLengthChangedCallback));
        public static readonly DependencyProperty InputTypeProperty = DependencyProperty.Register("InputType", typeof(EInputType), typeof(InputBoxKeyPadView), new PropertyMetadata(EInputType.None, InputTypeChangedCallback));
        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InputBoxKeyPadView keyPad = (InputBoxKeyPadView)d;
            keyPad.Text = (string)e.NewValue;
        }

        private static void MaxLengthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InputBoxKeyPadView keyPad = (InputBoxKeyPadView)d;
            keyPad.MaxLength = (int)e.NewValue;
        }
        private static void InputTypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InputBoxKeyPadView keyPad = (InputBoxKeyPadView)d;
            keyPad.InputType = (EInputType)e.NewValue;
        }

        [Category("Common")]
        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                _oldText = Text;
                this.SetValue(TextProperty, value);
            }                
        }

        [Category("Common")]
        public int MaxLength
        {
            get
            {
                return (int)this.GetValue(MaxLengthProperty);
            }
            set
            {
                this.SetValue(MaxLengthProperty, value);
                this.txtInputText.MaxLength = value;
            }
        }
        [Category("Common")]
        public EInputType InputType
        {
            get
            {
                return (EInputType)this.GetValue(InputTypeProperty);
            }
            set
            {
                this.SetValue(InputTypeProperty, value);
            }
        }



        public InputBoxKeyPadView()
        {
            InitializeComponent();
            txtInputText.Focus();
            Loaded += InputBoxKeyPadView_Loaded;
            GotFocus += InputBoxKeyPadView_GotFocus;
            
        }

        private void InputBoxKeyPadView_GotFocus(object sender, RoutedEventArgs e)
        {
            txtInputText.Focus();
        }

        private void InputBoxKeyPadView_Loaded(object sender, RoutedEventArgs e)
        {
            txtInputText.Text = "";
            txtInputText.Focus();
        }

        protected void KeyClicked(object sender, RoutedEventArgs e)
        {
            this.KeyClicked(sender, e, OnClear, OnBack, OnEnter);
        }

        private void txtInputText_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (InputType)
            {
                case EInputType.Number:
                    if(txtInputText.Text.Length > 0) {
                        int numericValue;
                      bool isNumber = int.TryParse(txtInputText.Text, out numericValue);
                        if (isNumber)
                        {
                            this.Text = txtInputText.Text;
                        }
                        else
                        {
                            txtInputText.Text = _oldText;
                            txtInputText.SelectionStart = _oldText.Length;
                            txtInputText.SelectionLength = 0;
                        }
                    }
                    else
                    {
                        this.Text = txtInputText.Text;
                    }
                   
                    break;
                case EInputType.Text:
                case EInputType.None:
                default:
                    this.Text = txtInputText.Text;
                    break;
            }
        }

        public void ClearText()
        {
            if (string.IsNullOrEmpty(txtInputText.Text))
            {
                return;
            }
            txtInputText.Text = string.Empty;
        }
    }
    public enum EInputType { None, Number,Text}
}
