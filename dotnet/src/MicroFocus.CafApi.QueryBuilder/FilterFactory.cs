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
using System;
using System.Collections.Generic;

namespace MicroFocus.CafApi.QueryBuilder
{
    public static class FilterFactory
    {
        public static Filter<FieldSpec> Equals<FieldSpec>(FieldSpec fieldSpec, bool value)
        {
            return visitor =>
            {
                visitor.VisitEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> Equals<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> Equals<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> NotEquals<FieldSpec>(FieldSpec fieldSpec, bool value)
        {
            return visitor =>
            {
                visitor.VisitNotEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> NotEquals<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitNotEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> NotEquals<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitNotEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> In<FieldSpec>(FieldSpec fieldSpec, params long[] values)
        {
            RequireNonNull(values);
            return visitor =>
            {
                visitor.VisitIn(fieldSpec, values);
            };
        }

        public static Filter<FieldSpec> In<FieldSpec>(FieldSpec fieldSpec, params string[] values)
        {
            RequireNonNull(values);
            return visitor =>
            {
                visitor.VisitIn(fieldSpec, values);
            };
        }

        public static Filter<FieldSpec> In<FieldSpec>(FieldSpec fieldSpec, IEnumerable<string> values)
        {
            RequireNonNull(values);
            return visitor =>
            {
                visitor.VisitIn(fieldSpec, values);
            };
        }

        public static Filter<FieldSpec> Contains<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitContains(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> StartsWith<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitStartsWith(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> Like<FieldSpec>(FieldSpec fieldSpec, params LikeToken[] likeTokens)
        {
            return visitor =>
            {
                visitor.VisitLike(fieldSpec, likeTokens);
            };
        }

        public static Filter<FieldSpec> Between<FieldSpec>(FieldSpec fieldSpec, long? startValue, long? endValue)
        {
            return visitor =>
            {
                visitor.VisitBetween(fieldSpec, startValue, endValue);
            };
        }

        public static Filter<FieldSpec> Between<FieldSpec>(FieldSpec fieldSpec, string startValue, string endValue)
        {
            return visitor =>
            {
                visitor.VisitBetween(fieldSpec, startValue, endValue);
            };
        }

        public static Filter<FieldSpec> LessThan<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitLessThan(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> LessThan<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitLessThan(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> LessThanOrEquals<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitLessThanOrEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> LessThanOrEquals<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitLessThanOrEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> GreaterThan<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitGreaterThan(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> GreaterThan<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitGreaterThan(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> GreaterThanOrEquals<FieldSpec>(FieldSpec fieldSpec, long value)
        {
            return visitor =>
            {
                visitor.VisitGreaterThanOrEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> GreaterThanOrEquals<FieldSpec>(FieldSpec fieldSpec, string value)
        {
            RequireNonNull(value);
            return visitor =>
            {
                visitor.VisitGreaterThanOrEquals(fieldSpec, value);
            };
        }

        public static Filter<FieldSpec> Exists<FieldSpec>(FieldSpec fieldSpec)
        {
            return visitor =>
            {
                visitor.VisitExists(fieldSpec);
            };
        }

        public static Filter<FieldSpec> Empty<FieldSpec>(FieldSpec fieldSpec)
        {
            return visitor =>
            {
                visitor.VisitEmpty(fieldSpec);
            };
        }

        public static Filter<FieldSpec> Or<FieldSpec>(params Filter<FieldSpec>[] filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitOr(filters);
            };
        }

        public static Filter<FieldSpec> Or<FieldSpec>(IEnumerable<Filter<FieldSpec>> filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitOr(filters);
            };
        }

        public static Filter<FieldSpec> And<FieldSpec>(params Filter<FieldSpec>[] filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitAnd(filters);
            };
        }

        public static Filter<FieldSpec> And<FieldSpec>(IEnumerable<Filter<FieldSpec>> filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitAnd(filters);
            };
        }

        public static Filter<FieldSpec> Not<FieldSpec>(Filter<FieldSpec> filter)
        {
            return visitor =>
            {
                visitor.VisitNot(filter);
            };
        }

        public static Filter<FieldSpec> FullText<FieldSpec>(FullTextFilter fullTextFilter)
        {
            RequireNonNull(fullTextFilter);
            return visitor =>
            {
                visitor.VisitFullText(fullTextFilter);
            };
        }

        public static Filter<FieldSpec> FieldFullText<FieldSpec>(
            IEnumerable<FieldSpec> fieldSpecs,
            FullTextFilter fullTextFilter
        )
        {
            return visitor =>
            {
                visitor.VisitFieldFullText(fieldSpecs, fullTextFilter);
            };
        }

        private static void RequireNonNull(object input)
        {
            if (input == null)
            {
                throw new NullReferenceException("Input is null");
            }
        }
    }
}
