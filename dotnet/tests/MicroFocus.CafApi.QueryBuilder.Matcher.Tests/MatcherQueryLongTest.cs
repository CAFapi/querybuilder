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
    public sealed class MatcherQueryLongTest
    {
        private readonly Dictionary<string, List<string>> _document;
        private static readonly string SINGLE_VALUE = "SINGLE_VALUE";
        private static readonly string SINGLE_VALUE_WITH_INVALID_ENTRY = "SINGLE_VALUE_WITH_INVALID_ENTRY";
        private static readonly string MULTIPLE_VALUE = "MULTIPLE_VALUE";
        private static readonly string MULTIPLE_VALUE_WITH_INVALID_ENTRY = "MULTIPLE_VALUE_WITH_INVALID_ENTRY";

        public MatcherQueryLongTest()
        {
            _document = GetDocument();
        }

        //  Equality testing
        [Fact]
        public void SingleValueFieldEqualsLong()
        {
            Filter<string> filter = FilterFactory.Equals(SINGLE_VALUE, 5);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryEqualsLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.Equals(SINGLE_VALUE_WITH_INVALID_ENTRY, 5);
                Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE_WITH_INVALID_ENTRY field to 5");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldNotEqualToLong()
        {
            Filter<string> filter = FilterFactory.NotEquals(SINGLE_VALUE, 6);
            Assert.True(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field to 6");
        }

        [Fact]
        public void MultiValuedFieldEqualsLong()
        {
            Filter<string> filter = FilterFactory.Equals(MULTIPLE_VALUE, 10);
            Assert.True(DocMatches(filter), "Should have matched 10 in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldNotEqualToLong()
        {
            Filter<string> filter = FilterFactory.NotEquals(MULTIPLE_VALUE, 1000);
            Assert.True(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field to 1000");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidEntryNotEqualToLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.NotEquals(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 5);
                Assert.True(DocMatches(filter), "Should not have matched 5 in MULTIPLE_VALUE_WITH_INVALID_ENTRY field to 5");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void MultiValuedFieldWithInvalidEntryEqualToLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.Equals(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 3);
                Assert.True(DocMatches(filter), "Should have matched 3 in MULTIPLE_VALUE_WITH_INVALID_ENTRY field to 3");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        //  In testing
        [Fact]
        public void SingleValueFieldHasLongIn()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, 4, 5, 6);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryHasNotGotLongIn()
        {
            try
            {
                Filter<string> filter = FilterFactory.In(SINGLE_VALUE_WITH_INVALID_ENTRY, 4, 5, 6);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldHasNotGotLongIn()
        {
            Filter<string> filter = FilterFactory.In(SINGLE_VALUE, 6, 7, 8);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasLongIn()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, 1, 10, 100);
            Assert.True(DocMatches(filter), "Should have matched 10 in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidEntryHasLongIn()
        {
            try
            {
                Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 1, 10, 100);
                Assert.True(DocMatches(filter), "Should have matched 10 in MULTIPLE_VALUE field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void MultiValuedFieldHasNotGotLongIn()
        {
            Filter<string> filter = FilterFactory.In(MULTIPLE_VALUE, 1000, 2000, 3000);
            Assert.False(DocMatches(filter), "Should have matched anything in MULTIPLE_VALUE field");
        }

        //  Between testing
        [Fact]
        public void SingleValueFieldHasLongBetween()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, 4L, 6L);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasLongBetweenInclusiveStart()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, 5L, 6L);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldHasLongBetweenInclusiveEnd()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, 1L, 5L);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryHasLongBetween()
        {
            try
            {
                Filter<string> filter = FilterFactory.Between(SINGLE_VALUE_WITH_INVALID_ENTRY, 4L, 6L);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldHasNotGotLongBetween()
        {
            Filter<string> filter = FilterFactory.Between(SINGLE_VALUE, 6L, 8L);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasLongBetween()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, 19L, 25L);
            Assert.True(DocMatches(filter), "Should have matched 20 in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasLongBetweenInclusiveStart()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, 20L, 25L);
            Assert.True(DocMatches(filter), "Should have matched 20 in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldHasLongBetweenInclusiveEnd()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, 5L, 20L);
            Assert.True(DocMatches(filter), "Should have matched 20 in MULTIPLE_VALUE field");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidEntryHasLongBetween()
        {
            try
            {
                Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 5L, 10L);
                Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void MultiValuedFieldHasNotGotLongBetween()
        {
            Filter<string> filter = FilterFactory.Between(MULTIPLE_VALUE, 25L, 50L);
            Assert.False(DocMatches(filter), "Should not have matched anything in MULTIPLE_VALUE field");
        }

        //  Less than testing
        [Fact]
        public void SingleValueFieldLessThanLong()
        {
            Filter<string> filter = FilterFactory.LessThan(SINGLE_VALUE, 20);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as < 20");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryLessThanLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.LessThan(SINGLE_VALUE_WITH_INVALID_ENTRY, 20);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldNotLessThanLong()
        {
            Filter<string> filter = FilterFactory.LessThan(SINGLE_VALUE, 2);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field as < 2");
        }

        [Fact]
        public void MultiValuedFieldLessThanLong()
        {
            Filter<string> filter = FilterFactory.LessThan(MULTIPLE_VALUE, 3);
            Assert.True(DocMatches(filter), "Should have matched 2 in MULTIPLE_VALUE field as < 3");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidEntryLessThanLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.LessThan(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 3);
                Assert.True(DocMatches(filter), "Should have matched 1 in MULTMULTIPLE_VALUE_WITH_INVALID_ENTRYIPLE_VALUE field as < 3");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void MultiValuedFieldNotLessThanLong()
        {
            Filter<string> filter = FilterFactory.LessThan(MULTIPLE_VALUE, 1);
            Assert.False(DocMatches(filter), "Should not have matched 2 in MULTIPLE_VALUE field as < 1");
        }

        //  Less than or equal to testing
        [Fact]
        public void SingleValueFieldLessThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, 6);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as <= 6");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryLessThanOrEqualToLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE_WITH_INVALID_ENTRY, 6);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldNotLessThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, 3);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field as <= 3");
        }

        [Fact]
        public void SingleValueFieldLessThanOrEqualToLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(SINGLE_VALUE, 5);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as <= 5");
        }

        [Fact]
        public void MultiValuedFieldLessThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, 3);
            Assert.True(DocMatches(filter), "Should have matched 2 in MULTIPLE_VALUE field as <= 3");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidLessThanOrEqualToLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 3);
                Assert.True(DocMatches(filter), "Should have matched 3 in MULTIPLE_VALUE_WITH_INVALID_ENTRY field as <= 3");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        [Fact]
        public void MultiValuedFieldNotLessThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, 1);
            Assert.False(DocMatches(filter), "Should not have matched 2 in MULTIPLE_VALUE field as <= 13");
        }

        [Fact]
        public void MultiValuedFieldLessThanOrEqualToLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(MULTIPLE_VALUE, 20);
            Assert.True(DocMatches(filter), "Should have matched 20 in MULTIPLE_VALUE field as <= 20");
        }

        //  Greater than testing
        [Fact]
        public void SingleValueFieldGreaterThanLong()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, 4);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as > 4");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryGreaterThanLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE_WITH_INVALID_ENTRY, 4);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanLong()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, 6);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field as > 6");
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE, 5);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field as > 5");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanLong()
        {
            Filter<string> filter = FilterFactory.GreaterThan(MULTIPLE_VALUE, 19);
            Assert.True(DocMatches(filter), "Should have matched 20 in MULTIPLE_VALUE field as > 19");
        }

        [Fact]
        public void MultiValuedFieldNotGreaterThanLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThan(MULTIPLE_VALUE, 20);
            Assert.False(DocMatches(filter), "Should not have matched 20 in MULTIPLE_VALUE field as > 20");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidNotGreaterThanLongWithEqualValue()
        {
            try
            {
                Filter<string> filter = FilterFactory.GreaterThan(MULTIPLE_VALUE_WITH_INVALID_ENTRY, 20);
                Assert.False(DocMatches(filter), "Should not have matched 20 in MULTIPLE_VALUE_WITH_INVALID_ENTRY field as > 20");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
        }

        //  Greater than or equal to testing
        [Fact]
        public void SingleValueFieldGreaterThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, 2);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as >= 2");
        }

        [Fact]
        public void SingleValueFieldWithInvalidEntryGreaterThanOrEqualToLong()
        {
            try
            {
                Filter<string> filter = FilterFactory.GreaterThan(SINGLE_VALUE_WITH_INVALID_ENTRY, 2);
                Assert.False(DocMatches(filter), "Should not have matched anything in SINGLE_VALUE_WITH_INVALID_ENTRY field");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"five\" as a long value");
            }
        }

        [Fact]
        public void SingleValueFieldNotGreaterThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, 6);
            Assert.False(DocMatches(filter), "Should not have matched 5 in SINGLE_VALUE field as >= 6");
        }

        [Fact]
        public void SingleValueFieldGreaterThanOrEqualToLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(SINGLE_VALUE, 5);
            Assert.True(DocMatches(filter), "Should have matched 5 in SINGLE_VALUE field as >= 5");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, 1);
            Assert.True(DocMatches(filter), "Should have matched 2 in MULTIPLE_VALUE field as >= 1");
        }

        [Fact]
        public void MultiValuedFieldNotGreaterThanOrEqualToLong()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, 1000);
            Assert.False(DocMatches(filter), "Should not have matched 2 in MULTIPLE_VALUE field as >= 1000");
        }

        [Fact]
        public void MultiValuedFieldGreaterThanOrEqualToLongWithEqualValue()
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, 2);
            Assert.True(DocMatches(filter), "Should have matched 2 in MULTIPLE_VALUE field as >= 2");
        }

        [Fact]
        public void MultiValuedFieldWithInvalidGreaterThanOrEqualToLongWithEqualValue()
        {
            try
            {
                Filter<string> filter = FilterFactory.GreaterThanOrEquals(MULTIPLE_VALUE, 2);
                Assert.True(DocMatches(filter), "Should have matched 2 in MULTIPLE_VALUE field as >= 2");
            }
            catch (FormatException)
            {
                Assert.True(false, "Should not have thrown an error when processing \"two\" as a long value");
            }
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
                { SINGLE_VALUE, new List<string> { "5" } },
                { SINGLE_VALUE_WITH_INVALID_ENTRY, new List<string> { "five" } },
                { MULTIPLE_VALUE, new List<string> { "2", "10", "20" } },
                { MULTIPLE_VALUE_WITH_INVALID_ENTRY, new List<string> { "1", "two", "3" } }
            };

            return document;
        }
    }
}