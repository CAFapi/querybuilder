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
using System.Globalization;
using System.Linq;
using System.Diagnostics;

namespace MicroFocus.CafApi.QueryBuilder.Matcher
{
    internal sealed class MatcherQueryVisitor<Document> : IFilterVisitor<IMatcherFieldSpec<Document>>
    {
        private readonly Document _document;
        private readonly IEnumerable<IMatcherFieldSpec<Document>> _allFullTextFieldSpecs;
        private bool? _isMatch;

        public MatcherQueryVisitor(
            Document document,
            IEnumerable<IMatcherFieldSpec<Document>> allFullTextFieldSpecs
        )
        {
            _document = document;
            _allFullTextFieldSpecs = allFullTextFieldSpecs;
            _isMatch = false;
        }

        public bool? GetResult()
        {
            return _isMatch;
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, bool value)
        {
            var values = GetStringValues(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => bool.Parse(v) == value);
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => v == value);
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var stream = GetFieldValuesAsStream(fieldSpec);
            if (stream == null)
            {
                _isMatch = null;
            }
            else
            {
                var comparisonType = fieldSpec.IsCaseInsensitive
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                _isMatch = stream
                    .Any(v => string.Equals(v, value, comparisonType));
            }
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, bool value)
        {
            var values = GetStringValues(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)!values.Any(v => bool.Parse(v) == value);
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)!values.Any(v => v == value);
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var stream = GetFieldValuesAsStream(fieldSpec);
            if (stream == null)
            {
                _isMatch = null;
            }
            else
            {
                var comparisonType = fieldSpec.IsCaseInsensitive
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                _isMatch = !stream
                    .Any(v => string.Equals(v, value, comparisonType));
            }
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, long[] values)
        {
            var longValues = GetLongStream(fieldSpec);
            if (longValues == null)
            {
                _isMatch = null;
            }
            else
            {
                var inList = values.ToList();
                var documentValues = longValues.ToList();

                _isMatch = documentValues.Intersect(inList).Any();
            }
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, IEnumerable<string> values)
        {
            var stream = GetFieldValuesAsStream(fieldSpec);
            _isMatch = (stream == null)
                ? null
                : (bool?)stream.ToList()
                    .Intersect(fieldSpec.IsCaseInsensitive ? ToUppercase(values) : values)
                    .Any();
        }

        public void VisitContains(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.Contains(valueToCompare));
            }
        }

        public void VisitStartsWith(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.StartsWith(valueToCompare));
            }
        }

        public void VisitLike(IMatcherFieldSpec<Document> fieldSpec, LikeToken[] likeTokens)
        {
            throw new NotImplementedException("Not implemented yet.");
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, long? startValue, long? endValue)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => (startValue is null || v >= startValue) && (endValue is null || v <= endValue));
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, string startValue, string endValue)
        {
            if (startValue != null && endValue != null)
            {
                string valueStart = fieldSpec.IsCaseInsensitive ? startValue.ToUpperInvariant() : startValue;
                string valueEnd = fieldSpec.IsCaseInsensitive ? endValue.ToUpperInvariant() : endValue;
                var values = GetStringValues(fieldSpec);
                _isMatch = (values == null)
                    ? null
                    : (bool?)values.Any(v => v.CompareTo(valueStart) >= 0 && v.CompareTo(valueEnd) <= 0);
            }
            else if (startValue != null)
            {
                VisitGreaterThanOrEquals(fieldSpec, startValue);
            }
            else if (endValue != null)
            {
                VisitLessThanOrEquals(fieldSpec, endValue);
            }
            else
            {
                VisitExists(fieldSpec);
            }
        }

        public void VisitLessThan(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => v < value);
        }

        public void VisitLessThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.CompareTo(valueToCompare) < 0);
            }
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => v <= value);
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.CompareTo(valueToCompare) < 1);
            }
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => v > value);
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.CompareTo(valueToCompare) > 0);
            }
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            var values = GetLongStream(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any(v => v >= value);
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                _isMatch = null;
            }
            else
            {
                string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpperInvariant() : value;

                _isMatch = values.Any(v => v.CompareTo(valueToCompare) >= 0);
            }
        }

        public void VisitExists(IMatcherFieldSpec<Document> fieldSpec)
        {
            var values = GetFieldValues(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)values.Any();
        }

        public void VisitEmpty(IMatcherFieldSpec<Document> fieldSpec)
        {
            var values = GetStringValues(fieldSpec);
            _isMatch = (values == null)
                ? null
                : (bool?)!values.Any(v => v.Length > 0);
        }

        public void VisitOr(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            /*
                true or true === true
                true or false === true
                true or unknown === true
                false or true === true
                false or false === false
                false or unknown === unknown
                unknown or true === true
                unknown or false === unknown
                unknown or unknown === unknown
            */
            using (var filterEnumerator = filters.GetEnumerator())
            {
                while (filterEnumerator.MoveNext())
                {
                    filterEnumerator.Current.Invoke(this);
                    if (_isMatch == true)
                    {
                        return;
                    }

                    if (_isMatch == null)
                    {
                        while (filterEnumerator.MoveNext())
                        {
                            _isMatch = false;
                            filterEnumerator.Current.Invoke(this);
                            if (_isMatch == true)
                            {
                                return;
                            }
                        }

                        _isMatch = null;
                        return;
                    }
                }
            }
        }

        public void VisitAnd(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            /*
                true and true === true
                true and false === false
                true and unknown === unknown
                false and true === false
                false and false === false
                false and unknown === false
                unknown and true === unknown
                unknown and false === false
                unknown and unknown === unknown
            */
            bool? result = true;

            foreach (Filter<IMatcherFieldSpec<Document>> filter in filters)
            {
                bool? isMatch = filter.IsMatch(_document, _allFullTextFieldSpecs);

                if (isMatch == false)
                {
                    result = false;
                    break;
                }

                if (isMatch == null)
                {
                    result = null;
                }
            }

            _isMatch = result;
        }

        public void VisitNot(Filter<IMatcherFieldSpec<Document>> filter)
        {
            /*
                true === false
                false === true
                unknown === unknown
            */
            filter.Invoke(this);
            if (_isMatch != null)
            {
                _isMatch = !_isMatch;
            }
        }

        public void VisitFullText(FullTextFilter fullTextFilter)
        {
            VisitFieldFullTextImpl(_allFullTextFieldSpecs, fullTextFilter);
        }

        public void VisitFieldFullText(
            IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
            FullTextFilter fullTextFilter
        )
        {
            VisitFieldFullTextImpl(fieldSpecs, fullTextFilter);
        }

        private IEnumerable<IMatcherFieldValue> GetFieldValues(IMatcherFieldSpec<Document> fieldSpec)
        {
            return fieldSpec.GetFieldValues(_document);
        }

        private IEnumerable<long> GetLongStream(IMatcherFieldSpec<Document> fieldSpec)
        {
            var values = GetStringValues(fieldSpec);
            if (values == null)
            {
                return null;
            }
            return from stringValue in values
                   select new
                   {
                       Parsed = long.TryParse(stringValue, NumberStyles.AllowLeadingSign, null, out long result),
                       Result = result
                   }
                   into parsedValue
                   where parsedValue.Parsed
                   select parsedValue.Result;
        }

        private IEnumerable<string> GetStringValues(IMatcherFieldSpec<Document> fieldSpec)
        {
            var values = GetFieldValues(fieldSpec);
            if (values == null)
            {
                return null;
            }
            var stream = from fieldValue in values
                         where !fieldValue.IsReference
                         select fieldValue.StringValue;
            return fieldSpec.IsCaseInsensitive
                ? stream.Select(v => v.ToUpperInvariant())
                : stream;
        }

        private IEnumerable<string> GetFieldValuesAsStream(IMatcherFieldSpec<Document> fieldSpec)
        {
            if (fieldSpec.IsTokenizedPath)
            {
                var values = GetStringValues(fieldSpec);
                if (values == null)
                {
                    return null;
                }
                return values.SelectMany(v => TokenizePath(v));
            }
            else
            {
                return GetStringValues(fieldSpec);
            }
        }

        private static List<string> ToUppercase(IEnumerable<string> values)
        {
            return values.Select(v => v.ToUpperInvariant()).ToList();
        }

        private static IEnumerable<string> TokenizePath(string path)
        {
            Debug.WriteLine("Tokenizing path: " + path);
            var tokens = new List<string>();
            FilePath f = new FilePath(path);
            do
            {
                Debug.WriteLine("path :name: " + f.GetPath());
                tokens.Add(f.GetPath().Replace('\\', '/'));
                f = f.GetParentFile();
            } while (f != null && f.GetParentFile() != null);
            Debug.WriteLine("path Tokens: " + string.Join(", ", tokens));
            return tokens;
        }

        private void VisitFieldFullTextImpl(
            IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
            FullTextFilter fullTextFilter
        )
        {
            if (fieldSpecs == null || HasFields(fieldSpecs) != false)
            {
                // Document has or may have fullText fields so matcher does not currently implement full text operation
                throw new NotSupportedException("Not supported.");
            }
            else
            {
                // Document does not have fullTextField
                fullTextFilter.Invoke(new NoFullTextFieldsFullTextFilterVisitorImpl(fieldSpecs, this));
            }
        }

        private bool? HasFields(IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs)
        {
            foreach (var fieldSpec in fieldSpecs)
            {
                var fldValues = GetFieldValues(fieldSpec);
                if (fldValues == null)
                {
                    return null;
                }

                if (fldValues.Any())
                {
                    return true;
                }
            }

            return false;
        }

        abstract class FullTextFilterVisitorImpl : IFullTextFilterVisitor
        {
            protected IEnumerable<IMatcherFieldSpec<Document>> _fieldSpecs;
            protected MatcherQueryVisitor<Document> _mqVisitor;

            public FullTextFilterVisitorImpl(
                IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
                MatcherQueryVisitor<Document> mqVisitor)
            {
                _fieldSpecs = fieldSpecs;
                _mqVisitor = mqVisitor;
            }

            public void VisitOr(IEnumerable<FullTextFilter> fullTextFilters)
            {
                using (var filterEnumerator = fullTextFilters.GetEnumerator())
                {
                    while (filterEnumerator.MoveNext())
                    {
                        filterEnumerator.Current.Invoke(this);
                        if (_mqVisitor._isMatch == true)
                        {
                            return;
                        }

                        if (_mqVisitor._isMatch == null)
                        {
                            while (filterEnumerator.MoveNext())
                            {
                                _mqVisitor._isMatch = false;
                                filterEnumerator.Current.Invoke(this);
                                if (_mqVisitor._isMatch == true)
                                {
                                    return;
                                }
                            }

                            _mqVisitor._isMatch = null;
                            return;
                        }
                    }
                }
            }

            public void VisitAnd(IEnumerable<FullTextFilter> fullTextFilters)
            {
                bool? result = true;

                foreach (var fullTextFilter in fullTextFilters)
                {
                    MatcherQueryVisitor<Document> visitor =
                        new MatcherQueryVisitor<Document>(_mqVisitor._document, _mqVisitor._allFullTextFieldSpecs);
                    _mqVisitor.VisitFieldFullTextImpl(_fieldSpecs, fullTextFilter);

                    bool? isMatch = visitor.GetResult();

                    if (isMatch == false)
                    {
                        result = false;
                        break;
                    }

                    if (isMatch == null)
                    {
                        result = null;
                    }
                }

                _mqVisitor._isMatch = result;
            }

            public void VisitNot(FullTextFilter fullTextFilter)
            {
                fullTextFilter.Invoke(this);
                if (_mqVisitor._isMatch != null)
                {
                    _mqVisitor._isMatch = !_mqVisitor._isMatch;
                }
            }

            public abstract void VisitNear(LikeToken[] lhs, LikeToken[] rhs, int distance);
            public abstract void VisitDnear(LikeToken[] lhs, LikeToken[] rhs, int distance);
            public abstract void VisitFullText(LikeToken[] searchTokens);
        }

        /**
         * This visitor must only be used if the document DOES NOT HAVE ANY full text fields.
         */
        class NoFullTextFieldsFullTextFilterVisitorImpl : FullTextFilterVisitorImpl
        {
            public NoFullTextFieldsFullTextFilterVisitorImpl(
                IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
                MatcherQueryVisitor<Document> mqVisitor
            ) : base(fieldSpecs, mqVisitor)
            {
            }

            public override void VisitNear(LikeToken[] lhs, LikeToken[] rhs, int distance)
            {
                _mqVisitor._isMatch = false;
            }

            public override void VisitDnear(LikeToken[] lhs, LikeToken[] rhs, int distance)
            {
                _mqVisitor._isMatch = false;
            }

            public override void VisitFullText(LikeToken[] searchTokens)
            {
                _mqVisitor._isMatch = false;
            }
        }
    }
}
