﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client.Models;
using Kmd.Logic.Identity.Authorization;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Kmd.Logic.Cpr.Client.Sample
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            InitLogger();

            try
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddUserSecrets(typeof(Program).Assembly)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build()
                    .Get<AppConfiguration>();

                await Run(config).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                Log.Fatal(ex, "Caught a fatal unhandled exception");
            }
#pragma warning restore CA1031 // Do not catch general exception types
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static async Task Run(AppConfiguration configuration)
        {
            var validator = new ConfigurationValidator(configuration);
            if (!validator.Validate())
            {
                return;
            }

            using (var httpClient = new HttpClient())
            {
                var tokenProviderFactory = new LogicTokenProviderFactory(configuration.TokenProvider);
                var cprClient = new CprClient(httpClient, tokenProviderFactory, configuration.Cpr);

                var configs = await cprClient.GetAllCprConfigurationsAsync().ConfigureAwait(false);
                if (configs == null || configs.Count == 0)
                {
                    Log.Error("There are no CPR configurations defined for this subscription");
                    return;
                }

                CprProviderConfigurationModel cprProvider;
                if (configuration.Cpr.CprConfigurationId == Guid.Empty)
                {
                    if (configs.Count > 1)
                    {
                        Log.Error("There is more than one CPR configuration defined for this subscription");
                        return;
                    }

                    cprProvider = configs[0];
                    configuration.Cpr.CprConfigurationId = cprProvider.Id.Value;
                }
                else
                {
                    cprProvider = configs.FirstOrDefault(x => x.Id == configuration.Cpr.CprConfigurationId);

                    if (cprProvider == null)
                    {
                        Log.Error("Invalid CPR configuration id {Id}", configuration.Cpr.CprConfigurationId);
                        return;
                    }
                }

                Log.Information("Fetching {Cpr} using configuration {Name}", configuration.CprNumber, cprProvider.Name);

                var citizen = await cprClient.GetCitizenByCprAsync(configuration.CprNumber).ConfigureAwait(false);

                Log.Information("Citizen data: {@Citizen}", citizen);
            }
        }
    }
}