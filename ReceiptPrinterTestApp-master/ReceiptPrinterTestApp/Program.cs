using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ReceiptPrinterLib.PrinterDevice;

using System.Text;

namespace ReceiptPrinterTestApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .ConfigureServices(a =>
                {
                    a.AddLogging();
                    a.AddTransient<Form1>();
                    a.AddTransient<GeneralPrinter>();
                })
                .UseConsoleLifetime(a => { })
                .Build();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            using(var scope = host.Services.CreateScope()) 
            {
                Application.Run(scope.ServiceProvider.GetService<Form1>());
            }            
        }
    }
}