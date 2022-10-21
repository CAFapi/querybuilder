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
    public abstract class MatcherQueryStringTestBase
    {
        protected readonly Dictionary<string, List<string>> _document;

        public MatcherQueryStringTestBase()
        {
            _document = GetDocument();
        }

        protected void TestEquals(string fieldName, string query)
        {
            TestEquals(fieldName, query, true);
        }

        protected void TestEquals(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.Equals(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestNotEquals(string fieldName, string query)
        {
            TestNotEquals(fieldName, query, true);
        }

        protected void TestNotEquals(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.NotEquals(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestIn(string fieldName, string[] query)
        {
            TestIn(fieldName, true, query.AsEnumerable());
        }

        protected void TestIn(string fieldName, bool expected, string[] query)
        {
            Filter<string> filter = FilterFactory.In(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestIn(string fieldName, IEnumerable<string> query)
        {
            TestIn(fieldName, true, query);
        }

        protected void TestIn(string fieldName, bool expected, IEnumerable<string> query)
        {
            Filter<string> filter = FilterFactory.In(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestContains(string fieldName, string query)
        {
            TestContains(fieldName, query, true);
        }

        protected void TestContains(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.Contains(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestStartsWith(string fieldName, string query)
        {
            TestStartsWith(fieldName, query, true);
        }

        protected void TestStartsWith(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.StartsWith(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestLessThan(string fieldName, string query)
        {
            TestLessThan(fieldName, query, true);
        }

        protected void TestLessThan(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.LessThan(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestLessThanOrEqual(string fieldName, string query)
        {
            TestLessThanOrEqual(fieldName, query, true);
        }

        protected void TestLessThanOrEqual(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.LessThanOrEquals(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestGreaterThan(string fieldName, string query)
        {
            TestGreaterThan(fieldName, query, true);
        }

        protected void TestGreaterThan(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.GreaterThan(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected void TestGreaterThanOrEqual(string fieldName, string query)
        {
            TestGreaterThanOrEqual(fieldName, query, true);
        }

        protected void TestGreaterThanOrEqual(string fieldName, string query, bool expected)
        {
            Filter<string> filter = FilterFactory.GreaterThanOrEquals(fieldName, query);
            Assert.True(DocMatches(filter) == expected, "Should" + (expected ? " " : " not ") + "have matched " + query + " in " + fieldName);
        }

        protected bool DocMatches(Filter<string> filter)
        {
            return filter.Map(MapKeyMatcherFieldSpec.Create).IsMatch(_document);
        }

        protected abstract Dictionary<string, List<string>> GetDocument();
    }
}
