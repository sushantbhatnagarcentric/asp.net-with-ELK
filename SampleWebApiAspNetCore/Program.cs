using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleWebApiAspNetCore.Repositories;
using SampleWebApiAspNetCore.Services;
using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;


namespace SampleWebApiAspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Initializes db.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                
                //var logger = services.GetRequiredService<ILogger<Program>>();
                //logger.LogInformation("hey this is the request!");

                try
                {
                    var context = services.GetRequiredService<FoodDbContext>();
                    var dbInitializer = services.GetRequiredService<ISeedDataService>();
                    dbInitializer.Initialize(context).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               
            // Begin - Coding for SeriLog Logging
               .UseSerilog(configureLogger:(context, configuration) => 
               {
                   configuration.Enrich.FromLogContext()
                        .Enrich.WithMachineName()                
                        .WriteTo.Console()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                        {
                            IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",

                            AutoRegisterTemplate = true,
                            NumberOfShards = 2,
                            NumberOfReplicas = 1
                        })
                        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                        .ReadFrom.Configuration(context.Configuration);

               })
                

               // End - Coding for Serilog Logging 
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}
