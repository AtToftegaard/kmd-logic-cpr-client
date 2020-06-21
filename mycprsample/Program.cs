using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Client;
using Kmd.Logic.Cpr.Client.Models;
using Kmd.Logic.Identity.Authorization;
using Microsoft.Extensions.Configuration;

namespace mycprsample {
    class Program {
        static async Task Main(string[] args) {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

            var tokenProviderConfig = new LogicTokenProviderOptions();
            tokenProviderConfig.AuthorizationScope = configuration.GetSection("TokenProvider")["AuthorizationScope"];
            tokenProviderConfig.ClientId = configuration.GetSection("TokenProvider")["ClientId"];
            tokenProviderConfig.ClientSecret = configuration.GetSection("TokenProvider")["ClientSecret"];

            var cprConfig = new CprOptions();
            cprConfig.CprConfigurationId = Guid.Parse(configuration.GetSection("Cpr")["CprConfigurationId"]);
            cprConfig.SubscriptionId = Guid.Parse(configuration.GetSection("Cpr")["SubscriptionId"]);

            using (var httpClient = new HttpClient())
            using (var tokenProviderFactory = new LogicTokenProviderFactory(tokenProviderConfig)) {
                
                //var tokenProvider = tokenProviderFactory.GetProvider(httpClient);
                //CancellationTokenSource source = new CancellationTokenSource();
                //CancellationToken token = source.Token;
                //var accessToken = await tokenProvider.GetAuthenticationHeaderAsync(token).ConfigureAwait(true);
                
                var cprClient = new CprClient(httpClient, tokenProviderFactory, cprConfig);
                var citizen = await cprClient.GetCitizenByCprAsync("0101010391").ConfigureAwait(false);
                var cprConfigurations = await cprClient.GetAllCprConfigurationsAsync().ConfigureAwait(false);
                Console.Write(cprConfigurations);
            }
        }
    }
}
