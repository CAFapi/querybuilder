/**
 * Copyright 2022 Micro Focus or one of its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MapKeyMatcherFieldSpec : IMatcherFieldSpec<Dictionary<string, List<string>>>
    {
        private readonly string _key;
        public static MapKeyMatcherFieldSpec Create(string key)
        {
            return new MapKeyMatcherFieldSpec(key);
        }
        public MapKeyMatcherFieldSpec(string key)
        {
            _key = key;
        }

        [return: NotNull]
        IEnumerable<IMatcherFieldValue> IMatcherFieldSpec<Dictionary<string, List<string>>>.GetFieldValues(Dictionary<string, List<string>> document)
        {
            if (document[_key] == null)
            {
                return Enumerable.Empty<IMatcherFieldValue>();
            }
            else
            {
                return document[_key].Select(value => new MatcherFieldValue(value));
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsCaseInsensitive
        {
            get
            {
                switch (this._key)
                {
                    case "SINGLE_VALUE":
                    case "MULTIPLE_VALUE":
                    case "SINGLE_VALUE_PATH":
                    case "MULTIPLE_VALUE_PATH":
                    case "SHAREPOINT_PATH":
                        return true;
                    default:
                        return false;
                }
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsTokenizedPath
        {
            get
            {
                switch (_key)
                {
                    case "SINGLE_VALUE_PATH":
                    case "MULTIPLE_VALUE_PATH":
                    case "SHAREPOINT_PATH":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override string ToString()
        {
            return "MapKeyMatcherFieldSpec [key=" + _key + "]";
        }

        class MatcherFieldValue : IMatcherFieldValue
        {
            private readonly string _value;
            public MatcherFieldValue(string value)
            {
                _value = value;
            }
            string IMatcherFieldValue.StringValue => _value;

            bool IMatcherFieldValue.IsReference => _value.StartsWith("ref:");
        }

    }
}
