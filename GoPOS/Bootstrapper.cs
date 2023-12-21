using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using GoPOS.Common.Interface.Model;
using GoPOS.Common.ViewModels;
using GoPOS.Models.Common;
using GoPOS.Service;
using GoPOS.ViewModels;
using GoShared.Contract;
using GoShared.Helpers;
using GoPOS.Database;
using Microsoft.Extensions.Logging;
using System.Web.Services.Description;
using System.Configuration;
using System.Data.Entity;
using GoShared;
using AutoMapper.Internal;
using System.Globalization;
using System.Runtime.CompilerServices;
using GoPOS.Common.PrinterLib;
using Serilog;
using Serilog.Extensions.Logging;
using GoPOS.Models.Config;
using GoPOS.Common.Views.Controls;
using System.Threading;
using System.Windows.Threading;
//using GoPOS.OrderPay.ViewModels.ExtMenus;

namespace GoPOS
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer mSimpleContainer;
        public Bootstrapper()
        {
            mSimpleContainer = new SimpleContainer();
            Initialize();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }
        protected override void Configure()
        {
            mSimpleContainer.Singleton<IWindowManager, WindowManager>();
            mSimpleContainer.Singleton<IEventAggregator, EventAggregator>();
            mSimpleContainer.Singleton<GeneralPrinter>();
            var serviceProviderAdapter = new ServiceProviderAdapter(mSimpleContainer);
            mSimpleContainer.Instance<IServiceProvider>(serviceProviderAdapter);

            var list = new List<MenuConfigInfo>();
            list.Add(new MenuConfigInfo()
            {
                MenuCode = "OrderPayMainViewModel",
                MenuName = "OrderPayMainViewModel",
                ModelName = "OrderPayMainViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.OrderPay",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "OrderPayMainViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "PostPayTableManagermentViewModel",
                MenuName = "PostPayTableManagermentViewModel",
                ModelName = "PostPayTableManagermentViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.OrderPay",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "PostPayTableManagermentViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "TableArrangePopupViewModel",
                MenuName = "TableArrangePopupViewModel",
                ModelName = "TableArrangePopupViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.OrderPay",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "TableArrangePopupViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "SalesMngMainViewModel",
                MenuName = "SalesMngMainViewModel",
                ModelName = "SalesMngMainViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.Sales",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "SalesMngMainViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "RePrintBillViewModel",
                MenuName = "RePrintBillViewModel",
                ModelName = "RePrintBillViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.OrderPay",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "RePrintBillViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "SellingStatusMainViewModel",
                MenuName = "SellingStatusMainViewModel",
                ModelName = "SellingStatusMainViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.SellingStatus",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "SellingStatusMainViewModel",
            });

            list.Add(new MenuConfigInfo()
            {
                MenuCode = "ConfigSetupMainViewModel",
                MenuName = "ConfigSetupMainViewModel",
                ModelName = "ConfigSetupMainViewModel",
                ModelNamespace = "GoPOS.ViewModels",
                Assembly = "GoPOS.ConfigSetup",
                MenuType = EMenuType.Main.ToString(),
                ViewModelNm = "ConfigSetupMainViewModel",
            });

            var menuConfigFile = Path.Combine(Environment.CurrentDirectory.GetParentFolder(),
                    AppConfig.PathConfig, AppConfig.MenuConfig);
            XmlHelper.WriteXml<MenuConfigInfo>(list, menuConfigFile);
            DataLocals.MenuList = XmlHelper.Reads<MenuConfigInfo>(menuConfigFile).data ?? (new List<MenuConfigInfo>());
            DataLocals.LoadConfigs();

            var ci = new CultureInfo("ko");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

            var assemblys = GetGoPOSAssemblies();
            Extensions.GoPOSAssemblies = assemblys;

            foreach (var assembly in assemblys)
            {
                // var xxxx = assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList();

                assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList()
                .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType.GetInterfaces().First(), nameof(vmType), vmType));

                assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("ViewModel", StringComparison.CurrentCulture)).ToList()
                .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType, nameof(vmType), vmType));
                //.ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType.GetInterfaces().FirstOrDefault() ?? vmType, nameof(vmType), vmType));

                assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("View", StringComparison.CurrentCulture)).ToList()
                .ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType, nameof(vmType), vmType));
                //.ForEach(vmType => mSimpleContainer.RegisterSingleton(vmType.GetInterfaces().FirstOrDefault() ?? vmType, nameof(vmType), vmType));
            }

        }

        public class ServiceProviderAdapter : IServiceProvider
        {
            private readonly SimpleContainer container;

            public ServiceProviderAdapter(SimpleContainer container)
            {
                this.container = container;
            }

            public object GetService(Type serviceType)
            {
                return container.GetInstance(serviceType, null);
            }

            public ServiceProviderAdapter() { }
        }


        private List<Assembly> GetGoPOSAssemblies()
        {
            var path = AppContext.BaseDirectory;  // returns bin/debug path
            var directory = new DirectoryInfo(path);
            var assemblys = new List<string>();

            if (directory.Exists)
            {
                var dllFiles = directory.GetFiles("GoPOS*.dll");  // get only assembly files from debug path
                foreach (var dllFile in dllFiles)
                {
                    assemblys.Add(dllFile.FullName);
                }

                var dllFiles1 = directory.GetFiles("OllStarPOS*.dll");  // get only assembly files from debug path
                foreach (var dllFile in dllFiles1)
                {
                    assemblys.Add(dllFile.FullName);
                }
            }

            List<Assembly> assemblies = new List<Assembly>();
            assemblys.ForEach(a =>
            {
                if (!a.Contains("VanAPI.dll"))
                {
                    assemblies.Add(Assembly.LoadFrom(a));
                }
            });
            return assemblies;
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return GetGoPOSAssemblies();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            // await DisplayRootViewForAsync(typeof(OrderPayMainViewModel)).ConfigureAwait(false);
          //  var loadingBar = new ProgressBarView();

    //        loadingBar.Show();
            //System.Threading.Thread.Sleep(4000);
            await DisplayRootViewForAsync(typeof(ShellViewModel)).ConfigureAwait(false);
     //       loadingBar?.Close();
        }

        protected override object GetInstance(Type service, string key)
        {
            return mSimpleContainer.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return mSimpleContainer.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            mSimpleContainer.BuildUp(instance);
        }
    }
}
