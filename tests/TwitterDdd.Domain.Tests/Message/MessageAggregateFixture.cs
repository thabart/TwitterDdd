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

using Moq;
using NServiceBus;
using System;
using System.Text;
using TwitterDdd.Domain.Message.Models;
using Xunit;

namespace TwitterDdd.Domain.Tests.Message
{
    public class MessageAggregateFixture
    {
        private IMessageAggregate _messageAggregate;
        private Mock<IMessageHandlerContext> _messageContextStub;

        [Fact]
        public void When_Passing_Invalid_Parameters_To_Create_Then_Exceptions_Are_Thrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            var builder = new StringBuilder();
            for (var i = 0; i < 200; i++) builder.Append('a');

            // ASSERTS
            Assert.Throws<ArgumentNullException>(() => _messageAggregate.Create(null, null));
            Assert.Throws<ArgumentNullException>(() => _messageAggregate.Create(string.Empty, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => _messageAggregate.Create(builder.ToString(), null));
        }

        private void InitializeFakeObjects()
        {
            _messageContextStub = new Mock<IMessageHandlerContext>();
            _messageAggregate = new MessageAggregate(_messageContextStub.Object);
        }
    }
}
