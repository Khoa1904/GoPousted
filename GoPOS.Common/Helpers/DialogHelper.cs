using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Common.ViewModels.Controls;
using GoPOS.Common.Views.Controls;
using GoPOS.Models;
using GoShared;
using Microsoft.Web.WebView2.Core;

namespace GoPOS.Helpers
{
    public static class DialogHelper
    {
        public static MessageBoxResult MessageBox(string msg)
        {
            return MessageBox(msg, GMessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult MessageBox(string msg, GMessageBoxButton buttons, MessageBoxImage buttonImages)
        {
            IoC.Get<MessageViewModel>().MsgSet(msg, buttons);

            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.MinWidth = 402;
            settings.MinHeight = 212;
            settings.WindowStyle = WindowStyle.None;

            IWindowManager manager = new WindowManager();
            manager.ShowDialogAsync(IoC.Get<MessageViewModel>(), null, settings);
            return IoC.Get<MessageViewModel>().DialogResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="buttons"></param>
        /// <param name="buttonImages"></param>
        /// <param name="buttonCaps">caption of each button shown</param>
        /// <returns></returns>
        public static MessageBoxResult MessageBox(string msg, GMessageBoxButton buttons, MessageBoxImage buttonImages, string[] buttonCaps)
        {
            IoC.Get<MessageViewModel>().MsgSet(msg, buttons, buttonCaps);

            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.MinWidth = 402;
            settings.MinHeight = 212;
            settings.WindowStyle = WindowStyle.None;

            IWindowManager manager = new WindowManager();
            manager.ShowDialogAsync(IoC.Get<MessageViewModel>(), null, settings);
            return IoC.Get<MessageViewModel>().DialogResult();
        }

        #region ShowDialog by type


        public static Dictionary<string, object> ShowDialog(Type viewType, double width, double height, params object[] csParams)
        {
            return ShowDialog(false,viewType, width, height, -1, -1, null, csParams);
        }
        public static Dictionary<string, object> ShowDialog_ShowInTaskbar(Type viewType, double width, double height, params object[] csParams)
        {
            return ShowDialog(true, viewType, width, height, -1, -1, null, csParams);
        }
        public static Dictionary<string, object> ShowDialogWithCoords(Type viewType, double width, double height, double left, double top, params object[] csParams)
        {
            return ShowDialog(false,viewType, width, height, left, top, null, csParams);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="csParams">constructor params</param>
        /// <returns></returns>
        public static Dictionary<string, object> ShowDialog(Type viewType, double width, double height, double left, double top, OPAreaActivatedEventHandler areaActivatedEventHandler, params object[] csParams)
        {
            //var dialogView = IoC.Get<DialogWindowViewModel>();
            var childView = (IViewModel)IoC.GetInstance(viewType, null);
            if (areaActivatedEventHandler != null)
            {
                var e = new ChildActivatedEventArgs()
                {
                    AreaName = string.Empty,
                    Cancelled = false,
                    ChildType = viewType,
                    ChildViewModel = childView,
                    ChildVMType = viewType.Name
                };
                areaActivatedEventHandler(childView, e);

                if (e.Cancelled)
                {
                    return null;
                }
            }

            childView.SetData(csParams);

            dynamic settings = new ExpandoObject();
            if (left == -1 || top == -1)
            {
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                settings.WindowStartupLocation = WindowStartupLocation.Manual;
                settings.Left = left;
                settings.Top = top;
            }
            settings.ResizeMode = ResizeMode.CanResize;
            settings.SizeToContent = SizeToContent.Manual;


            settings.WindowStyle = WindowStyle.None;            
            settings.Width = width;
            settings.Height = height;
            settings.ShowInTaskbar = false;
            settings.AllowsTransparency = true;

            IWindowManager manager = new WindowManager();
            manager.ShowDialogAsync(childView, null, settings);
            return ((IDialogViewModel)childView).DialogResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewType"></param>
        /// <param name="csParams">constructor params</param>
        /// <returns></returns>
        public static Dictionary<string, object> ShowDialog(bool showInTaskbar, Type viewType, double width, double height, double left, double top, OPAreaActivatedEventHandler areaActivatedEventHandler, params object[] csParams)
        {
            //var dialogView = IoC.Get<DialogWindowViewModel>();
            var childView = (IViewModel)IoC.GetInstance(viewType, null);
            if (areaActivatedEventHandler != null)
            {
                var e = new ChildActivatedEventArgs()
                {
                    AreaName = string.Empty,
                    Cancelled = false,
                    ChildType = viewType,
                    ChildViewModel = childView,
                    ChildVMType = viewType.Name
                };
                areaActivatedEventHandler(childView, e);

                if (e.Cancelled)
                {
                    return null;
                }
            }

            childView.SetData(csParams);

            dynamic settings = new ExpandoObject();
            if (left == -1 || top == -1)
            {
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                settings.WindowStartupLocation = WindowStartupLocation.Manual;
                settings.Left = left;
                settings.Top = top;
            }
            settings.ResizeMode = ResizeMode.CanResize;
            settings.SizeToContent = SizeToContent.Manual;


            settings.WindowStyle = WindowStyle.None;
            settings.Width = width;
            settings.Height = height;
            settings.ShowInTaskbar = showInTaskbar;
            settings.AllowsTransparency = true;

            IWindowManager manager = new WindowManager();
            manager.ShowDialogAsync(childView, null, settings);
            return ((IDialogViewModel)childView).DialogResult;
        }

        #endregion

        #region ShowDialog by Typename

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewTypeName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="csParams"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ShowDialog(string viewTypeName, double width, double height, params object[] csParams)
        {
            return ShowDialog(viewTypeName, width, height, null, csParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewTypeName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="areaActivatedEventHandler"></param>
        /// <param name="csParams"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ShowDialog(string viewTypeName, double width, double height, OPAreaActivatedEventHandler areaActivatedEventHandler, params object[] csParams)
        {
            Type viewType = null;
            foreach (var ass in Extensions.GoPOSAssemblies)
            {
                viewType = ass.GetTypes().Where(type => type.IsClass).FirstOrDefault(type => type.Name.Equals(viewTypeName));
                if (viewType != null)
                {
                    break;
                }
            }

            if (viewType == null) return null;
            return ShowDialog(false,viewType, width, height, -1, -1, areaActivatedEventHandler, csParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewTypeName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="csParams"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ShowDialog(string viewTypeName, double width, double height, double left, double top, params object[] csParams)
        {
            Type viewType = null;
            foreach (var ass in Extensions.GoPOSAssemblies)
            {
                viewType = ass.GetTypes().Where(type => type.IsClass).FirstOrDefault(type => type.Name.Equals(viewTypeName));
                if (viewType != null)
                {
                    break;
                }
            }

            if (viewType == null) return null;
            return ShowDialog(viewType, width, height, left, top, csParams);
        }

        #endregion
    }

    public enum GMessageBoxButton
    {
        OK,
        OKCancel,
        YesNo
    }
}
