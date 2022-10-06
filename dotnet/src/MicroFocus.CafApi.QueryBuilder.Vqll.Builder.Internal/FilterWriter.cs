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
using System.Collections.Generic;
using System.Linq;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Builders.Internal
{
    public class FilterWriter
    {
        private IJsonBuilder _jsonBuilder;

        private FilterWriter(IJsonBuilder jsonBuilder)
        {
            this._jsonBuilder = jsonBuilder;
        }

        public static void WriteToJsonArray(
            Filter<string> filter,
            IJsonBuilder jsonBuilder

        )
        {
            FilterWriter filterWriter = new FilterWriter(jsonBuilder);
            // TODO: Could the jsonBuilder be an instance variable of FilterWriterVisitor?
            new FilterWriterVisitor(filterWriter).WriteFilter(filter);
        }
        class FilterWriterVisitor : IFilterVisitor<string>
        {
            private FilterWriter _filterWriter;

            public FilterWriterVisitor(FilterWriter filterWriter)
            {
                this._filterWriter = filterWriter;
            }
            public void WriteFilter(Filter<string> filter)
            {
                _filterWriter._jsonBuilder.WriteStartArray();
                filter.Invoke(this);
                _filterWriter._jsonBuilder.WriteEndArray();
            }
            public void VisitEquals(string fieldSpec, bool value)
            {
                _filterWriter._jsonBuilder.WriteString("==");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteBoolean(value);
            }
            public void VisitEquals(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString("==");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitEquals(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("==");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitNotEquals(string fieldSpec, bool value)
            {
                _filterWriter._jsonBuilder.WriteString("!=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteBoolean(value);
            }
            public void VisitNotEquals(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString("!=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitNotEquals(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("!=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitIn(string fieldSpec, long[] values)
            {
                _filterWriter._jsonBuilder.WriteString(values.Length == 0 ? "in-numbers" : "in");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                foreach (long value in values)
                {
                    _filterWriter._jsonBuilder.WriteNumber(value);
                }
            }
            public void VisitIn(string fieldSpec, IEnumerable<string> values)
            {
                _filterWriter._jsonBuilder.WriteString(values.Count() == 0 ? "in-strings" : "in");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                foreach (string value in values)
                {
                    _filterWriter._jsonBuilder.WriteString(value);
                }
            }
            public void VisitContains(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("contains");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitStartsWith(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("starts-with");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitLike(string fieldSpec, LikeToken[] likeTokens)
            {
                _filterWriter._jsonBuilder.WriteString("like");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                new LikeTokenWriterVisitor(_filterWriter).WriteLikeTokens(likeTokens);
            }
            public void VisitBetween(string fieldSpec, long? startValue, long? endValue)
            {
                _filterWriter._jsonBuilder.WriteString(startValue == null && endValue == null ? "between-numbers" : "between");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                if (startValue == null)
                {
                    _filterWriter._jsonBuilder.WriteNull();
                }
                else
                {
                    _filterWriter._jsonBuilder.WriteNumber(startValue);
                }
                if (endValue == null)
                {
                    _filterWriter._jsonBuilder.WriteNull();
                }
                else
                {
                    _filterWriter._jsonBuilder.WriteNumber(endValue);
                }
            }
            public void VisitBetween(string fieldSpec, string startValue, string endValue)
            {
                _filterWriter._jsonBuilder.WriteString(startValue == null && endValue == null ? "between-strings" : "between");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                if (startValue == null)
                {
                    _filterWriter._jsonBuilder.WriteNull();
                }
                else
                {
                    _filterWriter._jsonBuilder.WriteString(startValue);
                }
                if (endValue == null)
                {
                    _filterWriter._jsonBuilder.WriteNull();
                }
                else
                {
                    _filterWriter._jsonBuilder.WriteString(endValue);
                }
            }
            public void VisitLessThan(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString("<");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitLessThan(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("<");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitLessThanOrEquals(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString("<=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitLessThanOrEquals(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString("<=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitGreaterThan(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString(">");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitGreaterThan(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString(">");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitGreaterThanOrEquals(string fieldSpec, long value)
            {
                _filterWriter._jsonBuilder.WriteString(">=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteNumber(value);
            }
            public void VisitGreaterThanOrEquals(string fieldSpec, string value)
            {
                _filterWriter._jsonBuilder.WriteString(">=");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
                _filterWriter._jsonBuilder.WriteString(value);
            }
            public void VisitExists(string fieldSpec)
            {
                _filterWriter._jsonBuilder.WriteString("exists");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
            }
            public void VisitEmpty(string fieldSpec)
            {
                _filterWriter._jsonBuilder.WriteString("empty");
                _filterWriter._jsonBuilder.WriteString(fieldSpec);
            }

            public void VisitOr(IEnumerable<Filter<string>> filters)
            {
                _filterWriter._jsonBuilder.WriteString("or");
                foreach (Filter<string> filter in filters)
                {
                    WriteFilter(filter);
                }
            }

            public void VisitAnd(IEnumerable<Filter<string>> filters)
            {
                _filterWriter._jsonBuilder.WriteString("and");
                foreach (Filter<string> filter in filters)
                {
                    WriteFilter(filter);
                }
            }

            public void VisitNot(Filter<string> filter)
            {
                _filterWriter._jsonBuilder.WriteString("not");
                WriteFilter(filter);
            }

            public void VisitFullText(FullTextFilter fullTextFilter)
            {
                _filterWriter._jsonBuilder.WriteString("full-text");
                new FullTextFilterWriterVisitor(_filterWriter).WriteFullTextFilter(fullTextFilter);
            }

            public void VisitFieldFullText(IEnumerable<string> fieldSpecs, FullTextFilter fullTextFilter)
            {
                _filterWriter._jsonBuilder.WriteString("field-full-text");
                _filterWriter._jsonBuilder.WriteStartArray();
                foreach (string fieldSpec in fieldSpecs)
                {
                    _filterWriter._jsonBuilder.WriteString(fieldSpec);
                }
                _filterWriter._jsonBuilder.WriteEndArray();
                new FullTextFilterWriterVisitor(_filterWriter).WriteFullTextFilter(fullTextFilter);
            }
        }

        class LikeTokenWriterVisitor : ILikeTokenVisitor
        {
            private FilterWriter _filterWriter;

            public LikeTokenWriterVisitor(FilterWriter filterWriter)
            {
                this._filterWriter = filterWriter;
            }
            public void WriteLikeTokenArray(LikeToken[] likeTokens)
            {
                _filterWriter._jsonBuilder.WriteStartArray();
                WriteLikeTokens(likeTokens);
                _filterWriter._jsonBuilder.WriteEndArray();
            }
            public void WriteLikeTokens(LikeToken[] likeTokens)
            {
                foreach (LikeToken likeToken in likeTokens)
                {
                    WriteLikeToken(likeToken);
                }
            }
            private void WriteLikeToken(LikeToken likeToken)
            {
                _filterWriter._jsonBuilder.WriteStartArray();
                likeToken.Invoke(this);
                _filterWriter._jsonBuilder.WriteEndArray();
            }
            public void VisitStringLiteral(string literal)
            {
                _filterWriter._jsonBuilder.WriteString("'");
                _filterWriter._jsonBuilder.WriteString(literal);
            }
            public void VisitSingleCharacterWildcard()
            {
                _filterWriter._jsonBuilder.WriteString("?");
            }
            public void VisitZeroOrMoreCharactersWildcard()
            {
                _filterWriter._jsonBuilder.WriteString("*");
            }
        }
        class FullTextFilterWriterVisitor : IFullTextFilterVisitor
        {
            private FilterWriter filterWriter;

            public FullTextFilterWriterVisitor(FilterWriter filterWriter)
            {
                this.filterWriter = filterWriter;
            }
            public void WriteFullTextFilter(FullTextFilter fullTextFilter)
            {
                filterWriter._jsonBuilder.WriteStartArray();
                fullTextFilter.Invoke(this);
                filterWriter._jsonBuilder.WriteEndArray();
            }
            public void VisitNear(LikeToken[] lhs, LikeToken[] rhs, int distance)
            {
                WriteNearOrDNear("near", distance, lhs, rhs);
            }
            public void VisitDnear(LikeToken[] lhs, LikeToken[] rhs, int distance)
            {
                WriteNearOrDNear("dnear", distance, lhs, rhs);
            }
            private void WriteNearOrDNear(string nearOrDNear, int distance, LikeToken[] lhs, LikeToken[] rhs)
            {
                filterWriter._jsonBuilder.WriteString(nearOrDNear);
                filterWriter._jsonBuilder.WriteNumber(distance);

                LikeTokenWriterVisitor Visitor = new LikeTokenWriterVisitor(filterWriter);
                Visitor.WriteLikeTokenArray(lhs);
                Visitor.WriteLikeTokenArray(rhs);
            }
            public void VisitFullText(LikeToken[] searchTokens)
            {
                filterWriter._jsonBuilder.WriteString("full-text");
                new LikeTokenWriterVisitor(filterWriter).WriteLikeTokens(searchTokens);
            }
            public void VisitOr(IEnumerable<FullTextFilter> fullTextFilters)
            {
                filterWriter._jsonBuilder.WriteString("or");
                foreach (FullTextFilter fullTextFilter in fullTextFilters)
                {
                    WriteFullTextFilter(fullTextFilter);
                }
            }
            public void VisitAnd(IEnumerable<FullTextFilter> fullTextFilters)
            {
                filterWriter._jsonBuilder.WriteString("and");
                foreach (FullTextFilter fullTextFilter in fullTextFilters)
                {
                    WriteFullTextFilter(fullTextFilter);
                }
            }
            public void VisitNot(FullTextFilter fullTextFilter)
            {
                filterWriter._jsonBuilder.WriteString("not");
                WriteFullTextFilter(fullTextFilter);
            }
        }
    }
}
