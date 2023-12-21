using GoPOS.Models.Common;
using GoPOS.Service.Service.MST;
using GoShared.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;

namespace GoPOS.Servers;

public class WebApiServer
{
    private static readonly Lazy<WebApiServer> Lazy = new(() => new WebApiServer());
    private readonly IWebHost host;

    private WebApiServer()
    {
        host = new WebHostBuilder().UseKestrel().UseUrls($"http://*:{DataLocals.AppConfig.PosComm.TRPort}").UseStartup<Startup>().Build();
    }
    public static WebApiServer Go => Lazy.Value;

    public async void StartWebApiServer()
    {
        if (DataLocals.AppConfig.PosOption != null && ((char)EPOSFlag.MainPOS).ToString().Equals(DataLocals.AppConfig.PosOption.POSFlag))
        {
            await host.RunAsync();
            LogHelper.Logger.Info("WebApiServer 시작...");
        }
    }

    public void StopWebApiServer()
    {
        if (DataLocals.AppConfig.PosOption == null)
        {
            return;
        }
        if (((char)EPOSFlag.MainPOS).ToString().Equals(DataLocals.AppConfig.PosOption.POSFlag))
        {
            host.StopAsync();
            host.Dispose();
            LogHelper.Logger.Info("WebApiServer 종료...");
        }
    }
}

public class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddSingleton<IApplicationBuilder, ApplicationBuilder>();
        typeof(Startup).Assembly.GetTypes().Where(type => type.IsClass).Where(type => type.Name.EndsWith("Service")).ToList()
            .ForEach(vmType => services.AddSingleton(vmType.GetInterfaces().First(), vmType));
    }

    public static void Configure(IApplicationBuilder app)
    {

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
