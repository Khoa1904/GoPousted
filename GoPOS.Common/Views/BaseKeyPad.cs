using GoPOS.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Caliburn.Micro;

namespace GoPOS.Common.Views
{
    public static class BaseKeyPad
    {
        public static void KeyClicked(this UserControl uc, object sender, RoutedEventArgs e,
            EventHandler OnClear, EventHandler OnBack, EventHandler OnEnter)
        {
            if (Keyboard.PrimaryDevice == null ||
                Keyboard.PrimaryDevice.ActiveSource == null)
                return;

            Button b = (Button)sender;
            string sKey = (string)b.Tag;
            if ("00".Equals(sKey))
            {
                //SimKeyboard.Press(Key.NumPad0);                
                //SimKeyboard.Press(Key.NumPad0);
                IInputElement focusedControl = Keyboard.FocusedElement;
                string mainPropName = "Text";

                if (focusedControl is PasswordBox ||
                    focusedControl is TextBox)
                {
                    if (focusedControl is PasswordBox)
                    {
                        mainPropName = "Password";
                    }

                    int maxLength = Convert.ToInt32(focusedControl.GetType().GetProperty("MaxLength")?.GetValue(focusedControl));
                    string? cText = focusedControl.GetType().GetProperty(mainPropName)?.GetValue(focusedControl).ToString();
                    int length = string.IsNullOrEmpty(cText) ? 0 : cText.Length;
                    if (maxLength - length > 0 || maxLength <= 0)
                    {
                        var addText = "00".Substring(0,
                            Math.Min(2, maxLength <= 0 ? 2 : maxLength - cText.Length));
                        focusedControl.GetType().GetProperty(mainPropName)?.SetValue(focusedControl, cText + addText);
                        length = (cText + addText).Length;
                    }

                    focusedControl.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.Public | 
                        BindingFlags.NonPublic).Invoke(focusedControl, new object[] { length, 0 });
                }
            }
            else if (sKey != "Clear")
            {
                SimKeyboard.Press((Key)Enum.Parse(typeof(Key), sKey, true));
            }

            if ("Clear".Equals(sKey))
            {
                //IInputElement focusedControl = FocusManager.GetFocusedElement(this);
                IInputElement focusedControl = Keyboard.FocusedElement;
                string mainPropName = "Text";
                if (focusedControl is PasswordBox)
                {
                    mainPropName = "Password";
                }
                focusedControl.GetType().GetProperty(mainPropName)?.SetValue(focusedControl, string.Empty);
                OnClear?.Invoke(null, EventArgs.Empty);
            }
            if ("Back".Equals(sKey))
            {
                OnBack?.DynamicInvoke();
            }
            if ("Enter".Equals(sKey))
            {
                OnEnter?.Invoke(null, EventArgs.Empty);
                SimKeyboard.Press(Key.Enter);
            }
        }

        public static void InvokeKey(IEventAggregator eventAggregator, Key k)
        {
            var ev = new KeyboardEventData()
            {
                FocusedControl = Keyboard.FocusedElement,
                PressedKey = k,
                FwdCancelled = false
            };

            eventAggregator.PublishOnUIThreadAsync(ev);
            if (ev.FwdCancelled)
            {
                return;
            }
        }
    }
}
