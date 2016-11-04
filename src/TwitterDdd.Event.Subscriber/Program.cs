#region copyright
// Copyright 2016 Habart Thierry
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

using NServiceBus;
using System;
using System.Threading.Tasks;
using TwitterDdd.Common.Message.Events;

namespace TwitterDdd.Event.Subscriber
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "TwitterDdd.Event.Subscriber";
            var edpConfiguration = new EndpointConfiguration("TwitterDdd.Event.Subscriber");
            edpConfiguration.UseSerialization<JsonSerializer>();
            edpConfiguration.EnableInstallers();
            edpConfiguration.UsePersistence<InMemoryPersistence>();
            edpConfiguration.SendFailedMessagesTo("error");
            var edpInstance = await Endpoint.Start(edpConfiguration).ConfigureAwait(false);
            // Subscribe message
            var transport = edpConfiguration.UseTransport<MsmqTransport>();
            transport.ConnectionString(string.Empty);
            transport.Transactions(TransportTransactionMode.ReceiveOnly);
            var routing = transport.Routing();
            routing.RegisterPublisher(
                assembly: typeof(MessageCreatedEvent).Assembly,
                publisherEndpoint: "TwitterDdd.Command.Subscriber");
            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await edpInstance.Stop().ConfigureAwait(false);
            }
        }
    }
}
