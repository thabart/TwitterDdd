﻿#region copyright
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

using System.Threading.Tasks;
using NServiceBus;
using TwitterDdd.Domain.Message.Models;
using TwitterDdd.Domain.Message.Repositories;
using TwitterDdd.Common.Message.Commands;

namespace TwitterDdd.Command.Subscriber
{
    public class MessageCommandHandler : IHandleMessages<SendMessageCommand>
    {
        private readonly IMessageAggregateRepository _messageAggregateRepository;

        public MessageCommandHandler(IMessageAggregateRepository messageAggregateRepository)
        {
            _messageAggregateRepository = messageAggregateRepository;
        }

        public async Task Handle(SendMessageCommand message, IMessageHandlerContext context)
        {
            var messageAggregate = new MessageAggregate(context);
            messageAggregate.Create(message.Content, message.SenderSubject);
            await messageAggregate.Send();
            await _messageAggregateRepository.InsertMessage(messageAggregate);
        }
    }
}
