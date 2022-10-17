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
global using Xunit;

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public static class MatcherTestHelper
    {
        public static Filter<MapKeyMatcherFieldSpec> MapFilter(Filter<string> filter)
        {
            Func<string, MapKeyMatcherFieldSpec> create = GetMappingFunction();
            return FilterMapper<string, MapKeyMatcherFieldSpec>.Map(filter, create);
        }

        public static Func<string, MapKeyMatcherFieldSpec> GetMappingFunction()
        {
            // TODO: check this: static Create function vs constructor
            //var method = typeof(MapKeyMatcherFieldSpec).GetConstructor(new[] { typeof(string) });
            var method = typeof(MapKeyMatcherFieldSpec).GetMethod("Create");
            return (Func<string, MapKeyMatcherFieldSpec>)Delegate.CreateDelegate(typeof(Func<string, MapKeyMatcherFieldSpec>), method!);
        }

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