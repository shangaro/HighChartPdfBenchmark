using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Notification.HighCharPdfBenchmark
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                await CreateHostBuilder(args).RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
                Environment.Exit(-1);
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, configuration) =>
            {

                IHostEnvironment env = hostContext.HostingEnvironment;
                var environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT");

                configuration
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    // .AddEnvironmentVariables();


            }).ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();

            }).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });            
    }
}

