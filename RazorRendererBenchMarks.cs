// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Engines;
// using HighChartPdfBenchmark.Models;
// using RazorEngine.Templating;
// using Engine = RazorEngine.Engine;

// namespace Notification.HighCharPdfBenchmark;

// [MemoryDiagnoser]
// [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
// [MeanColumn,StdDevColumn]

// // [SimpleJob(RunStrategy.Monitoring, launchCount: 4,
// //         warmupCount: 3, targetCount: 20, id: "RazorRendererBenchMarks")]

// public class RazorRendererBenchMarks
// {

//     private string _html;
//     private ESRModel _model;
//     private PupeteerSharpPdfGen _puppeteerPdfGen;
//     [Params(1,10,50)]
//     public int requests;

//     [GlobalSetup]
//     public async Task GlobalSetup()
//     {
//         _html = await File.ReadAllTextAsync("./Views/esrfoo.html");
//         _model = new ESRModel
//         {
//             HeaderTitle = "YOUR EXECUTIVE SUMMARY REPORT FOR:",
//             CompanyName = "Casella Waste Systems",
//             CurrentData = "October 31, 2021",
//             CompanyAddress = "4 Bunker Hill Industrial Park, Auburn, MA 01501",
//             ComparisonData = "September 30, 2021",
//             ComparisonTimeframe = "Month over Month",
//             Locations = "97",
//             Maximizer = "12%",
//             Visibility = "50.74%",
//             Accuracy = "90.91%",
//             ContentAnalysis = "87.96%",
//             BingTotalImpressions = "10,758",
//             Reviews = "5",
//             StarRating = "4.2 out of 5 %",
//             LocalRank = "17.11"

//         };


//     }
//     [Benchmark]
//     public void CompileRazorSequential()
//     {
//         for (int i = 0; i < requests; i++)
//         {
//             var generatedBody = Engine.Razor.RunCompile(_html, "esrtest", null, (object)_model);
//         }

//     }
//     [Benchmark]
//     public void CompileRazorParallel()
//     {
//         var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
//         Parallel.For(0, requests, parallelOptions, i =>
//          {
//              Engine.Razor.RunCompile(_html, "esrtest", null, (object)_model);
//          });

//     }
//     [GlobalCleanup]
//     public void GlobalCleanup()
//     {
//         _html = null;
//         _model = null;
//     }

// }

