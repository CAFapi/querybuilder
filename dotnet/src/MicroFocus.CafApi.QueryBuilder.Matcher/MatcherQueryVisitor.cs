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
//using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;

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
            _isMatch = GetStringValues(fieldSpec).Any(v => bool.Parse(v) == value);
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            _isMatch = GetLongStream(fieldSpec).Any(v => v == value);
        }

        public void VisitEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            if (fieldSpec.IsCaseInsensitive)
            {
                _isMatch = GetFieldValuesAsStream(fieldSpec)
                    .Any(v => string.Equals(v, value, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                _isMatch = GetFieldValuesAsStream(fieldSpec)
                    .Any(v => string.Equals(v, value));
            }
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, bool value)
        {
            _isMatch = !GetStringValues(fieldSpec).Any(v => bool.Parse(v) == value);
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            _isMatch = !GetLongStream(fieldSpec).Any(v => v == value);
        }

        public void VisitNotEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            if (fieldSpec.IsCaseInsensitive)
            {
                _isMatch = !GetFieldValuesAsStream(fieldSpec)
                    .Any(v => string.Equals(v, value, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                _isMatch = !GetFieldValuesAsStream(fieldSpec)
                    .Any(v => string.Equals(v, value));
            }
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, long[] values)
        {
            var inList = values.ToList();
            var documentValues = GetLongStream(fieldSpec).ToList();

            _isMatch = documentValues.Intersect(inList).Any();
        }

        public void VisitIn(IMatcherFieldSpec<Document> fieldSpec, IEnumerable<string> values)
        {
            _isMatch = GetFieldValuesAsStream(fieldSpec).ToList()
                .Intersect(fieldSpec.IsCaseInsensitive ? ToUppercase(values) : values)
                .Any();
        }

        public void VisitContains(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.Contains(valueToCompare));
        }

        public void VisitStartsWith(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.StartsWith(valueToCompare));
        }

        public void VisitLike(IMatcherFieldSpec<Document> fieldSpec, LikeToken[] likeTokens)
        {
            throw new NotImplementedException("Not implemented yet.");
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, long? startValue, long? endValue)
        {
            _isMatch = GetLongStream(fieldSpec).Any(v => v >= startValue && v <= endValue);
        }

        public void VisitBetween(IMatcherFieldSpec<Document> fieldSpec, string startValue, string endValue)
        {
            if (startValue != null && endValue != null)
            {
                string valueStart = fieldSpec.IsCaseInsensitive ? startValue.ToUpper(new CultureInfo("en-US")) : startValue;
                string valueEnd = fieldSpec.IsCaseInsensitive ? endValue.ToUpper(new CultureInfo("en-US")) : endValue;
                _isMatch = GetStringValues(fieldSpec).Any(v => v.CompareTo(valueStart) >= 0 && v.CompareTo(valueEnd) <= 0);
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
            _isMatch = GetLongStream(fieldSpec).Any(v => v < value);
        }

        public void VisitLessThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.CompareTo(valueToCompare) < 0);
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            _isMatch = GetLongStream(fieldSpec).Any(v => v <= value);
        }

        public void VisitLessThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.CompareTo(valueToCompare) < 1);
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            _isMatch = GetLongStream(fieldSpec).Any(v => v > value);
        }

        public void VisitGreaterThan(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.CompareTo(valueToCompare) > 0);
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, long value)
        {
            _isMatch = GetLongStream(fieldSpec).Any(v => v >= value);
        }

        public void VisitGreaterThanOrEquals(IMatcherFieldSpec<Document> fieldSpec, string value)
        {
            string valueToCompare = fieldSpec.IsCaseInsensitive ? value.ToUpper(new CultureInfo("en-US")) : value;

            _isMatch = GetStringValues(fieldSpec).Any(v => v.CompareTo(valueToCompare) >= 0);
        }

        public void VisitExists(IMatcherFieldSpec<Document> fieldSpec)
        {
            _isMatch = GetFieldValues(fieldSpec).Any();
        }

        public void VisitEmpty(IMatcherFieldSpec<Document> fieldSpec)
        {
            // TODO: check this
            //_isMatch = GetStringValues(fieldSpec).Select(v => !string.IsNullOrEmpty(v)).Count() == 0;
            _isMatch = !GetStringValues(fieldSpec).Any(v => v.Length > 0);
        }

        public void VisitOr(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            foreach (Filter<IMatcherFieldSpec<Document>> filter in filters)
            {
                filter.Invoke(this);
                if (_isMatch)
                {
                    break;
                }
            }
        }

        public void VisitAnd(IEnumerable<Filter<IMatcherFieldSpec<Document>>> filters)
        {
            _isMatch = filters.All(filter => filter.IsMatch(_document, _allFullTextFieldSpecs));
        }

        public void VisitNot(Filter<IMatcherFieldSpec<Document>> filter)
        {
            filter.Invoke(this);
            _isMatch = !_isMatch;
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

        //[return: NotNull]
        private IEnumerable<IMatcherFieldValue> GetFieldValues(IMatcherFieldSpec<Document> fieldSpec)
        {
            return fieldSpec.GetFieldValues(_document);
        }

        //[return: NotNull]
        private IEnumerable<long> GetLongStream(IMatcherFieldSpec<Document> fieldSpec)
        {
            return GetStringValues(fieldSpec).Where(v =>
            {
                return long.TryParse(
                    v, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowLeadingSign, null, out long xi);
            }).Select(v => long.Parse(v));
        }

        //[return: NotNull]
        private IEnumerable<string> GetStringValues(IMatcherFieldSpec<Document> fieldSpec)
        {
            var stream = GetFieldValues(fieldSpec)
                .Where(v => !v.IsReference)
                .Select(v => v.StringValue);
            return (fieldSpec.IsCaseInsensitive == true)
            ? stream.Select(v => v.ToUpper(new CultureInfo("en-US")))
            : stream;
        }

        //[return: NotNull]
        private IEnumerable<string> GetFieldValuesAsStream(IMatcherFieldSpec<Document> fieldSpec)
        {
            if (fieldSpec.IsTokenizedPath)
            {
                return GetStringValues(fieldSpec).SelectMany(v => TokenizePath(v));
            }
            else
            {
                return GetStringValues(fieldSpec);
            }
        }

        //[return: NotNull]
        private static List<string> ToUppercase(IEnumerable<string> values)
        {
            return values.Select(v => v.ToUpper(new CultureInfo("en-US"))).ToList();
        }

        //[return: NotNull]
        private static IEnumerable<string> TokenizePath(string path)
        {
            List<string> tokens = new List<string>();
            FileInfo f = new FileInfo(path);
            string fpath = f.FullName;
            do
            {
                tokens.Add(fpath.Replace('\\', '/'));
                fpath = f.Directory.FullName;
            } while (f.Directory != null);
            return tokens;
        }

        private void VisitFieldFullTextImpl(
            IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs,
            FullTextFilter fullTextFilter
        )
        {
            if (fieldSpecs == null || HasFields(fieldSpecs))
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

        private bool HasFields(IEnumerable<IMatcherFieldSpec<Document>> fieldSpecs)
        {
            return fieldSpecs.SelectMany(v => GetFieldValues(v)).Any();
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
                foreach (FullTextFilter fullTextFilter in fullTextFilters)
                {
                    fullTextFilter.Invoke(this);
                    if (_mqVisitor._isMatch)
                    {
                        break;
                    }
                }
            }

            public void VisitAnd(IEnumerable<FullTextFilter> fullTextFilters)
            {
                _mqVisitor._isMatch = fullTextFilters.All(fullTextFilter =>
                {
                    MatcherQueryVisitor<Document> visitor =
                        new MatcherQueryVisitor<Document>(_mqVisitor._document, _mqVisitor._allFullTextFieldSpecs);
                    _mqVisitor.VisitFieldFullTextImpl(_fieldSpecs, fullTextFilter);

                    return visitor.GetResult();
                });
            }

            public void VisitNot(FullTextFilter fullTextFilter)
            {
                fullTextFilter.Invoke(this);
                _mqVisitor._isMatch = !_mqVisitor._isMatch;
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
                MatcherQueryVisitor<Document> mqVisitor) : base(fieldSpecs, mqVisitor)
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
