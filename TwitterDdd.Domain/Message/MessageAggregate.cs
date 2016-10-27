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

namespace TwitterDdd.Domain.Message
{
    public interface IMessageAggregate
    {
        void Create(string content, string senderSubject);
        void Send();
        void Cancel();
    }

    public class MessageAggregate
    {
        private readonly MessageAggregateState _state;

        public MessageAggregate()
        {
            _state = new MessageAggregateState();
            _state.Content = string.Empty;
            _state.HashTags = new List<string>();
            _state.Sender = null;
            _state.Attachments = new List<AttachmentState>();
            _state.Status = MessageStatus.NotCreated;
            _state.Likers = new List<LikerState>();
        }

        public void Create(string content, string senderSubject)
        {
            // 1. Check parameters & status.
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(content);
            }

            if (content.Length > 140)
            {
                // TODO : Throw invalid length exception.
            }

            if (string.IsNullOrWhiteSpace(senderSubject))
            {
                throw new ArgumentNullException(senderSubject);
            }
            
            if (_state.Status != MessageStatus.NotCreated)
            {
                // TODO : Send code + message
                throw new InvalidOperationException();
            }

            // 2. Parse the content to extract the hashtags.
            // TODO
            _state.HashTags = new[] { "hashtag" };


        }

        public void AddLike(string senderSubject)
        {
            if (_state.Status != MessageStatus.ReadyToBeSent)
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
            if (_state.Status != MessageStatus.ReadyToBeSent)
            {
                // TODO : send code + message
                throw new InvalidOperationException();
            }

            // 2. Send the message
        }

        public void Cancel()
        {
            if (_state.Status != MessageStatus.ReadyToBeSent)
            {
                // TODO : send code + message
                throw new InvalidOperationException();
            }
        }
    }
}
