using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using CefSharp;
using CefSharp.OffScreen;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Notification.HighCharPdfBenchmark
{
    public class Application : BackgroundService
    {
        private readonly ILogger<Application> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private int? _exitCode;
        private readonly PupeteerSharpPdfGen _pupeteer;

        public Application(ILogger<Application> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _appLifetime = appLifetime;
            _exitCode = null;
            _pupeteer= new PupeteerSharpPdfGen();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
               {
                   try
                   {
                       _logger.LogInformation("Application is started");
                       //    var chartParam = await File.ReadAllTextAsync("esrChartBody.json");
                       //    var body = JsonWrapper.DeserializeTextJson<ChartParameters>(chartParam);
                       //    var httpClient=new HttpClient();
                       //    httpClient.BaseAddress = new Uri("http://export.highcharts.com/");
                       //    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                       //    var response = await httpClient.PostAsJsonAsync("highchart",body);
                       
                        //    BenchmarkSwitcher.FromAssembly(typeof(Application).Assembly).Run(args: new[] { "--filter", "*PdfSequential*" }, config:ManualConfig.Create(DefaultConfig.Instance)
                        //    .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
                        //    .WithOption(ConfigOptions.JoinSummary, true));
                        // var puperteerPdfGen = new PupeteerSharpPdfGen();
                        var html =File.ReadAllText("Views/esrfoo.html");
                        // await puperteerPdfGen.LaunchBrowserAsync(browserCount:1);
                        // var stopwatch= new Stopwatch();
                        // stopwatch.Start();
                        // // Parallel.For(0, 10, async i =>
                        // // {
                        // //     var result = await puperteerPdfGen.GeneratePdf(html, "");
                        // //     Console.WriteLine("bytes: " + result?.Length, ConsoleColor.Green);
                        // // });

                        // for(int i=0;i<10;i++)
                        // {
                        //     var result = await puperteerPdfGen.GeneratePdf(html, "");
                        //     _logger.LogInformation("bytes: " + result?.Length);
                        // }
                        // stopwatch.Stop();
                        // _logger.LogInformation("Elapsed: " + stopwatch.Elapsed+"s");
                        // await puperteerPdfGen.DisposeAll();
                        
                        #if ANYCPU
                            CefRuntime.SubscribeAnyCpuAssemblyResolver();
                        #endif
                        
                        using(var brow= new ChromiumWebBrowser(new CefSharp.Web.HtmlString(html)))
                        {
                            var initialLoadResponse= await brow.WaitForInitialLoadAsync();
                            if(!initialLoadResponse.Success){
                                throw new Exception(string.Format("Page load failed with ErrorCode:{0}, HttpStatusCode:{1}", initialLoadResponse.ErrorCode, initialLoadResponse.HttpStatusCode));
                            }
                            await brow.PrintToPdfAsync("Views/esrfoo.pdf",new PdfPrintSettings{
                                BackgroundsEnabled=true,
                                HeaderFooterEnabled=true,
                                
                            });
                        }
                   }
                      
                   catch (Exception ex)
                   {
                       _logger.LogError("Processor Failure -", ex.Message);
                       _exitCode = 1;
                   }
                   finally
                   {
                       _appLifetime.StopApplication();

                   }
               });
            });

            return Task.CompletedTask;

        }


    }
}