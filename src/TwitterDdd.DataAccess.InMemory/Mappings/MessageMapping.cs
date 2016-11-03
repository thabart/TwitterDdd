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
using System.Linq;
using TwitterDdd.DataAccess.InMemory.MessageDomain;
using TwitterDdd.DataAccess.InMemory.UserDomain;
using TwitterDdd.Domain.Message.Models;

namespace TwitterDdd.DataAccess.InMemory.Mappings
{
    internal static class MessageMapping
    {
        public static Message ToModel(this MessageAggregateState message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var result = new Message
            {
                Content = message.Content,
                IsPinned = message.IsPinned,
                Status = (int)message.Status,
                Sender = new User
                {
                    Id = message.Sender
                },
                CreateDateTime = message.CreateDateTime,
                Attachments = new List<Attachment>(),
                HashTags = new List<HashTag>()
            };

            if (message.Attachments != null)
            {
                result.Attachments = message.Attachments.Select(a => new Attachment
                {
                    Type = (int)a.Type,
                    Url = a.Url
                });
            }

            if (message.HashTags != null)
            {
                result.HashTags = message.HashTags.Select(h => new HashTag
                {
                    Value = h
                });
            }

            if (message.Likes != null)
            {
                result.Likes = message.Likes.Select(l => new Like
                {
                    CreateDateTime = l.CreateDateTime,
                    User = new User
                    {
                        Id = l.Subject
                    }
                });
            }

            if (message.Shares != null)
            {
                result.Shares = message.Shares.Select(s => new Share
                {
                    CreateDateTime = s.CreateDateTime,
                    User = new User
                    {
                        Id = s.Subject
                    }
                });
            }

            return result;
        }

        public static MessageAggregateState ToDomain(this Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var result = new MessageAggregateState
            {
                Content = message.Content,
                IsPinned = message.IsPinned,
                Status = (MessageStatus)message.Status,
                Sender = message.Sender.Id,
                CreateDateTime = message.CreateDateTime,
                Attachments = new List<AttachmentState>(),
                HashTags = new List<string>()
            };

            if (message.Attachments != null)
            {
                result.Attachments = message.Attachments.Select(a => new AttachmentState
                {
                    Type = (AttachmentTypes)a.Type,
                    Url = a.Url
                });
            }

            if (message.HashTags != null)
            {
                result.HashTags = message.HashTags.Select(h => h.Value);
            }

            if (message.Likes != null)
            {
                result.Likes = message.Likes.Select(l => new LikeState
                {
                    CreateDateTime = l.CreateDateTime,
                    Subject = l.User.Id 
                });
            }

            if (message.Shares != null)
            {
                result.Shares = message.Shares.Select(s => new ShareState
                {
                    CreateDateTime = s.CreateDateTime,
                    Subject = s.User.Id
                });
            }

            return result;
        }
    }
}
