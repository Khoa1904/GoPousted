using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GoShared.Helpers;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Key = System.Windows.Input.Key;
using System.Windows.Threading;

namespace GoPOS.Common.Helpers.Controls
{
    public class TextBoxHelpers : DependencyObject
    {

        public static bool GetIsNumeric(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericProperty);
        }

        public static void SetIsNumeric(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsNumeric.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNumericProperty =
     DependencyProperty.RegisterAttached("IsNumeric", typeof(bool), typeof(TextBoxHelpers), new PropertyMetadata(false, new PropertyChangedCallback((s, e) =>
     {
         TextBox targetTextbox = s as TextBox;
         if (targetTextbox != null)
         {
             if ((bool)e.OldValue && !((bool)e.NewValue))
             {
                 targetTextbox.PreviewTextInput -= targetTextbox_PreviewTextInput;

             }
             if ((bool)e.NewValue)
             {
                 targetTextbox.PreviewTextInput += targetTextbox_PreviewTextInput;
                 targetTextbox.PreviewKeyDown += targetTextbox_PreviewKeyDown;
             }
         }
     })));

        static void targetTextbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var newChar = ((int)e.Key);
            //string kStr = Convert.ToChar((int)e.Key).ToString();
            //e.Handled = (e.Key == Key.Space) || !TypeHelper.IsNumeric(kStr);
            e.Handled = (e.Key == Key.Space) || !(newChar >= 34 && newChar <= 43 ||
                      newChar >= 74 && newChar <= 83 ||
                      newChar == 2);
        }

        static void targetTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //var text = e.Text;
            //if (!string.IsNullOrEmpty(text))
            //{
            //    Char newChar = e.Text.ToString()[0];
            //    e.Handled = !Char.IsNumber(newChar) || TypeHelper.IsHangulChar(newChar);
            //}

            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = !regex.IsMatch(e.Text);
        }
    }

    public class EnableDragHelper
    {
        public static readonly DependencyProperty EnableDragProperty = DependencyProperty.RegisterAttached(
            "EnableDrag",
            typeof(bool),
            typeof(EnableDragHelper),
            new PropertyMetadata(default(bool), OnLoaded));

        private static void OnLoaded(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var uiElement = dependencyObject as UIElement;
            if (uiElement == null || (dependencyPropertyChangedEventArgs.NewValue is bool) == false)
            {
                return;
            }
            if ((bool)dependencyPropertyChangedEventArgs.NewValue == true)
            {
                uiElement.MouseMove += UIElementOnMouseMove;
            }
            else
            {
                uiElement.MouseMove -= UIElementOnMouseMove;
            }

        }

        private static void UIElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var uiElement = sender as UIElement;
            if (uiElement != null)
            {
                if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    DependencyObject parent = uiElement;
                    int avoidInfiniteLoop = 0;
                    // Search up the visual tree to find the first parent window.
                    while ((parent is Window) == false)
                    {
                        parent = VisualTreeHelper.GetParent(parent);
                        avoidInfiniteLoop++;
                        if (avoidInfiniteLoop == 1000)
                        {
                            // Something is wrong - we could not find the parent window.
                            return;
                        }
                    }
                    var window = parent as Window;
                    var dispatcher = Application.Current.MainWindow.Dispatcher;
                    dispatcher.BeginInvoke(DispatcherPriority.Normal, new System.Action(() =>
                    {
                        if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
                            window.DragMove();

                    }));
                }
            }
        }

        public static void SetEnableDrag(DependencyObject element, bool value)
        {
            element.SetValue(EnableDragProperty, value);
        }

        public static bool GetEnableDrag(DependencyObject element)
        {
            return (bool)element.GetValue(EnableDragProperty);
        }
    }
}
