/**
 * Copyright 2022-2023 Open Text.
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
global using Xunit;
global using MicroFocus.CafApi.QueryBuilder.Mapper;

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public static class MatcherTestHelper
    {
        public static string Print(Dictionary<string, List<string>> document)
        {
            return "{" + string.Join(", ", document.Select(kv => kv.Key + " = " + Print(kv.Value)).ToArray()) + "}";
        }

        public static string Print(List<string> values)
        {
            return "[" + string.Join(", ", values) + "]";
        }
    }
}
