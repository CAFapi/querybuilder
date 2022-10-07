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

using Moq;
using Xunit;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Builders.Internal.Tests
{
    public sealed class FilterWriterTest
    {
        private readonly Mock<IJsonBuilder> _jsonBuilder;

        public FilterWriterTest()
        {
            _jsonBuilder = new Mock<IJsonBuilder>(MockBehavior.Strict);
        }

        [Fact]
        public void TestTransformEqualsTrue()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("EMBEDDED"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteBoolean(true));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("EMBEDDED", true);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsFalse()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("EMBEDDED"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteBoolean(false));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("EMBEDDED", false);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEquals()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("classified"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("CLASSIFICATION", "classified");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformPhraseEquals()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("\"class\""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("CLASSIFICATION", "\"class\"");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsSpecialChar()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("clas{}<>=:/()\\sified"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("CLASSIFICATION", "clas{}<>=:/()\\sified");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsAlphanumeric()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("cl4ss1fi13ed"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("CLASSIFICATION", "cl4ss1fi13ed");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsLong()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("DOC_COUNT", 5L);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsMaxLong()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("DOC_COUNT", Int64.MaxValue);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformEqualsMinLong()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.Equals("DOC_COUNT", Int64.MinValue);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsTrue()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("EMBEDDED"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteBoolean(true));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.NotEquals("EMBEDDED", true);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsFalse()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("EMBEDDED"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteBoolean(false));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.NotEquals("EMBEDDED", false);
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEquals()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("classified"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "classified");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformPhraseNotEquals()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("\"class\""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "\"class\"");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsSpecialChar()
        {
            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("clas{}<>=:/()\\sified"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "clas{}<>=:/()\\sified");
            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsAlphanumeric()
        {
            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "cl4ss1fi13ed");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("CLASSIFICATION"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("cl4ss1fi13ed"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsLong()
        {
            Filter<string> clause = FilterFactory.NotEquals("DOC_COUNT", 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsMaxLong()
        {
            Filter<string> clause = FilterFactory.NotEquals("DOC_COUNT", Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEqualsMinLong()
        {
            Filter<string> clause = FilterFactory.NotEquals("DOC_COUNT", Int64.MinValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("!="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInLong()
        {
            long[] values = { 1L, 2L, 3L };

            Filter<string> clause = FilterFactory.In("DOC_COUNT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            foreach (long? l in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(l.Value));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInLong20Elements()
        {
            long[] values = new long[20];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = i;
            }

            Filter<string> clause = FilterFactory.In("DOC_COUNT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            foreach (long? l in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(l.Value));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInLongMaxValueandMinValue()
        {
            long[] values = { Int64.MaxValue, Int64.MinValue };

            Filter<string> clause = FilterFactory.In("DOC_COUNT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            foreach (long? l in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(l.Value));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInLongEmptyArray()
        {
            long[] values = Array.Empty<long>();

            Filter<string> clause = FilterFactory.In("DOC_COUNT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in-numbers"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInString()
        {
            String[] values = { "test", "testing" };

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            foreach (String s in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInStringSpecialCharacters()
        {
            String[] values = { "t3@*&^%$t", "testing" };

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            foreach (String s in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInStringAndNumbers()
        {
            String[] values = { "test", "testing", "123", "456" };

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            foreach (String s in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInString20Elements()
        {
            String[] values = new String[20];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = "test" + i;
            }

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            foreach (String s in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInStringAsSingleChars()
        {
            String[] values = { "a", "b", "c" };

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            foreach (String s in values)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueInStringEmptyArray()
        {
            string[] values = Array.Empty<string>();

            Filter<string> clause = FilterFactory.In("DATA_SUBJECT", values);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("in-strings"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueContains()
        {
            Filter<string> clause = FilterFactory.Contains("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("contains"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueContainsSingleCharacterAsString()
        {
            Filter<string> clause = FilterFactory.Contains("DATA_SUBJECT", "a");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("contains"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("a"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueContainsEmptyString()
        {
            Filter<string> clause = FilterFactory.Contains("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("contains"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueContainsStringSpecialCharacters()
        {
            Filter<string> clause = FilterFactory.Contains("DATA_SUBJECT", "@#~$%^&*");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("contains"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~$%^&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueStartsWith()
        {
            Filter<string> clause = FilterFactory.StartsWith("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("starts-with"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueStartsWithSingleCharacterAsString()
        {
            Filter<string> clause = FilterFactory.StartsWith("DATA_SUBJECT", "a");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("starts-with"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("a"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueStartsWithNumberAsString()
        {
            Filter<string> clause = FilterFactory.StartsWith("DATA_SUBJECT", "123");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("starts-with"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("123"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueStartsWithEmptyString()
        {
            Filter<string> clause = FilterFactory.StartsWith("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("starts-with"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueStartsWithSpecialCharacters()
        {
            Filter<string> clause = FilterFactory.StartsWith("DATA_SUBJECT", "@#~$%^&");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("starts-with"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~$%^&"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeLiteralSingleValue()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.StringLiteral("test"));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeLiteralMultipleValues()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.StringLiteral("test"),
                                                             LikeTokenFactory.StringLiteral("test2"),
                                                             LikeTokenFactory.StringLiteral("test3"));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeSingleCharacterWildcard()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.SingleCharacterWildcard());

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeMultipleCharacterWildcard()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.SingleCharacterWildcard(),
                                                             LikeTokenFactory.SingleCharacterWildcard(),
                                                             LikeTokenFactory.SingleCharacterWildcard());

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeZeroOrMoreWildcards()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.ZeroOrMoreCharactersWildcard());

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLikeMultipleTypes()
        {
            Filter<string> clause = FilterFactory.Like("DATA_SUBJECT",
                                                             LikeTokenFactory.StringLiteral("test"),
                                                             LikeTokenFactory.SingleCharacterWildcard(),
                                                             LikeTokenFactory.ZeroOrMoreCharactersWildcard());

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("like"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenLong()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", 1L, 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(1L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenMinAndMax()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", Int64.MinValue, Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenNullAndMax()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", null, Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenMaxAndNull()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", Int64.MaxValue, null);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenLongNulls()
        {
            long? startValue = null;
            long? endValue = null;

            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", startValue, endValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between-numbers"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenString()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", "test", "testing");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("testing"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenNumbersAsString()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", "1", "5");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("1"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("5"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenStringAndNull()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", "test", null);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenNullAndString()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", null, "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenStringNulls()
        {
            string? startValue = null;
            string? endValue = null;

            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", startValue, endValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between-strings"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNull());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueBetweenSpecialCharacters()
        {
            Filter<string> clause = FilterFactory.Between("DATABATCH_ID", "@#~", "$%^&");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("between"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("$%^&"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanLong()
        {
            Filter<string> clause = FilterFactory.LessThan("DATABATCH_ID", 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanMaxLong()
        {
            Filter<string> clause = FilterFactory.LessThan("DATABATCH_ID", Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanMinLong()
        {
            Filter<string> clause = FilterFactory.LessThan("DATABATCH_ID", Int64.MinValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanString()
        {
            Filter<string> clause = FilterFactory.LessThan("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanNumberAsString()
        {
            Filter<string> clause = FilterFactory.LessThan("DATA_SUBJECT", "two");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("two"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanEmptyString()
        {
            Filter<string> clause = FilterFactory.LessThan("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanSpecialCharacter()
        {
            Filter<string> clause = FilterFactory.LessThan("DATA_SUBJECT", "@#~");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEquals()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATABATCH_ID", 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsThanMaxLong()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATABATCH_ID", Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsThanMinLong()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATABATCH_ID", Int64.MinValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsString()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsNumberAsString()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATA_SUBJECT", "two");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("two"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsEmptyString()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueLessThanOrEqualsSpecialCharacters()
        {
            Filter<string> clause = FilterFactory.LessThanOrEquals("DATA_SUBJECT", "@#~");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("<="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanLong()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATABATCH_ID", 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanMaxLong()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATABATCH_ID", Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanMinLong()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATABATCH_ID", Int64.MinValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanString()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanNumberAsString()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATA_SUBJECT", "two");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("two"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanEmptyString()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanSpecialCharacters()
        {
            Filter<string> clause = FilterFactory.GreaterThan("DATA_SUBJECT", "@#~");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEquals()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATABATCH_ID", 5L);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(5L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsThanMaxLong()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATABATCH_ID", Int64.MaxValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MaxValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsThanMinLong()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATABATCH_ID", Int64.MinValue);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATABATCH_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(Int64.MinValue));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsString()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATA_SUBJECT", "test");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsNumberAsString()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATA_SUBJECT", "two");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("two"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsEmptyString()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATA_SUBJECT", "");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValueGreaterThanOrEqualsSpecialCharacter()
        {
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("DATA_SUBJECT", "@#~");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(">="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("@#~"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestExist()
        {
            Filter<string> clause = FilterFactory.Exists("DATA_SUBJECT");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("exists"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestExistEmptyField()
        {
            Filter<string> clause = FilterFactory.Exists("");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("exists"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestEmpty()
        {
            Filter<string> clause = FilterFactory.Empty("DATA_SUBJECT");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("empty"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestEmptyField()
        {
            Filter<string> clause = FilterFactory.Empty("");

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("empty"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(""));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();
        }

        [Fact]
        public void TestTransformValuesOrSingleValue()
        {
            Filter<string> clause = FilterFactory.Or(FilterFactory.Equals("REPOSITORY_ID", 1234L));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("REPOSITORY_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(1234L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValuesOrMultipleEqualsValues()
        {
            Filter<string> clause = FilterFactory.Or(FilterFactory.Equals("REPOSITORY_ID", 1234L),
                                                           FilterFactory.Equals("DATA_SUBJECT", "test"),
                                                           FilterFactory.Equals("DOC_COUNT", 123));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("REPOSITORY_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(1234L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DOC_COUNT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(123));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValuesNotString()
        {
            Filter<string> clause = FilterFactory.Not(FilterFactory.Equals("DATA_SUBJECT", "test"));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("DATA_SUBJECT"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValuesNotNumber()
        {
            Filter<string> clause = FilterFactory.Not(FilterFactory.Equals("REPOSITORY_ID", 1234L));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("REPOSITORY_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(1234L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearSingleValueLiterals()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.StringLiteral("right") };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (string s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("right"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearSpecialCharactersLiterals()
        {
            List<string> fieldSpecs = new() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.StringLiteral("right@$%&*") };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (string s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("right@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearLiteralAndSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearLiteralAndZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearLiteralWithSpecialCharactersAndZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearLiteralWithSpecialCharactersAndSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearBothSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.SingleCharacterWildcard() };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextNearBothZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.ZeroOrMoreCharactersWildcard() };
            LikeToken[] rhs = { LikeTokenFactory.ZeroOrMoreCharactersWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Near(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("near"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearSingleValueLiterals()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.StringLiteral("right") };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("right"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearSpecialCharactersLiterals()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.StringLiteral("right@$%&*") };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("right@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearLiteralAndSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearLiteralAndZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearLiteralWithSpecialCharactersAndZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left@$%&*") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left@$%&*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearLiteralWithSpecialCharactersAndSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.StringLiteral("left") };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("left"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearBothSingleCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.SingleCharacterWildcard() };
            LikeToken[] rhs = { LikeTokenFactory.SingleCharacterWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextDNearBothZeroOrMoreCharacterWildcard()
        {
            List<string> fieldSpecs = new List<string>() { "DATA_SUBJECT" };
            LikeToken[] lhs = { LikeTokenFactory.ZeroOrMoreCharactersWildcard() };
            LikeToken[] rhs = { LikeTokenFactory.ZeroOrMoreCharactersWildcard() };
            FullTextFilter clause = FullTextFilterFactory.Dnear(lhs, rhs, 0);
            Filter<string> filterClause = FilterFactory.FieldFullText(fieldSpecs, clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("field-full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            foreach (String s in fieldSpecs)
            {
                _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString(s));
            }
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("dnear"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(0));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsLiteralSingleValue()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsLiteralSingleValueWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test*&^%$@#"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test*&^%$@#"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsLiteralMultipleValues()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"),
                                                                         LikeTokenFactory.StringLiteral("test2"),
                                                                         LikeTokenFactory.StringLiteral("test3"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsLiteralMultipleValuesWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test*&^%$@#"),
                                                                         LikeTokenFactory.StringLiteral("test2*&^%$@#"),
                                                                         LikeTokenFactory.StringLiteral("test3*&^%$@#"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test*&^%$@#"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2*&^%$@#"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3*&^%$@#"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsSingleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsMultipleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard(),
                                                                         LikeTokenFactory.SingleCharacterWildcard(),
                                                                         LikeTokenFactory.SingleCharacterWildcard());
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsSingleCharacterWildcardAndLiteral()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard(),
                                                                         LikeTokenFactory.StringLiteral("test"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsSingleCharacterWildcardAndLiteralWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard(),
                                                                         LikeTokenFactory.StringLiteral("test%^&*#@"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test%^&*#@"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsZeroOrMoreCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsZeroOrMoreCharactersWildcardAndLiteral()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard(),
                                                                         LikeTokenFactory.StringLiteral("test"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsZeroOrMoreCharactersWildcardAndLiteralWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard(),
                                                                         LikeTokenFactory.StringLiteral("test%^&*#@"));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test%^&*#@"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsZeroOrMoreCharactersWildcardAndSingleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard(),
                                                                         LikeTokenFactory.SingleCharacterWildcard());
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());

            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOr()
        {
            FullTextFilter filter = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"));
            FullTextFilter clause = FullTextFilterFactory.Or(filter);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrWithSpecialCharacters()
        {
            FullTextFilter filter = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%"));
            FullTextFilter clause = FullTextFilterFactory.Or(filter);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrSingleCharacterWildcard()
        {
            FullTextFilter filter = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            FullTextFilter clause = FullTextFilterFactory.Or(filter);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrMultipleClauses()
        {
            FullTextFilter filter1 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"));
            FullTextFilter filter2 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2"));
            FullTextFilter filter3 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3"));
            FullTextFilter clause = FullTextFilterFactory.Or(filter1, filter2, filter3);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrMultipleClausesWithSpecialCharacters()
        {
            FullTextFilter filter1 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%"));
            FullTextFilter filter2 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2#@*&^%"));
            FullTextFilter filter3 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3#@*&^%"));
            FullTextFilter clause = FullTextFilterFactory.Or(filter1, filter2, filter3);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrMultipleClausesWithSingleCharacterWildcard()
        {
            FullTextFilter filter1 = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"));
            FullTextFilter filter2 = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            FullTextFilter clause = FullTextFilterFactory.Or(filter1, filter2);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrMultipleClausesWithSingleCharacterWildcardAndZeroOrMoreCharactersWildcard()
        {
            FullTextFilter filter1 = FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            FullTextFilter filter2 = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            FullTextFilter clause = FullTextFilterFactory.Or(filter1, filter2);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrList()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListWithSpecialCharacters()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListSingleCharacterWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListMultipleClauses()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>(){
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListMultipleClausesWithSpecialCharacters()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Or(new List<FullTextFilter>(){
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2#@*&^%")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3#@*&^%")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListMultipleClausesWithSingleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.Or(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsOrListMultipleClausesWithSingleCharacterWildcardAndZeroOrMoreCharactersWildcard()

        {
            FullTextFilter clause = FullTextFilterFactory.Or(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("or"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformValuesAndSingleValue()
        {
            Filter<string> clause = FilterFactory.And(FilterFactory.Equals("REPOSITORY_ID", 1234L));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("=="));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("REPOSITORY_ID"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteNumber(1234L));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory
                .And(FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndSingleCharacterWildcard()
        {
            FullTextFilter filter = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            FullTextFilter clause = FullTextFilterFactory.And(filter);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndMultipleClauses()
        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndMultipleClausesWithSpecialCharacters()
        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2#@*&^%")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3#@*&^%")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndMultipleClausesWithSingleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndMultipleClausesWithSingleCharacterWildcardAndZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndList()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListWithSpecialCharacters()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListSingleCharacterWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>() { FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListMultipleClauses()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>(){
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListMultipleClausesWithSpecialCharacters()
        {
            FullTextFilter clause
                = FullTextFilterFactory.And(new List<FullTextFilter>(){
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test2#@*&^%")),
                    FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test3#@*&^%")) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test2#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test3#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListMultipleClausesWithSingleCharacterWildcard()
        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test")),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsAndListMultipleClausesWithSingleCharacterWildcardAndZeroOrMoreCharactersWildcard()

        {
            FullTextFilter clause = FullTextFilterFactory.And(
                new List<FullTextFilter>(){FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()),
                              FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard()) });
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("and"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsNot()
        {
            Filter<string> filterClause
                = FilterFactory.FullText<string>(FullTextFilterFactory.Not(FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test"))));

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsNotWithSpecialCharacters()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Not(FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("test#@*&^%")));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("'"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("test#@*&^%"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsNotSingleCharacterWildcard()
        {
            FullTextFilter filter = FullTextFilterFactory.FullText(LikeTokenFactory.SingleCharacterWildcard());
            FullTextFilter clause = FullTextFilterFactory.Not(filter);
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("?"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformFullTextAsNotZeroOrMoreCharactersWildcard()
        {
            FullTextFilter clause
                = FullTextFilterFactory.Not(FullTextFilterFactory.FullText(LikeTokenFactory.ZeroOrMoreCharactersWildcard()));
            Filter<string> filterClause = FilterFactory.FullText<string>(clause);

            var sequence = new MockSequence();
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("not"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("full-text"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteStartArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteString("*"));
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.InSequence(sequence).Setup(x => x.WriteEndArray());
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(filterClause, _jsonBuilder.Object);
        }
    }
}
