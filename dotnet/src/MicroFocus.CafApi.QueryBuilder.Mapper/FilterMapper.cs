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
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicroFocus.CafApi.QueryBuilder.Mapper
{
    public static class FilterMapper<T, R>
    {
        public static Filter<R> Map(Filter<T> sourceFilter, Func<T, R> mapper)
        {
            FilterMapperVisitor<T, R> visitor = new FilterMapperVisitor<T, R>(mapper);
            sourceFilter.Invoke(visitor);

            return visitor.GetResult();
        }

        class FilterMapperVisitor<VT, VR> : IFilterVisitor<T>
        {
            private readonly Func<T, R> _mapper;
            private Filter<R> _result;

            public FilterMapperVisitor(Func<T, R> mapper)
            {
                _mapper = mapper;
                _result = null;
            }

            public Filter<R> GetResult()
            {
                return _result;
            }


            public void VisitEquals(T fieldSpec, bool value)
            {
                _result = visitor =>
                {
                    visitor.VisitEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitEquals(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitEquals(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitNotEquals(T fieldSpec, bool value)
            {
                _result = visitor =>
                {
                    visitor.VisitNotEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitNotEquals(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitNotEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitNotEquals(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitNotEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitIn(T fieldSpec, long[] values)
            {
                _result = visitor =>
                {
                    visitor.VisitIn(_mapper.Invoke(fieldSpec), values);
                };
            }


            public void VisitIn(T fieldSpec, IEnumerable<string> values)
            {
                _result = visitor =>
                {
                    visitor.VisitIn(_mapper.Invoke(fieldSpec), values);
                };
            }


            public void VisitContains(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitContains(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitStartsWith(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitStartsWith(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitLike(T fieldSpec, LikeToken[] likeTokens)
            {
                _result = visitor =>
                {
                    visitor.VisitLike(_mapper.Invoke(fieldSpec), likeTokens);
                };
            }


            public void VisitBetween(T fieldSpec, long? startValue, long? endValue)
            {
                _result = visitor =>
                {
                    visitor.VisitBetween(_mapper.Invoke(fieldSpec), startValue, endValue);
                };
            }


            public void VisitBetween(T fieldSpec, string startValue, string endValue)
            {
                _result = visitor =>
                {
                    visitor.VisitBetween(_mapper.Invoke(fieldSpec), startValue, endValue);
                };
            }


            public void VisitLessThan(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitLessThan(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitLessThan(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitLessThan(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitLessThanOrEquals(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitLessThanOrEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitLessThanOrEquals(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitLessThanOrEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitGreaterThan(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitGreaterThan(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitGreaterThan(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitGreaterThan(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitGreaterThanOrEquals(T fieldSpec, long value)
            {
                _result = visitor =>
                {
                    visitor.VisitGreaterThanOrEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitGreaterThanOrEquals(T fieldSpec, string value)
            {
                _result = visitor =>
                {
                    visitor.VisitGreaterThanOrEquals(_mapper.Invoke(fieldSpec), value);
                };
            }


            public void VisitExists(T fieldSpec)
            {
                _result = visitor =>
                {
                    visitor.VisitExists(_mapper.Invoke(fieldSpec));
                };
            }


            public void VisitEmpty(T fieldSpec)
            {
                _result = visitor =>
                {
                    visitor.VisitEmpty(_mapper.Invoke(fieldSpec));
                };
            }


            public void VisitOr(IEnumerable<Filter<T>> filters)
            {
                _result = visitor =>
                {
                    visitor.VisitOr(filters
                        .Select(filter => Map(filter, _mapper))
                        .ToList());
                };
            }


            public void VisitAnd(IEnumerable<Filter<T>> filters)
            {
                _result = visitor =>
                {
                    visitor.VisitAnd(filters
                        .Select(filter => Map(filter, _mapper))
                        .ToList());
                };
            }


            public void VisitNot(Filter<T> filter)
            {
                _result = visitor =>
                {
                    visitor.VisitNot(Map(filter, _mapper));
                };
            }


            public void VisitFullText(FullTextFilter fullTextFilter)
            {
                _result = visitor =>
                {
                    visitor.VisitFullText(fullTextFilter);
                };
            }


            public void VisitFieldFullText(IEnumerable<T> fieldSpecs, FullTextFilter filter)
            {
                _result = visitor =>
                {
                    visitor.VisitFieldFullText(
                        fieldSpecs.Select(_mapper).ToList(),
                        filter);
                };
            }
        }
    }
}
