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
    public static class FilterWriter
    {
        public static void WriteToJsonArray(
            Filter<string> filter,
            IJsonBuilder jsonBuilder
        )
        {
            new FilterWriterVisitor(jsonBuilder).WriteFilter(filter);
        }
        class FilterWriterVisitor : IFilterVisitor<string>
        {
            private readonly IJsonBuilder _jsonBuilder;

            public FilterWriterVisitor(IJsonBuilder jsonBuilder)
            {
                _jsonBuilder = jsonBuilder;
            }
            public void WriteFilter(Filter<string> filter)
            {
                _jsonBuilder.WriteStartArray();
                filter.Invoke(this);
                _jsonBuilder.WriteEndArray();
            }
            public void VisitEquals(string fieldSpec, bool value)
            {
                _jsonBuilder.WriteString("==");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteBoolean(value);
            }
            public void VisitEquals(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString("==");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitEquals(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("==");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitNotEquals(string fieldSpec, bool value)
            {
                _jsonBuilder.WriteString("!=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteBoolean(value);
            }
            public void VisitNotEquals(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString("!=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitNotEquals(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("!=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitIn(string fieldSpec, long[] values)
            {
                _jsonBuilder.WriteString(values.Length == 0 ? "in-numbers" : "in");
                _jsonBuilder.WriteString(fieldSpec);
                foreach (long value in values)
                {
                    _jsonBuilder.WriteNumber(value);
                }
            }
            public void VisitIn(string fieldSpec, IEnumerable<string> values)
            {
                _jsonBuilder.WriteString(!values.Any() ? "in-strings" : "in");
                _jsonBuilder.WriteString(fieldSpec);
                foreach (string value in values)
                {
                    _jsonBuilder.WriteString(value);
                }
            }
            public void VisitContains(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("contains");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitStartsWith(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("starts-with");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitLike(string fieldSpec, LikeToken[] likeTokens)
            {
                _jsonBuilder.WriteString("like");
                _jsonBuilder.WriteString(fieldSpec);
                new LikeTokenWriterVisitor(_jsonBuilder).WriteLikeTokens(likeTokens);
            }
            public void VisitBetween(string fieldSpec, long? startValue, long? endValue)
            {
                _jsonBuilder.WriteString(startValue == null && endValue == null ? "between-numbers" : "between");
                _jsonBuilder.WriteString(fieldSpec);
                if (!startValue.HasValue)
                {
                    _jsonBuilder.WriteNull();
                }
                else
                {
                    _jsonBuilder.WriteNumber(startValue.Value);
                }
                if (!endValue.HasValue)
                {
                    _jsonBuilder.WriteNull();
                }
                else
                {
                    _jsonBuilder.WriteNumber(endValue.Value);
                }
            }
            public void VisitBetween(string fieldSpec, string startValue, string endValue)
            {
                _jsonBuilder.WriteString(startValue == null && endValue == null ? "between-strings" : "between");
                _jsonBuilder.WriteString(fieldSpec);
                if (startValue == null)
                {
                    _jsonBuilder.WriteNull();
                }
                else
                {
                    _jsonBuilder.WriteString(startValue);
                }
                if (endValue == null)
                {
                    _jsonBuilder.WriteNull();
                }
                else
                {
                    _jsonBuilder.WriteString(endValue);
                }
            }
            public void VisitLessThan(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString("<");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitLessThan(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("<");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitLessThanOrEquals(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString("<=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitLessThanOrEquals(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString("<=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitGreaterThan(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString(">");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitGreaterThan(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString(">");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitGreaterThanOrEquals(string fieldSpec, long value)
            {
                _jsonBuilder.WriteString(">=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteNumber(value);
            }
            public void VisitGreaterThanOrEquals(string fieldSpec, string value)
            {
                _jsonBuilder.WriteString(">=");
                _jsonBuilder.WriteString(fieldSpec);
                _jsonBuilder.WriteString(value);
            }
            public void VisitExists(string fieldSpec)
            {
                _jsonBuilder.WriteString("exists");
                _jsonBuilder.WriteString(fieldSpec);
            }
            public void VisitEmpty(string fieldSpec)
            {
                _jsonBuilder.WriteString("empty");
                _jsonBuilder.WriteString(fieldSpec);
            }

            public void VisitOr(IEnumerable<Filter<string>> filters)
            {
                _jsonBuilder.WriteString("or");
                foreach (Filter<string> filter in filters)
                {
                    WriteFilter(filter);
                }
            }

            public void VisitAnd(IEnumerable<Filter<string>> filters)
            {
                _jsonBuilder.WriteString("and");
                foreach (Filter<string> filter in filters)
                {
                    WriteFilter(filter);
                }
            }

            public void VisitNot(Filter<string> filter)
            {
                _jsonBuilder.WriteString("not");
                WriteFilter(filter);
            }

            public void VisitFullText(FullTextFilter fullTextFilter)
            {
                _jsonBuilder.WriteString("full-text");
                new FullTextFilterWriterVisitor(_jsonBuilder).WriteFullTextFilter(fullTextFilter);
            }

            public void VisitFieldFullText(IEnumerable<string> fieldSpecs, FullTextFilter fullTextFilter)
            {
                _jsonBuilder.WriteString("field-full-text");
                _jsonBuilder.WriteStartArray();
                foreach (string fieldSpec in fieldSpecs)
                {
                    _jsonBuilder.WriteString(fieldSpec);
                }
                _jsonBuilder.WriteEndArray();
                new FullTextFilterWriterVisitor(_jsonBuilder).WriteFullTextFilter(fullTextFilter);
            }
        }

        class LikeTokenWriterVisitor : ILikeTokenVisitor
        {
            private IJsonBuilder _jsonBuilder;

            public LikeTokenWriterVisitor(IJsonBuilder jsonBuilder)
            {
                _jsonBuilder = jsonBuilder;
            }
            public void WriteLikeTokenArray(LikeToken[] likeTokens)
            {
                _jsonBuilder.WriteStartArray();
                WriteLikeTokens(likeTokens);
                _jsonBuilder.WriteEndArray();
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
                _jsonBuilder.WriteStartArray();
                likeToken.Invoke(this);
                _jsonBuilder.WriteEndArray();
            }
            public void VisitStringLiteral(string literal)
            {
                _jsonBuilder.WriteString("'");
                _jsonBuilder.WriteString(literal);
            }
            public void VisitSingleCharacterWildcard()
            {
                _jsonBuilder.WriteString("?");
            }
            public void VisitZeroOrMoreCharactersWildcard()
            {
                _jsonBuilder.WriteString("*");
            }
        }
        class FullTextFilterWriterVisitor : IFullTextFilterVisitor
        {
            private readonly IJsonBuilder _jsonBuilder;

            public FullTextFilterWriterVisitor(IJsonBuilder jsonBuilder)
            {
                _jsonBuilder = jsonBuilder;
            }
            public void WriteFullTextFilter(FullTextFilter fullTextFilter)
            {
                _jsonBuilder.WriteStartArray();
                fullTextFilter.Invoke(this);
                _jsonBuilder.WriteEndArray();
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
                _jsonBuilder.WriteString(nearOrDNear);
                _jsonBuilder.WriteNumber(distance);

                LikeTokenWriterVisitor Visitor = new LikeTokenWriterVisitor(_jsonBuilder);
                Visitor.WriteLikeTokenArray(lhs);
                Visitor.WriteLikeTokenArray(rhs);
            }
            public void VisitFullText(LikeToken[] searchTokens)
            {
                _jsonBuilder.WriteString("full-text");
                new LikeTokenWriterVisitor(_jsonBuilder).WriteLikeTokens(searchTokens);
            }
            public void VisitOr(IEnumerable<FullTextFilter> fullTextFilters)
            {
                _jsonBuilder.WriteString("or");
                foreach (FullTextFilter fullTextFilter in fullTextFilters)
                {
                    WriteFullTextFilter(fullTextFilter);
                }
            }
            public void VisitAnd(IEnumerable<FullTextFilter> fullTextFilters)
            {
                _jsonBuilder.WriteString("and");
                foreach (FullTextFilter fullTextFilter in fullTextFilters)
                {
                    WriteFullTextFilter(fullTextFilter);
                }
            }
            public void VisitNot(FullTextFilter fullTextFilter)
            {
                _jsonBuilder.WriteString("not");
                WriteFullTextFilter(fullTextFilter);
            }
        }
    }
}
