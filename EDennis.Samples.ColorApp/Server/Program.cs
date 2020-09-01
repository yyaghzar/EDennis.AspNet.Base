﻿using EDennis.NetStandard.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Linq;

namespace EDennis.Samples.ColorApp.Server {
    public class Program {
        public static void Main(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .GetLoggerFromConfiguration<Program>("Logging:Serilog");

            if (args.Contains("/idp-config")) {
                Log.Information("Generating IDP Config file...");
                SeedDataGenerator.GenerateIdpConfigStub<Startup>(5000, 44336, IdpConfigType.AuthorizationCode);
                Log.Information("Exiting...");
                return;
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
    }
}
