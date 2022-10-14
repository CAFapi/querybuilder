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
    public sealed class MatcherQueryStringTest
    {
        private readonly Dictionary<string, List<string>> _document;
        private static readonly string SINGLE_VALUE = "SINGLE_VALUE";
        private static readonly string MULTIPLE_VALUE = "MULTIPLE_VALUE";
        private static readonly string IS_EMPTY = "IS_EMPTY";
        private static readonly string IS_ACTUALLY_EMPTY = "IS_ACTUALLY_EMPTY";
        private static readonly string REFERENCE_FIELD = "REFERENCE_FIELD";

        public MatcherQueryStringTest()
        {
            _document = GetDocument();
        }

        //  Equality testing
        [Fact]
        public void SingleValueFieldEqualsString()
        {
            Filter<string> filter = FilterFactory.Equals(SINGLE_VALUE, "TIGER");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldNotEqualToString()
        {
            Filter<string> filter = FilterFactory.NotEquals(SINGLE_VALUE, "LIGER");
            Assert.True(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field to LIGER");
        }

        [Fact]
        public void MultiValuedFieldEqualsString()
        {
            Filter<string> filter = FilterFactory.Equals(MULTIPLE_VALUE, "MOOSE");
            Assert.True(DocMatches(filter), "Should have matched MOOSE in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldNotEqualToString()
        {
            Filter<string> filter = FilterFactory.NotEquals(MULTIPLE_VALUE, "RHINOCEROS");
            Assert.True(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field to RHINOCEROS");
        }

        //  In testing
        [Fact]
        public void SingleValueFieldHasStringInArray()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, "MOUSE", "TIGER", "ELEPHANT");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasStringInList()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, new List<string>() { "MOUSE", "TIGER", "ELEPHANT" });
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldDoesNotHaveStringInArray()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, "JAGUAR", "MUSTANG", "STINGRAY");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldDoesNotHaveStringInList()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, new List<string>() { "JAGUAR", "MUSTANG", "STINGRAY" });
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasStringInArray()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, "BEAVER", "MOOSE", "VOLE");
            Assert.True(DocMatches(filter), "Should have matched MOOSE in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasStringInList()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, new List<string>() { "BEAVER", "MOOSE", "VOLE" });
            Assert.True(DocMatches(filter), "Should have matched MOOSE in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldDoesNotHaveStringInArray()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, "JAGUAR", "MUSTANG", "STINGRAY");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldDoesNotHaveStringInList()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, new List<string>() { "JAGUAR", "MUSTANG", "STINGRAY" });
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        // Contains testing
        [Fact]
        public void SingleValueFieldContainsString()
        {
            Filter<string> filter = FilterFactory.Contains(SINGLE_VALUE, "IGE");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as containing IGE");
        }

        [Fact]
        public void SingleValueFieldDoesNotContainsString()
        {
            Filter<string> filter = FilterFactory.Contains(SINGLE_VALUE, "XXX");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as containing XXX");
        }

        [Fact]
        public void MultiValuedFieldContainsString()
        {
            Filter<string> filter = FilterFactory.Contains(MULTIPLE_VALUE, "OOS");
            Assert.True(DocMatches(filter), "Should have matched MOOSE in MULTIPLE_VALUE field as containing OOS");
        }

        [Fact]
        public void MultiValuedFieldDoesNotContainsString()
        {
            Filter<string> filter = FilterFactory.Contains(MULTIPLE_VALUE, "XXX");
            Assert.False(DocMatches(filter), "Should not have matched MOOSE in MULTIPLE_VALUE field as containing XXX");
        }

        // starts with testing
        [Fact]
        public void SingleValueFieldStartsWithString()
        {
            Filter<string> filter = FilterFactory.StartsWith(SINGLE_VALUE, "TIGE");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as starting with TIGE");
        }

        [Fact]
        public void SingleValueFieldDoesNotStartWithString()
        {
            Filter<string> filter = FilterFactory.StartsWith(SINGLE_VALUE, "LION");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as starting with LION");
        }

        [Fact]
        public void MultiValuedFieldStartsWithString()
        {
            Filter<string> filter = FilterFactory.StartsWith(MULTIPLE_VALUE, "MOOS");
            Assert.True(DocMatches(filter), "Should have matched anything in MULTIPLE_VALUE field as starting with MOOS");
        }

        [Fact]
        public void MultiValuedFieldDoesNotStartWithString()
        {
            Filter<string> filter = FilterFactory.StartsWith(MULTIPLE_VALUE, "LEOPARD");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field as starting with LEOPARD");
        }

        //  Between testing
        [Fact]
        public void SingleValueFieldHasStringBetween()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, "ANT", "ZEBRA");
            Assert.True(DocMatches(filter), "Should have matched 'TIGER' in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasStringBetweenInclusiveStart()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, "TIGER", "ZEBRA");
            Assert.True(DocMatches(filter), "Should have matched 'TIGER' in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasStringBetweenInclusiveEnd()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, "ANT", "TIGER");
            Assert.True(DocMatches(filter), "Should have matched 'TIGER' in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasNotGotStringBetween()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, "ANT", "MONKEY");
            Assert.False(DocMatches(filter), "Should not have matched 'TIGER' in SINGLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasStringBetween()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "MONKEY", "TIGER");
            Assert.True(DocMatches(filter), "Should have matched 'MOOSE' in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasStringBetweenInclusiveStart()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "MOOSE", "MOTH");
            Assert.True(DocMatches(filter), "Should have matched 'MOOSE' in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasStringBetweenInclusiveEnd()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "ALPACA", "ANACONDA");
            Assert.True(DocMatches(filter), "Should have matched 'ANACONDA' in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasNotGotStringBetween()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "ALPACA", "AMUR LEOPARD");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenNullStart()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, null, "AMUR LEOPARD");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenNullEnd()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "AMUR LEOPARD", null);
            Assert.True(DocMatches(filter), "Should have matched values in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenNullRange()
        {
            string? start = null;
            string? end = null;
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, start, end);
            Assert.True(DocMatches(filter), "Should not have checked MULTIPLE_VALUE field exists");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenEmptyStart()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "    ", "TIGER");
            Assert.True(DocMatches(filter), "Should have matched values in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenEmptyStart2()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "    ", "ALPACA");
            Assert.False(DocMatches(filter), "Should not have matched any value in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenEmptyStart3()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "", "TIGER");
            Assert.True(DocMatches(filter), "Should have matched values in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenEmptyEnd()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "MONKEY", "  ");
            Assert.False(DocMatches(filter), "Should not have matched any value in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldStringBetweenDBLQuoteInEnd()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, "ALPACA", "AMUR \"LEOPARD");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        //  Less than testing
        [Fact]
        public void SingleValueFieldLessThanString()
        {
            Filter<string> filter = FilterFactory.LessThan(SINGLE_VALUE, "ZEBRA");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as < ZEBRA");
        }

        [Fact]
        public void SingleValueFieldNotLessThanString()
        {
            Filter<string> filter = FilterFactory.LessThan(SINGLE_VALUE, "ANT");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as < ANT");
        }

        [Fact]
        public void MultiValuedFieldLessThanString()
        {
            Filter<string> filter = FilterFactory.LessThan(MULTIPLE_VALUE, "BUFFALO");
            Assert.True(DocMatches(filter), "Should have matched ANACONDA in MULTIPLE_VALUE field as < BUFFALO");
        }

        [Fact]
        public void MultiValuedFieldNotLessThanString()
        {
            Filter<string> filter = FilterFactory.LessThan(MULTIPLE_VALUE, "AARDVARK");
            Assert.False(DocMatches(filter), "Should not have matched ANACONDA in MULTIPLE_VALUE field as < AARDVARK");
        }

        //  Less than or equal to testing
        [Fact]
        public void SingleValueFieldLessThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, "WOMBAT");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as <= WOMBAT");
        }

        [Fact]
        public void SingleValueFieldNotLessThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, "BEAR");
            Assert.False(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as <= BEAR");
        }

        [Fact]
        public void SingleValueFieldLessThanOrEqualToStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, "TIGER");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as <= TIGER");
        }

        [Fact]
        public void MultiValuedFieldLessThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, "HORSE");
            Assert.True(DocMatches(filter), "Should have matched ANACONDA in MULTIPLE_VALUE field as <= HORSE");
        }

        [Fact]
        public void MultiValuedFieldNotLessThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, "AARDVARK");
            Assert.False(DocMatches(filter), "Should have matched anything in MULTIPLE_VALUE field as <= AARDVARK");
        }

        [Fact]
        public void MultiValuedFieldLessThanOrEqualToStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, "ZEBRA");
            Assert.True(DocMatches(filter), "Should have matched ZEBRA in MULTIPLE_VALUE field as <= ZEBRA");
        }

        //  Greater than testing
        [Fact]
        public void SingleValueFieldGreaterThanString()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, "RAT");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as > RAT");
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanString()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, "WHALE");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as > WHALE");
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, "TIGER");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as > TIGER");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanString()
        {
            Filter<string> filter = FilterFactory.GreaterThan(MULTIPLE_VALUE, "WHALE");
            Assert.True(DocMatches(filter), "Should have matched ZEBRA in MULTIPLE_VALUE field as > WHALE");
        }

        [Fact]
        public void MultiValuedFieldNotGreaterThanStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThan(MULTIPLE_VALUE, "ZEBRA");
            Assert.False(DocMatches(filter), "Should not have matched ZEBRA in MULTIPLE_VALUE field as > ZEBRA");
        }

        //  Greater than or equal to testing
        [Fact]
        public void SingleValueFieldGreaterThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, "SNAKE");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as >= SNAKE");
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, "WHALE");
            Assert.False(DocMatches(filter), "Should not have matched TIGER in SINGLE_VALUE field as >= WHALE");
        }

        [Fact]
        public void SingleValueFieldGreaterThanOrEqualToStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, "TIGER");
            Assert.True(DocMatches(filter), "Should have matched TIGER in SINGLE_VALUE field as >= TIGER");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, "AARDVARK");
            Assert.True(DocMatches(filter), "Should have matched ANACONDA in MULTIPLE_VALUE field as >= AARDVARK");
        }

        [Fact]
        public void MultiValuedFieldNotGreaterThanOrEqualToString()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, "ZEBU");
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field as >= ZEBU");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanOrEqualToStringWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, "ANACONDA");
            Assert.True(DocMatches(filter), "Should have matched ANACONDA in MULTIPLE_VALUE field as >= ANACONDA");
        }

        //  exists testing
        [Fact]
        public void FieldExists()
        {
            Filter<string> filter = FilterFactory.Exists(MULTIPLE_VALUE);
            Assert.True(DocMatches(filter), "Should have found MULTIPLE_VALUE field");
        }

        [Fact]
        public void FieldNotExists()
        {
            Filter<string> filter = FilterFactory.Exists("NOT_EXISTS");
            Assert.False(DocMatches(filter), "Should not have found NOT_EXISTS field");
        }

        // empty testing
        [Fact]
        public void FieldIsEmpty()
        {
            Filter<string> filter = FilterFactory.Empty(IS_EMPTY);
            Assert.True(DocMatches(filter), "Should have found IS_EMPTY field is empty");
        }

        [Fact]
        public void FieldIsNotEmpty()
        {
            Filter<string> filter = FilterFactory.Empty(MULTIPLE_VALUE);
            Assert.False(DocMatches(filter), "Should have found MULTIPLE_VALUE field is not empty");
        }

        [Fact]
        public void FieldIsActuallyEmpty()
        {
            Filter<string> filter = FilterFactory.Empty(IS_ACTUALLY_EMPTY);
            Assert.True(DocMatches(filter), "Should have found IS_ACTUALLY_EMPTY field is empty");
        }

        // or testing
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

        // and testing
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

        // not testing
        [Fact]
        public void NotTest()
        {
            Filter<string> filter = FilterFactory.Not(FilterFactory.Equals(MULTIPLE_VALUE, "AARDVARK"));
            Assert.True(DocMatches(filter), "Should not have found AARDVARK in MULTIPLE_VALUE field");
        }

        // testing a document with a reference field
        [Fact]
        public void GetStringValueOnReferenceFieldValueDoesNotThrowRuntimeException()
        {
            //TODO runtimeexception??
            //try {
            Filter<string> filter = FilterFactory.Contains(REFERENCE_FIELD, "CHAMELEON");
            Assert.False(DocMatches(filter), "Should not have found CHAMELEON in REFERENCE_FIELD field");
            //} catch (RuntimeException e) {
            //    fail("Should not have thrown a RuntimeException when calling GetStringValue on a reference field ");
            //}
        }

        private bool DocMatches(Filter<string> filter)
        {
            Filter<MapKeyMatcherFieldSpec> mappedFilter = MatcherTestHelper.MapFilter(filter);
            return mappedFilter.IsMatch(_document);
        }

        private static Dictionary<string, List<string>> GetDocument()
        {
            Dictionary<string, List<string>> document = new()
            {
                { SINGLE_VALUE, new List<string>() { "TIGER" } },
                { MULTIPLE_VALUE, new List<string>() { "ANACONDA", "MOOSE", "ZEBRA" } },
                { IS_EMPTY, new List<string>() { "" } },
                { REFERENCE_FIELD, new List<string>() { "ref:CHAMELEON" } },
                { IS_ACTUALLY_EMPTY, new List<string>() }
            };

            return document;
        }
    }
}