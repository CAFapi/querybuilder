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

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MapKeyMatcherFieldSpec : IMatcherFieldSpec<Dictionary<string, List<string>>>
    {
        private readonly string _key;
        public static MapKeyMatcherFieldSpec Create(string key)
        {
            return new MapKeyMatcherFieldSpec(key);
        }
        private MapKeyMatcherFieldSpec(string key)
        {
            _key = key;
        }

        [return: NotNull]
        IEnumerable<IMatcherFieldValue> IMatcherFieldSpec<Dictionary<string, List<string>>>.GetFieldValues(Dictionary<string, List<string>> document)
        {
            if (document.ContainsKey(_key))
            {
                return document[_key].Select(value => new MatcherFieldValue(value));
            }
            else
            {
                return Enumerable.Empty<IMatcherFieldValue>();
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsCaseInsensitive
        {
            get
            {
                return _key switch
                {
                    "SINGLE_VALUE"
                    or "MULTIPLE_VALUE"
                    or "SINGLE_VALUE_PATH"
                    or "MULTIPLE_VALUE_PATH"
                    or "SHAREPOINT_PATH"
                    or "WINDOWS_PATH"
                    or "IP_PATH"
                    or "NO_ROOT_PATH"
                    or "NO_ROOT_WINDOWS_PATH"
                    or "HOST_NAME_PATH"
                    or "FILE_PATH"
                    => true,
                    _
                    => false,
                };
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsTokenizedPath
        {
            get
            {
                return _key switch
                {
                    "SINGLE_VALUE_PATH"
                    or "MULTIPLE_VALUE_PATH"
                    or "SHAREPOINT_PATH"
                    or "WINDOWS_PATH"
                    or "IP_PATH"
                    or "NO_ROOT_PATH"
                    or "NO_ROOT_WINDOWS_PATH"
                    or "HOST_NAME_PATH"
                    or "FILE_PATH"
                    => true,
                    _
                    => false,
                };
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
