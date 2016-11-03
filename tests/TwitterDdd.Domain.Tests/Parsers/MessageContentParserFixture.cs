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
using System.Linq;
using TwitterDdd.Domain.Message.Parsers;
using Xunit;

namespace TwitterDdd.Domain.Tests.Parsers
{
    public class MessageContentParserFixture
    {
        private IMessageContentParser _messageContentParser;

        [Fact]
        public void When_Passing_Null_Parameter_Then_Exception_Is_Thrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => _messageContentParser.ExtractHashTags(null));
            Assert.Throws<ArgumentNullException>(() => _messageContentParser.ExtractHashTags(string.Empty));
        }

        [Fact]
        public void When_Message_Doesnt_Contain_HashTags_Then_Empty_Array_Is_Returned()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACT
            var result = _messageContentParser.ExtractHashTags("message");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(!result.Any());
        }

        [Fact]
        public void When_Message_Contain_Several_HashTags_Then_List_Is_Returned()
        {
            // ARRANGE
            var hashTags = new[] { "hashtag", "hashtag2" };
            InitializeFakeObjects();

            // ACT
            var result = _messageContentParser.ExtractHashTags("message #hashtag #hashtag2");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.All(r => hashTags.Contains(r)));
        }

        private void InitializeFakeObjects()
        {
            _messageContentParser = new MessageContentParser();
        }
    }
}
