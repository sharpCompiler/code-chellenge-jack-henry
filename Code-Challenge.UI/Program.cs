using Code_Challenge.Business.Definitions;
using Code_Challenge.Business.Parallelism;
using Code_Challenge.Business.Repository;
using Code_Challenge.Business.Services;
using Code_Challenge.Infrastructure;
using Code_Challenge.Persistence;
using Code_Challenge.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var services = new ServiceCollection();

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.AddEnvironmentVariables();

var config = builder.Build();

var log = new LoggerConfiguration()
    .Enrich.WithCorrelationId()
    .Enrich.WithThreadId()
    .Enrich.WithThreadName()
    .WriteTo.File(path: "log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "[{Timestamp:MM/dd/yy HH:mm:ss} {CorrelationId} {Level:u3}]  Thread: <{ThreadId}><{ThreadName}> :: {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

services.AddSingleton<IConfiguration>(config);
services.AddSingleton<ILogger>(log);
services.AddSingleton<IReportService, ReportService>();
services.AddSingleton<ITweetRepository, TweetRepository>();
services.AddSingleton<IHashTagRepository, HashTagRepository>();
services.AddSingleton<IImportTwitterService, ImportTwitterService>();
services.AddSingleton<AppService>();
services.AddTransient<TweetPipeLine>();

var serviceProvider = services.BuildServiceProvider();


var app = serviceProvider.GetService<AppService>();

Task.Run(() => { app.Run(); });
AppService.ConsoleEvent.WaitOne(); // make current thread to wait 

app?.Exit();
