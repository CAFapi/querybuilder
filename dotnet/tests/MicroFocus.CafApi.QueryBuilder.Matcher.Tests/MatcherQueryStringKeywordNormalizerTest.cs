/**
 * Copyright 2022-2026 Open Text.
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
    public sealed class MatcherQueryStringKeywordNormalizerTest : MatcherQueryStringTestBase
    {
        private static readonly string SINGLE_VALUE = "SINGLE_VALUE";
        private static readonly string MULTIPLE_VALUE = "MULTIPLE_VALUE";

        // Equality Testing
        [Fact]
        public void SingleValueFieldEqualsString()
        {
            TestEquals(SINGLE_VALUE, "TIGER");
            TestEquals(SINGLE_VALUE, "LIGER", false);
        }

        [Fact]
        public void SingleValueFieldNotEqualToString()
        {
            TestNotEquals(SINGLE_VALUE, "LIGER");
            TestNotEquals(SINGLE_VALUE, "TIGER", false);
        }

        [Fact]
        public void MultiValuedFieldEqualsString()
        {
            TestEquals(MULTIPLE_VALUE, "MOOSE");
            TestEquals(MULTIPLE_VALUE, "RHINOCEROS", false);
        }

        [Fact]
        public void MultiValuedFieldNotEqualToString()
        {
            TestNotEquals(MULTIPLE_VALUE, "RHINOCEROS");
            TestNotEquals(MULTIPLE_VALUE, "MOOSE", false);
        }

        // In Testing
        [Fact]
        public void SingleValueFieldHasStringInArray()
        {
            TestIn(SINGLE_VALUE, new List<string> { "MOUSE", "TIGER", "ELEPHANT" });
            TestIn(SINGLE_VALUE, false, new List<string> { "JAGUAR", "MUSTANG", "STINGRAY" });
        }

        [Fact]
        public void SingleValueFieldHasStringInList()
        {
            TestIn(SINGLE_VALUE, new List<string> { "MOUSE", "TIGER", "ELEPHANT" });
            TestIn(SINGLE_VALUE, false, new List<string> { "JAGUAR", "MUSTANG", "STINGRAY" });
        }

        [Fact]
        public void MultiValuedFieldHasStringInArray()
        {
            TestIn(MULTIPLE_VALUE, new List<string> { "BEAVER", "MOOSE", "VOLE" });
            TestIn(MULTIPLE_VALUE, false, new List<string> { "JAGUAR", "MUSTANG", "STINGRAY" });
        }

        [Fact]
        public void MultiValuedFieldHasStringInList()
        {
            TestIn(MULTIPLE_VALUE, new List<string> { "BEAVER", "MOOSE", "VOLE" });
            TestIn(MULTIPLE_VALUE, false, new List<string> { "JAGUAR", "MUSTANG", "STINGRAY" });
        }

        // Contains Testing
        [Fact]
        public void SingleValueFieldContainsString()
        {
            TestContains(SINGLE_VALUE, "IGE");
            TestContains(SINGLE_VALUE, "XXX", false);
        }

        [Fact]
        public void MultiValuedFieldContainsString()
        {
            TestContains(MULTIPLE_VALUE, "OOS");
            TestContains(MULTIPLE_VALUE, "XXX", false);
        }

        // starts with Testing
        [Fact]
        public void SingleValueFieldStartsWithString()
        {
            TestStartsWith(SINGLE_VALUE, "TIGE");
            TestStartsWith(SINGLE_VALUE, "LION", false);
        }

        [Fact]
        public void MultiValuedFieldStartsWithString()
        {
            TestStartsWith(MULTIPLE_VALUE, "MOOS");
            TestStartsWith(MULTIPLE_VALUE, "LEOPARD", false);
        }

        // Less than Testing
        [Fact]
        public void SingleValueFieldLessThanString()
        {
            TestLessThan(SINGLE_VALUE, "ZEBRA");
            TestLessThan(SINGLE_VALUE, "ANT", false);
        }

        [Fact]
        public void MultiValuedFieldLessThanString()
        {
            TestLessThan(MULTIPLE_VALUE, "BUFFALO");
            TestLessThan(MULTIPLE_VALUE, "AARDVARK", false);
        }

        // Less than or equal to Testing
        [Fact]
        public void SingleValueFieldLessThanOrEqualToString()
        {
            TestLessThanOrEqual(SINGLE_VALUE, "WOMBAT");
            TestLessThanOrEqual(SINGLE_VALUE, "BEAR", false);
            TestLessThanOrEqual(SINGLE_VALUE, "TIGER");
        }

        [Fact]
        public void MultiValuedFieldLessThanOrEqualToString()
        {
            TestLessThanOrEqual(MULTIPLE_VALUE, "HORSE");
            TestLessThanOrEqual(MULTIPLE_VALUE, "AARDVARK", false);
            TestLessThanOrEqual(MULTIPLE_VALUE, "ZEBRA");
        }

        // Greater than Testing
        [Fact]
        public void SingleValueFieldGreaterThanString()
        {
            TestGreaterThan(SINGLE_VALUE, "RAT");
            TestGreaterThan(SINGLE_VALUE, "WHALE", false);
            TestGreaterThan(SINGLE_VALUE, "TIGER", false);
        }

        [Fact]
        public void MultiValuedFieldGreaterThanString()
        {
            TestGreaterThan(MULTIPLE_VALUE, "WHALE");
            TestGreaterThan(MULTIPLE_VALUE, "ZEBRA", false);
        }

        // Greater than or equal to Testing
        [Fact]
        public void SingleValueFieldGreaterThanOrEqualToString()
        {
            TestGreaterThanOrEqual(SINGLE_VALUE, "SNAKE");
            TestGreaterThanOrEqual(SINGLE_VALUE, "WHALE", false);
            TestGreaterThanOrEqual(SINGLE_VALUE, "TIGER");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanOrEqualToString()
        {
            TestGreaterThanOrEqual(MULTIPLE_VALUE, "AARDVARK");
            TestGreaterThanOrEqual(MULTIPLE_VALUE, "ZEBU", false);
            TestGreaterThanOrEqual(MULTIPLE_VALUE, "ANACONDA");
        }

        // exists Testing
        [Fact]
        public void FieldExists()
        {
            Filter<string> filter = FilterFactory.Exists(MULTIPLE_VALUE);
            Assert.True(DocMatches(filter), "Should have found MULTIPLE_VALUE field");
        }

        [Fact]
        public void FieldIsNotEmpty()
        {
            Filter<string> filter = FilterFactory.Empty(MULTIPLE_VALUE);
            Assert.False(DocMatches(filter), "Should have found MULTIPLE_VALUE field is not empty");
        }

        // or Testing
        [Fact]
        public void OrTest()
        {
            Filter<string> filter = FilterFactory.Or(FilterFactory.Empty(MULTIPLE_VALUE), FilterFactory.Equals(SINGLE_VALUE, "TIGER"));
            Assert.True(DocMatches(filter), "Should have found SINGLE_VALUE field has TIGER");
        }

        [Fact]
        public void NorTest()
        {
            Filter<string> filter = FilterFactory.Or(
                FilterFactory.Equals(SINGLE_VALUE, "BOAT"),
                FilterFactory.Equals(SINGLE_VALUE, "PLANE"));
            Assert.False(DocMatches(filter), "Should not have found a match");
        }

        // and Testing
        [Fact]
        public void AndTest()
        {
            Filter<string> filter = FilterFactory.And(
                FilterFactory.Equals(MULTIPLE_VALUE, "ANACONDA"),
                FilterFactory.Equals(MULTIPLE_VALUE, "MOOSE"));
            Assert.True(DocMatches(filter), "Should have found both ANACONDA and MOOSE in MULTIPLE_VALUE field");
        }

        [Fact]
        public void NandTest()
        {
            Filter<string> filter = FilterFactory.And(
                FilterFactory.Equals(MULTIPLE_VALUE, "AARDVARK"),
                FilterFactory.Equals(MULTIPLE_VALUE, "MOOSE"));
            Assert.False(DocMatches(filter), "Should not have found both AARDVARK and MOOSE in MULTIPLE_VALUE field");
        }

        // not Testing
        [Fact]
        public void NotTest()
        {
            Filter<string> filter = FilterFactory.Not(FilterFactory.Equals(MULTIPLE_VALUE, "AARDVARK"));
            Assert.True(DocMatches(filter), "Should not have found AARDVARK in MULTIPLE_VALUE field");
        }

        protected override Dictionary<string, List<string>> GetDocument()
        {
            Dictionary<string, List<string>> newDocument = new()
            {
                { SINGLE_VALUE, new List<string> { "tiger" } },
                { MULTIPLE_VALUE, new List<string> { "anaconda", "moose", "zebra" } }
            };
            return newDocument;
        }
    }
}
