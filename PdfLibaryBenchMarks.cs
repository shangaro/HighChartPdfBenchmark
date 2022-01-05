using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using PuppeteerSharp;

namespace Notification.HighCharPdfBenchmark
{
    [MemoryDiagnoser]
    [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
    [GcConcurrent(true)]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
        warmupCount: 0, targetCount: 1,invocationCount:10, id: "PdfLibaryBenchMark")]
    public class PdfLibraryBenchMarks
    {
         [Params(100)]
        public int filesToCreate;
        private readonly PupeteerSharpPdfGen _pdfGenerator;
        private IronPdfGen _pdfGeneratorIron;
        private readonly string html;
        private Browser _browser;
        public PdfLibraryBenchMarks()
        {
            _pdfGenerator = new PupeteerSharpPdfGen();
            _pdfGeneratorIron = new IronPdfGen();
            html = File.ReadAllText("Views/esrfoo.html");

        }


        [GlobalSetup(Targets = new[] { nameof(IronPdfSingle), nameof(IronPdfSequential), nameof(IronPdfParallel) })]
        public Task GlobalSetupA()
        {
            _pdfGeneratorIron.LaunchRenderer();
            Console.WriteLine("// " + "GlobalSetup Iron");
            return Task.CompletedTask;


        }
        [GlobalSetup(Targets = new[] { nameof(PupeteerPdfSingle), nameof(PupeteerPdfSequential), nameof(PupeteerPdfParallel) })]
        public async Task GlobalSetupB()
        {
            await _pdfGenerator.LaunchBrowserAsync(browserCount: 1);
            Console.WriteLine("// " + "GlobalSetup Pdf");


        }
        [Benchmark(Description = "PuppeteerSharp pdf 1 file")]
        public async Task PupeteerPdfSingle()
        {
            var result = await _pdfGenerator.GeneratePdf(html, "./pdfs/puppeteer.pdf");
            Console.WriteLine("bytes: " + result?.Length, ConsoleColor.Green);

        }
        [Benchmark(Description = "IronPdf pdf 1 files")]
        public async Task IronPdfSingle()
        {
            await _pdfGeneratorIron.GeneratePdf(html, "");

        }
        [Benchmark(Description = "PuppeteerSharp pdf seq 10 files")]
        public async Task PupeteerPdfSequential()
        {
            // var html = await File.ReadAllTextAsync("./Views/esrfoo.html");

            for (int i = 0; i < 10; i++)
            {
                var result = await _pdfGenerator.GeneratePdf(html, "./pdfs/puppeteer.pdf");
                Console.WriteLine("bytes: " + result?.Length, ConsoleColor.Green);

            }
        }

        [Benchmark(Description = "IronPdf pdf sequential 10 files")]
        public async Task IronPdfSequential()
        {
            for (int i = 0; i < 10; i++)
            {
                await _pdfGeneratorIron.GeneratePdf(html, "");

            }
        }
        [Benchmark(Description = "IronPdf pdf parallel 10 files")]
        public void IronPdfParallel()
        {
            var processorCount = Environment.ProcessorCount;
            Console.WriteLine($"// Parallel Benchmark with {processorCount} threads");
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = processorCount };
            Parallel.For(0, filesToCreate, parallelOptions, async i =>
             {
                 await _pdfGeneratorIron.GeneratePdf(html, "");

             });
        }
                [Benchmark(Description = "PuppeteerSharp pdf parallel 10 files")]
        public void PupeteerPdfParallel()
        {
            var processorCount = Environment.ProcessorCount;
            Console.WriteLine($"// Parallel Benchmark with {processorCount} threads");

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = processorCount };
            Parallel.For(0, filesToCreate, parallelOptions, async i =>
             {
                 var result = await _pdfGenerator.GeneratePdf(html, "./pdfs/puppeteer.pdf");
                 Console.WriteLine("bytes: " + result?.Length, ConsoleColor.Green);

             });


        }
        [GlobalCleanup]
        public async void GlobalCleanup()
        {
            Console.WriteLine("// " + "GlobalCleanup");
            await _pdfGenerator.DisposeAll();
        }
    }
}