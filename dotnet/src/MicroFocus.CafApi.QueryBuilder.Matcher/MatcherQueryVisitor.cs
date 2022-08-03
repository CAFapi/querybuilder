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

namespace MicroFocus.CafApi.QueryBuilder.Matcher
{
    internal sealed class MatcherQueryVisitor<Document> : IFilterVisitor<IMatcherFieldSpec<Document>>
    {
        private readonly Document _document;
        private readonly IEnumerable<IMatcherFieldSpec<Document>> _allFullTextFieldSpecs;
        private bool _isMatch;

        public MatcherQueryVisitor(
            Document document,
            IEnumerable<IMatcherFieldSpec<Document>> allFullTextFieldSpecs
        )
        {
            _document = document;
            _allFullTextFieldSpecs = allFullTextFieldSpecs;
            _isMatch = false;
        }

        public bool GetResult()
        {
            return _isMatch;
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, bool value)
        {
            throw new NotImplementedException();
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, bool value)
        {
            throw new NotImplementedException();
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, long[] values)
        {
            throw new NotImplementedException();
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }

        public void VisitContains(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitStartsWith(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitLike(IMatcherFieldSpec<Document> fieldSpec, LikeToken[] likeTokens)
        {
            throw new NotImplementedException();
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, long? startValue, long? endValue)
        {
            throw new NotImplementedException();
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, string startValue, string endValue)
        {
            throw new NotImplementedException();
        }

        public void VisitLessThan(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitLessThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            throw new NotImplementedException();
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            throw new NotImplementedException();
        }

        public void VisitExists(IMatcherFieldSpec<Document> fieldSpec)
        {
            throw new NotImplementedException();
        }

        public void VisitEmpty(IMatcherFieldSpec<Document> fieldSpec)
        {
            throw new NotImplementedException();
        }

        public void VisitOr(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            throw new NotImplementedException();
        }

        public void VisitAnd(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            throw new NotImplementedException();
        }

        public void VisitNot(Filter<IMatcherFieldSpec<Document>> filter)
        {
            throw new NotImplementedException();
        }

        public void VisitFullText(FullTextFilter fullTextFilter)
        {
            throw new NotImplementedException();
        }

        public void VisitFieldFullText(
            IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
            FullTextFilter fullTextFilter
        )
        {
            throw new NotImplementedException();
        }
    }
}
