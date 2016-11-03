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
using System.Text.RegularExpressions;

namespace TwitterDdd.Domain.Parsers
{
    public interface IMessageContentParser
    {
        IEnumerable<string> ExtractHashTags(string content);
    }

    internal class MessageContentParser : IMessageContentParser
    {
        public IEnumerable<string> ExtractHashTags(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            var result = new List<string>();
            var regex = new Regex(@"(?<=#)\w+");
            var matches = regex.Matches(content);
            if (matches == null)
            {
                return result;
            }

            foreach (Match match in matches)
            {
                result.Add(match.Value);
            }

            return result;
        }
    }
}
