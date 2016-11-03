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

using System;
using System.Collections.Generic;
using TwitterDdd.Domain.Parsers;
using TwitterDdd.Domain.User;

namespace TwitterDdd.Domain.Message
{
    public interface IMessageAggregate
    {
        void Create(string content, string senderSubject);
        void Send();
        void Cancel();
    }

    public class MessageAggregate : IMessageAggregate
    {
        private readonly IMessageContentParser _messageContentParser;

        public MessageAggregate()
        {
            State = new MessageAggregateState
            {
                Content = string.Empty,
                HashTags = new List<string>(),
                Sender = string.Empty,
                Attachments = new List<AttachmentState>(),
                Status = MessageStatus.NotCreated,
                Likes = new List<LikeState>(),
                Shares = new List<ShareState>(),
                IsPinned = false
            };
            _messageContentParser = new MessageContentParser();
        }

        internal MessageAggregateState State { get; private set; }

        public void Create(string content, string senderSubject)
        {
            // 1. Check parameters & status.
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(content);
            }

            if (content.Length > 140)
            {
                throw new ArgumentOutOfRangeException("the content size cannot exceed 140 characters");
            }

            if (string.IsNullOrWhiteSpace(senderSubject))
            {
                throw new ArgumentNullException(senderSubject);
            }
            
            if (State.Status != MessageStatus.NotCreated)
            {
                throw new InvalidOperationException("message cannot be create twice");
            }

            // 2. Get user state & check subject exists.
            var userAggregate = new UserAggregate();
            userAggregate.Create(senderSubject);

            // 3. Parse the content to extract the hashtags.
            State.Content = content;
            State.Sender = senderSubject;
            State.Status = MessageStatus.ReadyToBeSent;
            State.HashTags = _messageContentParser.ExtractHashTags(content);
        }

        public void AddLike(string senderSubject)
        {
            if (State.Status != MessageStatus.ReadyToBeSent)
            {
                // TODO : Send code + message
                throw new InvalidOperationException();
            }

            
        }

        public void AddAttachment(AttachmentTypes type, string url)
        {

        }

        public void Send()
        {
            // 1. Check status
            if (State.Status != MessageStatus.ReadyToBeSent)
            {
                throw new InvalidOperationException("message is not ready to be sent");
            }
        }

        public void Cancel()
        {
            if (State.Status != MessageStatus.ReadyToBeSent)
            {
                // TODO : send code + message
                throw new InvalidOperationException();
            }
        }
    }
}
