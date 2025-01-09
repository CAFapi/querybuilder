/**
 * Copyright 2022-2025 Open Text.
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
    public static class FullTextFilterFactory
    {
        public static FullTextFilter Near(LikeToken[] lhs, LikeToken[] rhs, int distance)
        {
            RequireNonNull(lhs);
            RequireNonNull(rhs);
            RequireWholeNumber(distance);
            return visitor =>
            {
                visitor.VisitNear(lhs, rhs, distance);
            };
        }

        public static FullTextFilter Dnear(LikeToken[] lhs, LikeToken[] rhs, int distance)
        {
            RequireNonNull(lhs);
            RequireNonNull(rhs);
            RequireWholeNumber(distance);
            return visitor =>
            {
                visitor.VisitDnear(lhs, rhs, distance);
            };
        }

        public static FullTextFilter FullText(params LikeToken[] searchTokens)
        {
            RequireNonNull(searchTokens);
            return visitor =>
            {
                visitor.VisitFullText(searchTokens);
            };
        }

        public static FullTextFilter Or(params FullTextFilter[] filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitOr(filters);
            };
        }

        public static FullTextFilter Or(IEnumerable<FullTextFilter> filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitOr(filters);
            };
        }

        public static FullTextFilter And(params FullTextFilter[] filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitAnd(filters);
            };
        }

        public static FullTextFilter And(IEnumerable<FullTextFilter> filters)
        {
            RequireNonNull(filters);
            return visitor =>
            {
                visitor.VisitAnd(filters);
            };
        }

        public static FullTextFilter Not(FullTextFilter filter)
        {
            return visitor =>
            {
                visitor.VisitNot(filter);
            };
        }

        private static void RequireNonNull(object input)
        {
            if (input == null)
            {
                throw new NullReferenceException("Input is null");
            }
        }

        private static void RequireWholeNumber(int input)
        {
            if (input < 0)
            {
                throw new ArgumentException("Input is not a whole number");
            }
        }
    }
}
