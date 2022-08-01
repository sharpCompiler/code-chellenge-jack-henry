using System.Net.Http.Headers;
using Code_Challenge.Business.Definitions;
using Code_Challenge.Business.Parallelism;
using Code_Challenge.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Code_Challenge.UI;

public class AppService
{
    public static ManualResetEvent ConsoleEvent = new(false); 
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly IReportService _reportService;

    public AppService(IServiceProvider serviceProvider, ILogger logger, IReportService reportService)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _reportService = reportService;
    }

    public async Task Run()
    {
        _logger.Information("Running the application.");


#pragma warning disable CS4014 //(No need to wait for it)
        Task.Run(() => StartDataImporter(_serviceProvider));
#pragma warning restore CS4014


        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Code challenge App");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Press R to see the Tweet reports");
        Console.WriteLine("Press any other key to quit the application");
        var key = Console.ReadLine();

        while (key?.ToLower() == "r")
        {
            var report = await _reportService.GetReportAsync();

            var listOfTopTenHashTags = report.HashTags.Select(x => $"{x.Name}(Count {x.Count})"); 
            Console.WriteLine($"Top 10 hash tags:  {string.Join(' ', listOfTopTenHashTags)}");
            Console.WriteLine($"Total number of Tweets:{report.TweetCount}");

            key = Console.ReadLine();
        }

        Exit();
    }


    private async Task StartDataImporter(IServiceProvider serviceProvider)
    {
        _logger.Information("Starting Twitter Importer - Simulator");
        var cancellationToken = new CancellationToken();
        var twitterService = serviceProvider.GetService<IImportTwitterService>();
        var pipeline = serviceProvider.GetService<TweetPipeLine>();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var tweets = await twitterService.GetTweetsAsync();
                await pipeline.Process(tweets);

                await Task.Delay(5000, cancellationToken); 
            }
            catch (AggregateException ae)
            {
                _logger.Error(ae.InnerException, "En exception has occurred in the pipeline work process");
                // here cen we notify the support or product owner 
            }  
            catch (Exception e)
            {
                _logger.Error(e, "En exception has occurred that JOB stop");
                // here cen we notify the support or product owner 
            }
        }
    }

    public void Exit()
    {
        _logger.Warning("Application terminated.");
        ConsoleEvent.Set();

    }
}


