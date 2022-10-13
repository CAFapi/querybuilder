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
using System;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MatcherQueryBooleanTest
    {
        private Dictionary<string, List<string>> _document;
        private static string SINGLE_VALUE = "SINGLE_VALUE";
        private static string MULTIPLE_VALUE = "MULTIPLE_VALUE";

        public MatcherQueryBooleanTest()
        {
            _document = GetDocument();
        }

        //  Equality testing
        [Fact]
        public void SingleValueFieldEqualsBoolean()
        {
            Filter<string> filter = FilterFactory.Equals(SINGLE_VALUE, true);
            Assert.True(DocMatches(filter), "Should have matched true in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldNotEqualToBoolean()
        {
            Filter<string> filter = FilterFactory.NotEquals(SINGLE_VALUE, false);
            Assert.True(DocMatches(filter), "Should not have matched true in SINGLE_VALUE field to false");
        }

        [Fact]
        public void MultiValuedFieldEqualsBoolean()
        {
            Filter<string> filter = FilterFactory.Equals(MULTIPLE_VALUE, true);
            Assert.True(DocMatches(filter), "Should have matched true in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldDoesNotEqualBoolean()
        {
            Filter<string> filter = FilterFactory.NotEquals(MULTIPLE_VALUE, false);
            Assert.True(DocMatches(filter), "Should not have matched true in MULTIPLE_VALUE field to false");
        }

        private bool DocMatches(Filter<string> filter)
        {
            //var method = typeof(MapKeyMatcherFieldSpec).GetConstructor(new[] { typeof(string) });
            var method = typeof(MapKeyMatcherFieldSpec).GetMethod("Create");
            if (method == null)
            { throw new ArgumentNullException("Mapping function not found"); }
            Func<string, MapKeyMatcherFieldSpec> create = (Func<string, MapKeyMatcherFieldSpec>)Delegate.CreateDelegate(typeof(Func<string, MapKeyMatcherFieldSpec>), method);
            var mappedFilter = FilterMapper<string, MapKeyMatcherFieldSpec>.Map(filter, create);
            return mappedFilter.IsMatch(_document);
        }

        private static Dictionary<string, List<string>> GetDocument()
        {
            Dictionary<string, List<string>> document = new();
            document.Add(SINGLE_VALUE, new List<string>() { "true" });
            document.Add(MULTIPLE_VALUE, new List<string>() { "true", "true", "true" });

            return document;
        }
    }
}