// using System.Net.Http.Headers;
// using System.Net.Http.Json;
// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Engines;
// using Notification.Bll.Core.Interfaces;
// using Notification.Bll.Core.Models;
// using Notification.Bll.Core.Models.Templates;
// using Notification.Bll.Core.Wrappers;

// namespace Notification.HighCharPdfBenchmark
// {
    

//     [MemoryDiagnoser]
//     [MarkdownExporter, AsciiDocExporter, HtmlExporter, CsvExporter, RPlotExporter]
//     [SimpleJob(RunStrategy.Monitoring, launchCount: 1,
//         warmupCount: 0, targetCount: 5,id: "HighChartServerBM")]

//     public class HighChartServerBM
//     {
//         private HttpClient _httpClient;
//         private readonly ChartParameters _chartParameters;

//         public HighChartServerBM(){
           
//             var file = File.ReadAllText("esrChartBody.json");
//         }
        
//         [GlobalSetup]
//         public async Task GlobalSetup(){
//             _httpClient=new HttpClient(){BaseAddress=new System.Uri("http://export.highcharts.com/")};
//             _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//         }
//         [Benchmark]
//         public async Task<IEnumerable<ChartModel>> GenerateChart()
//         {
//             var chartParams = 
//             var body = JsonWrapper.DeserializeTextJson<ChartParameters>(chartParams);
//             // do post request
//             var response = await _httpClient.PostAsJsonAsync("highchart", body);
 
            
//         }
    

//     }
// }