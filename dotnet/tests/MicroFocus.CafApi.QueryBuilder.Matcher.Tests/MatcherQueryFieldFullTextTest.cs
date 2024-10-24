/**
 * Copyright 2022-2024 Open Text.
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
using Microsoft.Extensions.Logging;

namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MatcherQueryFieldFullTextTest
    {
        private static readonly ILogger<MatcherQueryFieldFullTextTest> LOGGER =
                LoggerFactory.Create(b => b.AddDebug().AddConsole())
                .CreateLogger<MatcherQueryFieldFullTextTest>();

        private static Dictionary<string, List<string>>? metadataOnlyDocument;
        private static Dictionary<string, List<string>>? contentDocument;
        private static Filter<string>? simpleFullTextFilter;
        private static Filter<string>? andFullTextFilter;
        private static Filter<string>? andOnlyFullTextFilter;
        private static Filter<string>? orFullTextFilter;
        private static Filter<string>? filterNotFullText;
        private static Filter<string>? notFilterOfFullText;

        // field names
        private static readonly string SINGLE_VALUE = "SINGLE_VALUE";
        private static readonly string MULTIPLE_VALUE = "MULTIPLE_VALUE";
        private static readonly string IS_EMPTY = "IS_EMPTY";
        private static readonly string IS_ACTUALLY_EMPTY = "IS_ACTUALLY_EMPTY";
        private static readonly string REFERENCE_FIELD = "REFERENCE_FIELD";
        private static readonly string LANGUAGE_CODE = "LANGUAGE_CODE";
        private static readonly string FILE_SIZE = "FILE_SIZE";
        // full text field names
        private static readonly string ADDRESS_DISPLAY = "ADDRESS_DISPLAY";
        private static readonly string CONTENT_PRIMARY = "CONTENT_PRIMARY";
        private static readonly string TITLE = "TITLE";

        private static readonly List<string> CONFIGURED_FULLTEXT_FIELDS = new() { ADDRESS_DISPLAY, CONTENT_PRIMARY, TITLE };
        private static readonly List<string> NO_CONFIGURED_FULLTEXT_FIELDS = new();

        private static void PrepareData()
        {
            // Prepare some documents
            metadataOnlyDocument = GetMetadataOnlyDocument();
            contentDocument = GetContentDocument();

            // Prepare some filters
            simpleFullTextFilter = GetSimpleFullTextFilter();
            andOnlyFullTextFilter = GetAndOnlyFullTextFilter();
            andFullTextFilter = GetAndFullTextFilter();
            orFullTextFilter = GetOrFullTextFilter();
            filterNotFullText = GetFilterOfNotFullText();
            notFilterOfFullText = GetNotFilterOfFullText();
        }

        public static IEnumerable<object?[]> Data(int numTests)
        {
            PrepareData();
            return new List<object?[]>
            {
                // VQL: TITLE:{Julie*}
                new object?[]{"fullTextNullFullTextFieldsContentDoc", contentDocument, simpleFullTextFilter, null, false, true},
                new object?[]{"fullTextNullFullTextFieldsMetadataDoc", metadataOnlyDocument, simpleFullTextFilter, null, false, true},
                new object?[]{"fullTextNoFullTextFieldsContentDoc", contentDocument, simpleFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"fullTextNoFullTextFieldsMetadataDoc", metadataOnlyDocument, simpleFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false,
                 true},
                new object?[]{"fullTextFullTextFieldsAndContentDoc", contentDocument, simpleFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"fullTextFullTextFieldsAndMetadataDoc", metadataOnlyDocument, simpleFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false,
                 true},
                // VQL: (SINGLE_VALUE == \"TIGER\") AND TITLE,ADDRESS_DISPLAY,CONTENT_PRIMARY:{(Julie* AND *Shakespear)}
                new object?[]{"andFullTextNullFullTextFieldsContentDoc", contentDocument, andFullTextFilter, null, false, true},
                new object?[]{"andFullTextNullFullTextFieldsMetadataDoc", metadataOnlyDocument, andFullTextFilter, null, false, true},
                new object?[]{"andFullTextNoFullTextFieldsContentDoc", contentDocument, andFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"andFullTextNoFullTextFieldsMetadataDoc", metadataOnlyDocument, andFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false,
                 false},
                new object?[]{"andFullTextFullTextFieldsContentDoc", contentDocument, andFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"andFullTextFullTextFieldsMetadataDoc", metadataOnlyDocument, andFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false, false},
                // VQL: TITLE,ADDRESS_DISPLAY,CONTENT_PRIMARY:{(Julie NEAR3 Shakespeare) AND (Romeo)}
                new object?[]{"andOnlyFullTextNullFullTextFieldsContentDoc", contentDocument, andOnlyFullTextFilter, null, false, true},
                new object?[]{"andOnlyFullTextNullFullTextFieldsMetadataDoc", metadataOnlyDocument, andOnlyFullTextFilter, null, false, true},
                new object?[]{"andOnlyFullTextNoFullTextFieldsContentDoc", contentDocument, andOnlyFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false,
                 true},
                new object?[]{"andOnlyFullTextNoFullTextFieldsMetadataDoc", metadataOnlyDocument, andOnlyFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS,
                 false, false},
                new object?[]{"andOnlyFullTextFullTextFieldsContentDoc", contentDocument, andOnlyFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false,
                 true},
                new object?[]{"andOnlyFullTextFullTextFieldsMetadataDoc", metadataOnlyDocument, andOnlyFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false,
                 false},
                // VQL: TITLE:{Julie* OR *Shakespear}
                new object?[]{"orFullTextNullFullTextFieldsContentDoc", contentDocument, orFullTextFilter, null, false, true},
                new object?[]{"orFullTextNullFullTextFieldsMetadataDoc", metadataOnlyDocument, orFullTextFilter, null, false, true},
                new object?[]{"orFullTextNoFullTextFieldsContentDoc", contentDocument, orFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"orFullTextNoFullTextFieldsMetadataDoc", metadataOnlyDocument, orFullTextFilter, NO_CONFIGURED_FULLTEXT_FIELDS, false,
                 false},
                new object?[]{"orFullTextFullTextFieldsContentDoc", contentDocument, orFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false, true},
                new object?[]{"orFullTextFullTextFieldsMetadataDoc", metadataOnlyDocument, orFullTextFilter, CONFIGURED_FULLTEXT_FIELDS, false, false},
                // VQL: (NOT TITLE,CONTENT_PRIMARY:{Julie*}) Filter.fullTextFilter(not(fullTextFilter))
                new object?[]{"notFullTextFilterNullFullTextFieldsContentDoc", contentDocument, filterNotFullText, null, false, true},
                new object?[]{"notFullTextFilterNullFullTextFieldsMetadataDoc", metadataOnlyDocument, filterNotFullText, null, true, false},
                new object?[]{"notFullTextNoFullTextFieldsFilterContentDoc", contentDocument, filterNotFullText, NO_CONFIGURED_FULLTEXT_FIELDS, true,
                 true},
                new object?[]{"notFullTextNoFullTextFieldsFilterMetadataDoc", metadataOnlyDocument, filterNotFullText, NO_CONFIGURED_FULLTEXT_FIELDS,
                 true, false},
                new object?[]{"notFullTextAvailableFullTextFieldsFilterContentDoc", contentDocument, filterNotFullText, CONFIGURED_FULLTEXT_FIELDS, false,
                 true},
                new object?[]{"notFullTextAvailableFullTextFieldsFilterMetadataDoc", metadataOnlyDocument, filterNotFullText, CONFIGURED_FULLTEXT_FIELDS,
                 true, false},
                // VQL: (NOT TITLE,CONTENT_PRIMARY:{Julie*}) Filter.not(fullTextFilter)
                new object?[]{"notFilterFullTextNullFullTextFieldsContentDoc", contentDocument, notFilterOfFullText, null, false, true},
                new object?[]{"notFilterFullTextNullFullTextFieldsMetadataDoc", metadataOnlyDocument, notFilterOfFullText, null, true, true},
                new object?[]{"notFilterFullTextNoFullTextFieldsContentDoc", contentDocument, notFilterOfFullText, NO_CONFIGURED_FULLTEXT_FIELDS, true,
                 true},
                new object?[]{"notFilterFullTextNoFullTextFieldsMetadataDoc", metadataOnlyDocument, notFilterOfFullText, NO_CONFIGURED_FULLTEXT_FIELDS,
                 true, false},
                new object?[]{"notFilterFullTextAvaialableFullTextFieldsContentDoc", contentDocument, notFilterOfFullText, CONFIGURED_FULLTEXT_FIELDS,
                 false, true},
                new object?[]{"notFilterFullTextAvaialableFullTextFieldsMetadataDoc", metadataOnlyDocument, notFilterOfFullText,
                 CONFIGURED_FULLTEXT_FIELDS, true, false}
            }
            .Take(numTests);
        }

        [Theory]
        [MemberData(nameof(Data), parameters: 6)]
        public void TestIsMatch(string message,
            Dictionary<string, List<string>> inputDocument,
            Filter<string> inputFilter,
            List<string> inputAllFullTextFields,
            bool? expectedMatch,
            bool expectedUnsupportedOperationException)
        {
            LOGGER.LogInformation("Testing {Message}", message);
            try
            {
                bool? isMatch = DocMatches(inputDocument, inputFilter, inputAllFullTextFields);
                Assert.Equal(expectedMatch, isMatch);
            }
            catch (NotSupportedException e)
            {
                if (expectedUnsupportedOperationException)
                {
                    Assert.True(e.Message.Contains("Not supported"), "Should have thrown UnsupportedOperationException");
                }
                else
                {
                    Assert.True(false, "Should not have thrown UnsupportedOperationException");
                }
            }
        }

        private static bool? DocMatches(
            Dictionary<string, List<string>> document,
            Filter<string> filter,
            List<string> allFullTextFields)
        {
            LOGGER.LogInformation("Matching document {Document}", MatcherTestHelper.Print(document));
            List<MapKeyMatcherFieldSpec>? fullTextFieldSpecs =
                allFullTextFields?.Select(x => new MapKeyMatcherFieldSpec(x)).ToList();
            return filter.Map(x => new MapKeyMatcherFieldSpec(x)).IsMatch(document, fullTextFieldSpecs);
        }

        private static Dictionary<string, List<string>> GetMetadataOnlyDocument()
        {
            Dictionary<string, List<string>> document = new()
            {
                { SINGLE_VALUE, new List<string> { "TIGER" } },
                { MULTIPLE_VALUE, new List<string> { "ANACONDA", "MOOSE", "ZEBRA" } },
                { IS_EMPTY, new List<string> { "" } },
                { REFERENCE_FIELD, new List<string> { "ref:CHAMELEON" } },
                { IS_ACTUALLY_EMPTY, new List<string>() },
                { LANGUAGE_CODE, new List<string> { "en" } },
                { FILE_SIZE, new List<string> { "500" } }
            };

            return document;
        }

        private static Dictionary<string, List<string>> GetContentDocument()
        {
            Dictionary<string, List<string>> document = new()
            {
                { SINGLE_VALUE, new List<string> { "TIGER" } },
                { MULTIPLE_VALUE, new List<string> { "ANACONDA", "MOOSE", "ZEBRA" } },
                { IS_EMPTY, new List<string> { "" } },
                { REFERENCE_FIELD, new List<string> { "ref:CHAMELEON" } },
                { IS_ACTUALLY_EMPTY, new List<string>() },
                { LANGUAGE_CODE, new List<string> { "en" } },
                { FILE_SIZE, new List<string> { "500" } },

                { ADDRESS_DISPLAY, new List<string> { "johndoe@abc.com", "janedoe@abc.com" } },
                {
                    CONTENT_PRIMARY,
                    new List<string>{"A rose by any other name would smell as sweet is a popular adage from "
                         + "William Shakespeare's play Romeo and Juliet, in which "
                         + "Juliet seems to argue that it does not matter that Romeo is from her family's rival house of Montague. "
                         + "The reference is used to state that the names of things do not affect what they really are."}
                },
                { TITLE, new List<string> { "A rose by any name" } }
            };

            return document;
        }

        private static Filter<string> GetSimpleFullTextFilter()
        {
            FullTextFilter julieAny = FullTextFilterFactory.FullText(
                LikeTokenFactory.StringLiteral("Julie"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            return FilterFactory.FieldFullText(new List<string> { TITLE }, julieAny);
        }

        private static Filter<string> GetAndFullTextFilter()
        {
            FullTextFilter julieAny = FullTextFilterFactory.FullText(
                LikeTokenFactory.StringLiteral("Julie"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            FullTextFilter anyShakespeare = FullTextFilterFactory.FullText(
                LikeTokenFactory.ZeroOrMoreCharactersWildcard(),
                (LikeTokenFactory.StringLiteral("Shakespeare")));

            Filter<string> equalsFilter = FilterFactory.Equals(SINGLE_VALUE, "TIGER");
            Filter<string> fullTextFiltersAnd = FilterFactory.FieldFullText(
                new List<string> { TITLE, ADDRESS_DISPLAY, CONTENT_PRIMARY }, FullTextFilterFactory.And(julieAny, anyShakespeare));

            return FilterFactory.And(equalsFilter, fullTextFiltersAnd);
        }

        private static Filter<string> GetAndOnlyFullTextFilter()
        {
            FullTextFilter nearFilter = FullTextFilterFactory.Near(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Julie") },
                new LikeToken[] { LikeTokenFactory.StringLiteral("Shakespeare") },
                3);
            FullTextFilter romeoFilter = FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("Romeo"));
            return FilterFactory.FieldFullText(
                new List<string> { TITLE, ADDRESS_DISPLAY, CONTENT_PRIMARY }, FullTextFilterFactory.And(nearFilter, romeoFilter));
        }

        private static Filter<string> GetOrFullTextFilter()
        {
            FullTextFilter julieAny = FullTextFilterFactory.FullText(
                LikeTokenFactory.StringLiteral("Julie"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            FullTextFilter anyShakespeare = FullTextFilterFactory.FullText(
                LikeTokenFactory.ZeroOrMoreCharactersWildcard(),
                (LikeTokenFactory.StringLiteral("Shakespeare")));
            return FilterFactory.FieldFullText(new List<string> { TITLE }, FullTextFilterFactory.Or(julieAny, anyShakespeare));
        }

        private static Filter<string> GetFilterOfNotFullText()
        {
            FullTextFilter julieAny = FullTextFilterFactory.FullText(
                LikeTokenFactory.StringLiteral("Julie"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            FullTextFilter notJulieAny = FullTextFilterFactory.Not(julieAny);
            return FilterFactory.FieldFullText(new List<string> { TITLE, CONTENT_PRIMARY }, notJulieAny);
        }

        private static Filter<string> GetNotFilterOfFullText()
        {
            FullTextFilter julieAny = FullTextFilterFactory.FullText(
                LikeTokenFactory.StringLiteral("Julie"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            Filter<string> filter = FilterFactory.FieldFullText(new List<string> { TITLE, CONTENT_PRIMARY }, julieAny);
            return FilterFactory.Not(filter);
        }
    }
}
