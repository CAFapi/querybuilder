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

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MatcherQueryBooleanTest
    {
        private readonly Dictionary<string, List<string>> _document;
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
            return filter.Map(MapKeyMatcherFieldSpec.Create).IsMatch(_document);
        }

        private static Dictionary<string, List<string>> GetDocument()
        {
            Dictionary<string, List<string>> document = new()
            {
                { SINGLE_VALUE, new List<string>() { "true" } },
                { MULTIPLE_VALUE, new List<string>() { "true", "true", "true" } }
            };

            return document;
        }
    }
}
