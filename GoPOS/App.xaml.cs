using GoPOS.Models.Config;
using GoPOS.Helpers;
using GoShared.Helpers;
using System.Windows;
using System.Windows.Threading;
using GoPOS.Models.Common;
using System.Collections.Generic;
using System;
using Dapper;
using GoPOS.Service.Common;
using System.Security.Principal;
using System.Linq;

namespace GoPOS;

public partial class App : Application
{
    public App()
    {
        SqlMapper.AddTypeHandler(new NumberHandler<decimal>());
        SqlMapper.AddTypeHandler(new NumberHandler<short>());
        SqlMapper.AddTypeHandler(new NumberHandler<int>());

        Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(AppDispatcherUnhandledException);


        this.Startup += App_Startup;
    }
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        try
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Environment.Exit(Environment.ExitCode);
            Environment.Exit(1);
        }
        catch (Exception)
        {

        }
    }

    private void App_Startup(object sender, StartupEventArgs e)
    {
        LoadLanguageFile();
    }

    void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ShowUnhandledException(e);
    }

    private void ShowUnhandledException(DispatcherUnhandledExceptionEventArgs e)
    {
        string errorMessage = e.Exception.Message + (e.Exception.InnerException != null ? "\n" +
            e.Exception.InnerException.Message : null);
        LogHelper.Logger.Error(errorMessage);

        DialogHelper.MessageBox(errorMessage, GMessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    internal void LoadLanguageFile()
    {
        var languageCode = DataLocals.AppConfig.PosInfo.LangCode;
        if (string.IsNullOrEmpty(languageCode) == false)
        {
            var dictionariesToRemove = new List<ResourceDictionary>();

            foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (dictionary.Source == null) continue;
                if (dictionary.Source.ToString().Contains(@"/StringResources") == true)
                    dictionariesToRemove.Add(dictionary);
            }

            foreach (var item in dictionariesToRemove)
                Application.Current.Resources.MergedDictionaries.Remove(item);

            var languageDictionary = new ResourceDictionary()
            {
                Source = new Uri($"pack://application:,,,/GoPOS.Resources;component/Resource/StringResources.{languageCode}.xaml", UriKind.RelativeOrAbsolute)
            };

            Application.Current.Resources.MergedDictionaries.Add(languageDictionary);
        }
    }
}
