/**
 * Copyright 2022-2023 Open Text.
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

namespace MicroFocus.CafApi.QueryBuilder
{
    public interface IFilterVisitor<in FieldSpec>
    {
        void VisitEquals(FieldSpec fieldSpec, bool value);

        void VisitEquals(FieldSpec fieldSpec, long value);

        void VisitEquals(FieldSpec fieldSpec, string value);

        void VisitNotEquals(FieldSpec fieldSpec, bool value);

        void VisitNotEquals(FieldSpec fieldSpec, long value);

        void VisitNotEquals(FieldSpec fieldSpec, string value);

        void VisitIn(FieldSpec fieldSpec, long[] values);

        void VisitIn(FieldSpec fieldSpec, IEnumerable<string> values);

        void VisitContains(FieldSpec fieldSpec, string value);

        void VisitStartsWith(FieldSpec fieldSpec, string value);

        void VisitLike(FieldSpec fieldSpec, LikeToken[] likeTokens);

        void VisitBetween(FieldSpec fieldSpec, long? startValue, long? endValue);

        void VisitBetween(FieldSpec fieldSpec, string startValue, string endValue);

        void VisitLessThan(FieldSpec fieldSpec, long value);

        void VisitLessThan(FieldSpec fieldSpec, string value);

        void VisitLessThanOrEquals(FieldSpec fieldSpec, long value);

        void VisitLessThanOrEquals(FieldSpec fieldSpec, string value);

        void VisitGreaterThan(FieldSpec fieldSpec, long value);

        void VisitGreaterThan(FieldSpec fieldSpec, string value);

        void VisitGreaterThanOrEquals(FieldSpec fieldSpec, long value);

        void VisitGreaterThanOrEquals(FieldSpec fieldSpec, string value);

        void VisitExists(FieldSpec fieldSpec);

        void VisitEmpty(FieldSpec fieldSpec);

        void VisitOr(IEnumerable<Filter<FieldSpec>> filters);

        void VisitAnd(IEnumerable<Filter<FieldSpec>> filters);

        void VisitNot(Filter<FieldSpec> filter);

        void VisitFullText(FullTextFilter fullTextFilter);

        void VisitFieldFullText(IEnumerable<FieldSpec> fieldSpecs, FullTextFilter fullTextFilter);
    }
}
