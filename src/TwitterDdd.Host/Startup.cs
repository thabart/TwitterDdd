#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NServiceBus;
using TwitterDdd.DataAccess.InMemory;
using TwitterDdd.Domain.Message.Commands;
using TwitterDdd.Domain.Message.Repositories;

namespace TwitterDdd.Host
{
    public class Startup
    {
        private const string EndPointName = "TwitterDdd";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            /*
            builder.RegisterInstance<IMessageAggregateRepository>(new MessageAggregateRepository());
            var container = builder.Build();
            */
            // Configure NServiceBus
            // 1. Configure endpoint.
            var edpConfiguration = new EndpointConfiguration(EndPointName);
            edpConfiguration.UseSerialization<JsonSerializer>();
            edpConfiguration.EnableInstallers();
            edpConfiguration.UsePersistence<InMemoryPersistence>();
            edpConfiguration.SendFailedMessagesTo("error");
            /*
            edpConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(container);
                });*/
            // 2. Configure transport & routing
            var transport = edpConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.None);
            var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(SendMessageCommand).Assembly,
                destination: EndPointName);
            // 3. Register message session.
            var edpInstance = Endpoint.Start(edpConfiguration).GetAwaiter().GetResult();
            services.AddSingleton<IMessageSession>(edpInstance);

            // Configure MVC
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
