using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Polly.Extensions.Http;
using Polly;
using PuppeteerSharp;
using System.Runtime.InteropServices;

namespace Notification.HighCharPdfBenchmark
{
    public class Startup
    {
        private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(300);
        private static readonly Polly.IAsyncPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                }
            );


        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            services.AddLogging();

            services.AddMvc()
                   .AddRazorRuntimeCompilation(options => options.FileProviders.Add(
                            new PhysicalFileProvider(
                                Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "./Views"))
                           ));
            //  RegisterChromium();

            services.AddHostedService<Application>();
        }

        public static AppInfo RegisterChromium()
        {

            var bfOptions = new BrowserFetcherOptions();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                bfOptions.Path = Path.GetTempPath();
            }
            var bf = new BrowserFetcher(bfOptions);
            bf.DownloadAsync(BrowserFetcher.DefaultChromiumRevision).GetAwaiter().GetResult();

            var info = new AppInfo
            {
                BrowserExecutablePath = bf.GetExecutablePath(BrowserFetcher.DefaultChromiumRevision),
            };
            return info;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }

    public class AppInfo
    {
        public string BrowserExecutablePath { get; set; }
    }
}
