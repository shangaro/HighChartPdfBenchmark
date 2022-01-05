using System.Collections.Concurrent;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Options;
using PdfActor.Models;
using PuppeteerSharp;
using PuppeteerSharp.Media;
namespace Notification.HighCharPdfBenchmark;

public class PupeteerSharpPdfGen
{
    private readonly AppInfo _appInfo;
    private readonly ConcurrentBag<Browser> _browserPool;

    public PupeteerSharpPdfGen()
    {
        _appInfo = Startup.RegisterChromium();
        _browserPool = new ConcurrentBag<Browser>();
    }
    public async Task<byte[]> GeneratePdf(string html, string pdfPath)

    {
        Browser browser = null;
        byte[] byteArray = null;
        try
        {
            if (_browserPool.TryPeek(out browser))
            {
                using (var page = await browser.NewPageAsync())
                {

                    await page.SetContentAsync(html);

                    byteArray = await page.PdfDataAsync(new PdfOptions
                    {
                        Format = PuppeteerSharp.Media.PaperFormat.A4,
                        MarginOptions = new MarginOptions
                        {
                            Top = "10px",
                            Bottom = "10px",
                            Left = "20px",
                            Right = "20px"
                        },
                        DisplayHeaderFooter = true,
                        PrintBackground = true,
                        Scale = 1m,
                        Landscape = false
                    });

                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Ex occured" + ex.Message + ex.StackTrace);
            throw;
        }
        return byteArray;

    }



    public async Task LaunchBrowserAsync(int browserCount = 1)
    {
        try
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < browserCount; i++)
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = _appInfo.BrowserExecutablePath
                });
                 _browserPool.Add(browser);
            }
            stopWatch.Stop();
            Console.WriteLine("Time taken to launch browsers: " + stopWatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            await DisposeAll();

            Console.WriteLine("Ex occured" + ex.Message + ex.StackTrace);
        }



    }
    public async Task Init()
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision).ConfigureAwait(false);
    }
    public Task DisposeAll()
    {
        Parallel.ForEach(_browserPool, async browser =>
        {
            await browser.DisposeAsync();
        });
        _browserPool.Clear();
        return Task.CompletedTask;
    }


}

