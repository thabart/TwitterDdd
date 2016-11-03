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

using System;
using System.Collections.Generic;
using TwitterDdd.DataAccess.InMemory.UserDomain;

namespace TwitterDdd.DataAccess.InMemory.MessageDomain
{
    internal class Message
    {
        public string Content { get; set; }
        public int Status { get; set; }
        public bool IsPinned { get; set; }
        public User Sender { get; set; }
        public DateTime CreateDateTime { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }
        public IEnumerable<HashTag> HashTags { get; set; }
        public IEnumerable<Share> Shares { get; set; }
        public IEnumerable<Like> Likes { get; set; }
    }
}