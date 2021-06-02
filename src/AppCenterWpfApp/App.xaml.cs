using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows;
using AppCenterTls13.IntegrationTests;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AppCenterWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Task.Run(async () =>
            {
                using (var host = CreateHostBuilder().Build())
                {
                    await host.StartAsync()
                        .ConfigureAwait(false);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

                    /*
                    AppCenter.Start("",
                        typeof(Crashes));
                    */

                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri("https://localhost:50443")
                    };

                    var response = await httpClient.GetAsync("/")
                        .ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    var request = WebRequest.Create ("https://localhost:50443");
                    var webResponse = request.GetResponse();

                    await host.StopAsync()
                        .ConfigureAwait(false);

                    await host.WaitForShutdownAsync().ConfigureAwait(false);
                }
            }).GetAwaiter().GetResult();
       }

        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(
                webBuilder =>
                {
                    webBuilder.UseKestrel(kestrelOptions =>
                    {
                        kestrelOptions.ConfigureHttpsDefaults(httpsOptions =>
                        {
                            // this is forcing tls 1.1 as you will need registry key changes for 1.3, but the bitwise on the servicepoint manager still causes the same failure
                            // to enable tls 1.3 see https://stackoverflow.com/questions/56072561/how-to-enable-tls-1-3-in-windows-10
                            httpsOptions.SslProtocols = SslProtocols.Tls11;
                        });

                        kestrelOptions.Listen(IPAddress.Loopback, 50443, listenOptions =>
                        {
                            listenOptions.UseHttps();
                        });

                    });

                    webBuilder.UseStartup<Startup>();
                });

    }
}
