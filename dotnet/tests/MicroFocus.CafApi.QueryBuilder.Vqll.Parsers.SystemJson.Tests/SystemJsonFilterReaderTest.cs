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

using Xunit;

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MicroFocus.CafApi.QueryBuilder.Vqll.Builders.SystemJson;
using Microsoft.Extensions.Logging;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Parsers.SystemJson.Tests
{
    public sealed class SystemJsonFilterReaderTest
    {
        private static readonly ILogger<SystemJsonFilterReaderTest> _logger =
            LoggerFactory.Create(b => b.AddDebug().AddConsole())
            .CreateLogger<SystemJsonFilterReaderTest>();

        private static readonly ILogger _filterReaderlogger =
            LoggerFactory.Create(
                b => b
                .AddDebug()
                .AddConsole()
                .AddFilter("SystemJsonFilterReader", LogLevel.Debug))
            .CreateLogger("SystemJsonFilterReader");

        [Fact]
        public void TestParseEquals()
        {
            string query = @"[""=="", ""TITLE"",""classified""]";
            Filter<string> clause = FilterFactory.Equals("TITLE", "classified");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParsePhraseEquals()
        {
            string query = "[`==`,`TITLE`,`\\`class\\``]";
            Filter<string> clause = FilterFactory.Equals("TITLE", @"""class""");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseEqualsSpecialChar()
        {
            string query = "[`==`,`TITLE`,`clas{}<>=:/()\\\\sified`]";
            Filter<string> clause = FilterFactory.Equals("TITLE", "clas{}<>=:/()\\sified");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseEqualsNumber()
        {
            string query = "[`==`,`TITLE`,80]";
            Filter<string> clause = FilterFactory.Equals("TITLE", 80);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseEqualsBoolean()
        {
            string query = "[`==`,`IS_HEAD_OF_FAMILY`,true]";
            Filter<string> clause = FilterFactory.Equals("IS_HEAD_OF_FAMILY", true);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseNotEquals()
        {
            string query = "[`!=`,`CLASSIFICATION`,`classified`]";
            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "classified");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseNotEqualsNumber()
        {
            string query = "[`!=`,`file_size`,1000]";
            Filter<string> clause = FilterFactory.NotEquals("file_size", 1000);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseNotEqualsBoolean()
        {
            string query = "[`!=`,`IS_HEAD_OF_FAMILY`,true]";
            Filter<string> clause = FilterFactory.NotEquals("IS_HEAD_OF_FAMILY", true);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeLiteral()
        {
            string query = "[`like`,`TITLE`,[`'`,`classified`]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("classified"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeSingleWildcard()
        {
            string query = "[`like`,`TITLE`,[`'`,`classifie`],[`'`,`?`]]";
            Filter<string> clause = FilterFactory.Like("TITLE",
                                                             LikeTokenFactory.StringLiteral("classifie"),
                                                             LikeTokenFactory.StringLiteral("?"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeAnyWildcard()
        {
            string query = "[`like`,`TITLE`,[`'`,`classified`],[`'`,`*`]]";
            Filter<string> clause = FilterFactory.Like("TITLE",
                                                             LikeTokenFactory.StringLiteral("classified"),
                                                             LikeTokenFactory.StringLiteral("*"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeSpecialChar()
        {
            string query = "[`like`,`TITLE`,[`'`,`clas{}<>=:/()\\\\sified`]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("clas{}<>=:/()\\sified"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeDblQuote()
        {
            string query = "[`like`,`TITLE`,[`'`,`clas\\`s`]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("clas\"s"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeDoubleQuotesInPhrase()
        {
            string query = "[`like`,`TITLE`,[`'`,`\\`clas\\`s\\``]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("\"clas\"s\""));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeSpecialCharPhrase()
        {
            string query = "[`like`,`TITLE`,[`'`,`\\`clas/s\\``]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("\"clas/s\""));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeHasSpace()
        {
            string query = "[`like`,`TITLE`,[`'`,`class work`]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("class work"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeHasSpacePhrase()
        {
            string query = "[`like`,`TITLE`,[`'`,`\\`class work\\``]]";
            Filter<string> clause = FilterFactory.Like("TITLE", LikeTokenFactory.StringLiteral("\"class work\""));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeMultipleLiterals()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane Doe`],[`'`,` John Doe`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane Doe"),
                LikeTokenFactory.StringLiteral(" John Doe"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeOperationWord()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane like`],[`'`,` John Doe`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane like"),
                LikeTokenFactory.StringLiteral(" John Doe"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeOperationWord2()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane `],[`'`,`like`],[`'`,` John Doe`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane "),
                LikeTokenFactory.StringLiteral("like"),
                LikeTokenFactory.StringLiteral(" John Doe")); // include separator spaces in the literals
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeSingleCharWildcard()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane like`],[`?`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane like"),
                LikeTokenFactory.SingleCharacterWildcard());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeMultiCharWildcard()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane like`],[`*`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane like"),
                LikeTokenFactory.ZeroOrMoreCharactersWildcard());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLikeMultiCharEscapedWildcard()
        {
            string query = "[`like`,`TITLE`,[`'`,`Jane like`],[`'`,`\\\\*lmn\\\\abc`]]";
            Filter<string> clause = FilterFactory.Like(
                "TITLE",
                LikeTokenFactory.StringLiteral("Jane like"),
                LikeTokenFactory.StringLiteral("\\*lmn\\abc"));
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseStartsWith()
        {
            string query = "[`starts-with`,`TITLE`,`Jane`]";
            Filter<string> clause = FilterFactory.StartsWith("TITLE", "Jane");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseStartsWithEmptyString()
        {
            string query = "[`starts-with`,`TITLE`,``]";
            Filter<string> clause = FilterFactory.StartsWith("TITLE", "");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseSpecialCharIn()
        {
            string query = "[`in`,`TITLE`,`Jane Doe`,`John/Doe`]";
            Filter<string> clause = FilterFactory.In("TITLE", "Jane Doe", "John/Doe");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseSpecialCharPhraseIn()
        {
            string query = "[`in`,`TITLE`,`Jane Doe`,`\\`John>Doe\\``]";
            Filter<string> clause = FilterFactory.In("TITLE", "Jane Doe", "\"John>Doe\"");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseNumbersIn()
        {
            string query = "[`in`,`TITLE`, 13, 15, 18]";
            Filter<string> clause = FilterFactory.In("TITLE", 13L, 15L, 18L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseInNumbers()
        {
            string query = "[`in-numbers`,`REPOSITORY_ID`, 13, 15, 18]";
            Filter<string> clause = FilterFactory.In("REPOSITORY_ID", 13L, 15L, 18L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseInNumbersNoArgs()
        {
            string query = "[`in-numbers`,`REPOSITORY_ID`]";
            Filter<string> clause = FilterFactory.In("REPOSITORY_ID", Array.Empty<long>());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseInStrings()
        {
            string query = "[`in-strings`,`REPOSITORY_ID`,`6789,3333`,`48,56`]";
            Filter<string> clause = FilterFactory.In("REPOSITORY_ID", "6789,3333", "48,56");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseInStringsNoArgs()
        {
            string query = "[`in-strings`,`REPOSITORY_ID`]";
            Filter<string> clause = FilterFactory.In("REPOSITORY_ID", Array.Empty<string>());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseStringsWithCommasIn()
        {
            string query = "[`in`,`REPOSITORY_ID`,`6789,3333`,`48,56`]";
            Filter<string> clause = FilterFactory.In("REPOSITORY_ID", "6789,3333", "48,56");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseContains()
        {
            string query = "[`contains`,`REPOSITORY_ID`,`1234`]";
            Filter<string> clause = FilterFactory.Contains("REPOSITORY_ID", "1234");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseContainsPath()
        {
            string query = "[`contains`,`FILEPATH`,`\\\\\\\\vika-agent03\\\\Vika Data for upload/0History-Stats service/April21`]";
            Filter<string> clause = FilterFactory.Contains(
                "FILEPATH", "\\\\vika-agent03\\Vika Data for upload/0History-Stats service/April21");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStartAndEndNumbers()
        {
            string query = "[`between-numbers`,`REPOSITORY_ID`,1234,2345]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", 1234L, 2345L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStartNumber()
        {
            string query = "[`between-numbers`,`REPOSITORY_ID`,1234,null]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", 1234L, null);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenEndNumber()
        {
            string query = "[`between-numbers`,`REPOSITORY_ID`, null, 2345]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", null, 2345L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenNumbersNulls()
        {
            string query = "[`between-numbers`,`REPOSITORY_ID`, null, null]";
            long? min = null;
            long? max = null;
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", min, max);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStrings()
        {
            string query = "[`between-strings`,`HOLD`,`abc`,`def`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "abc", "def");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsSpclChars()
        {
            string query = "[`between-strings`,`HOLD`,`(ab>c)`,`[d:ef]`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "(ab>c)", "[d:ef]");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsBkSlsh()
        {
            string query = "[`between-strings`,`HOLD`,`ab\\\\c`,`d:ef`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "ab\\c", "d:ef");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsDblQuote()
        {
            string query = "[`between-strings`, `HOLD`, `ab\\`c`, `d\\\\ef`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "ab\"c", "d\\ef");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsStart()
        {
            string query = "[`between-strings`, `HOLD`, `abc`, null]";
            Filter<string> clause = FilterFactory.Between("HOLD", "abc", null);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsEnd()
        {
            string query = "[`between-strings`,`HOLD`, null, `abc`]";
            Filter<string> clause = FilterFactory.Between("HOLD", null, "abc");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsEmptyStart()
        {
            string query = "[`between-strings`,`HOLD`,` `,`abc`]";
            Filter<string> clause = FilterFactory.Between("HOLD", " ", "abc");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsEmptyEnd()
        {
            string query = "[`between-strings`,`HOLD`,`ttt`,``]";
            Filter<string> clause = FilterFactory.Between("HOLD", "ttt", "");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsWithSpaceInStart()
        {
            string query = "[`between-strings`,`HOLD`,`ab c`,`d:ef`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "ab c", "d:ef");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsWithSpaceInEnd()
        {
            string query = "[`between-strings`,`HOLD`,`abc`,`d ef`]";
            Filter<string> clause = FilterFactory.Between("HOLD", "abc", "d ef");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStringsWithLeadingSpaceInStart()
        {
            string query = "[`between-strings`,`hold`,`  abc  `,`def`]";
            Filter<string> clause = FilterFactory.Between("hold", "  abc  ", "def");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenNumber()
        {
            string query = "[`between`,`REPOSITORY_ID`, null, 2345]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", null, 2345L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenNumberStart()
        {
            string query = "[`between`,`REPOSITORY_ID`, 1234, null]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", 1234L, null);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseBetweeenStartAndEndStrings()
        {
            string query = "[`between`,`REPOSITORY_ID`, `1234`, `2345`]";
            Filter<string> clause = FilterFactory.Between("REPOSITORY_ID", "1234", "2345");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLessThanNumber()
        {
            string query = "[`<`,`repository_id`,1234]";
            Filter<string> clause = FilterFactory.LessThan("repository_id", 1234L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLessThanString()
        {
            string query = "[`<`,`repository_id`,`1234`]";
            Filter<string> clause = FilterFactory.LessThan("repository_id", "1234");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLessThanOrEqualsNumber()
        {
            string query = "[`<=`,`repository_id`,1234]";
            Filter<string> clause = FilterFactory.LessThanOrEquals("repository_id", 1234L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseLessThanOrEqualsString()
        {
            string query = "[`<=`,`repository_id`,`1234`]";
            Filter<string> clause = FilterFactory.LessThanOrEquals("repository_id", "1234");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseGreaterThanNumber()
        {
            string query = "[`>`,`repository_id`,1234]";
            Filter<string> clause = FilterFactory.GreaterThan("repository_id", 1234L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseGreaterThanString()
        {
            string query = "[`>`,`repository_id`,`1234`]";
            Filter<string> clause = FilterFactory.GreaterThan("repository_id", "1234");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseGreaterThanOrEqualsNumber()
        {
            string query = "[`>=`,`repository_id`,1234]";
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("repository_id", 1234L);
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseGreaterThanOrEqualsString()
        {
            string query = "[`>=`,`repository_id`,`1234`]";
            Filter<string> clause = FilterFactory.GreaterThanOrEquals("repository_id", "1234");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseExists()
        {
            string query = "[`exists`,`ACCOUNTS.DISPLAY_NAME`]";
            Filter<string> clause = FilterFactory.Exists("ACCOUNTS.DISPLAY_NAME");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseEmpty()
        {
            string query = "[`empty`,`HOLD`]";
            Filter<string> clause = FilterFactory.Empty("HOLD");
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseOr()
        {
            string query = "[`or`,[`in`,`REPOSITORY_ID`,1234,2345],[`==`,`CLASSIFICATION`,`classified`]]";
            Filter<string> clause1 = FilterFactory.In("REPOSITORY_ID", 1234L, 2345L);
            Filter<string> clause2 = FilterFactory.Equals("CLASSIFICATION", "classified");
            Filter<string> orClause = FilterFactory.Or(clause1, clause2);
            VerifyVqllParsing(query, orClause);
        }

        [Fact]
        public void TestParseOrList()
        {
            string query = "[`or`,[`in`,`REPOSITORY_ID`,1234,2345],[`==`,`CLASSIFICATION`,`classified`]]";
            Filter<string> clause1 = FilterFactory.In("REPOSITORY_ID", 1234L, 2345L);
            Filter<string> clause2 = FilterFactory.Equals("CLASSIFICATION", "classified");
            Filter<string> orClause = FilterFactory.Or(new List<Filter<string>> { clause1, clause2});
            VerifyVqllParsing(query, orClause);
        }

        [Fact]
        public void TestParseOneClauseOr()
        {
            string query = "[`or`,[`==`,`CLASSIFICATION`,`classified`]]";
            Filter<string> clause1 = FilterFactory.Equals("CLASSIFICATION", "classified");
            Filter<string> orClause = FilterFactory.Or(clause1);
            VerifyVqllParsing(query, orClause);
        }

        [Fact]
        public void TestParseEmptyListOr()
        {
            string query = "[`or`]";
            Filter<string> clause = FilterFactory.Or(Enumerable.Empty<Filter<string>>());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseAnd()
        {
            string query = "[`and`,[`in`,`REPOSITORY_ID`,1234,2345],[`==`,`CLASSIFICATION`,`classified`]]";
            Filter<string> clause1 = FilterFactory.In("REPOSITORY_ID", 1234L, 2345L);
            Filter<string> clause2 = FilterFactory.Equals("CLASSIFICATION", "classified");
            Filter<string> andClause = FilterFactory.And(clause1, clause2);
            VerifyVqllParsing(query, andClause);
        }

        [Fact]
        public void TestParseOneClauseAnd()
        {
            string query = "[`and`,[`==`,`CLASSIFICATION`,`classified`]]";
            Filter<string> clause = FilterFactory.Equals("CLASSIFICATION", "classified");
            Filter<string> andClause = FilterFactory.And(clause);
            VerifyVqllParsing(query, andClause);
        }

        [Fact]
        public void TestParseEmptyListAnd()
        {
            string query = "[`and`]";
            Filter<string> clause = FilterFactory.And(Enumerable.Empty<Filter<string>>());
            VerifyVqllParsing(query, clause);
        }

        [Fact]
        public void TestParseNot()
        {
            string query = "[`not`,[`in`,`REPOSITORY_ID`,1234,2345]]";
            Filter<string> clause1 = FilterFactory.In("REPOSITORY_ID", 1234L, 2345L);
            Filter<string> notClause = FilterFactory.Not(clause1);
            VerifyVqllParsing(query, notClause);
        }

        [Fact]
        public void TestParseFullTextQuery()
        {
            string query = "[`full-text`,[`full-text`,[`'`,`Virus`],[`*`]]]";
            FullTextFilter clause = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Virus"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() });
            Filter<string> filter = FilterFactory.FullText<string>(clause);

            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFullTextNearQuery()
        {
            string query = "[`full-text`,[`near`,3,[[`'`,`Ja`],[`*`]],[[`'`,`Doe`]]]]";
            FullTextFilter clause = FullTextFilterFactory.Near(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Ja"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() },
                new LikeToken[] { LikeTokenFactory.StringLiteral("Doe") },
                3);
            Filter<string> filter = FilterFactory.FullText<string>(clause);
            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFullTextDnearQuery()
        {
            string query = "[`full-text`,[`dnear`,1,[[`'`,`jo`],[`?`]],[[`'`,`do`],[`*`]]]]";
            FullTextFilter clause = FullTextFilterFactory.Dnear(
                new LikeToken[] { LikeTokenFactory.StringLiteral("jo"), LikeTokenFactory.SingleCharacterWildcard() },
                new LikeToken[] { LikeTokenFactory.StringLiteral("do"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() },
                1);
            Filter<string> filter = FilterFactory.FullText<string>(clause);
            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFullTextAndQuery()
        {
            string query = "[`full-text`,[`and`,[`full-text`,[`'`,`general,`]],[`full-text`,[`'`,`distribution`]]]]";
            FullTextFilter clause = FullTextFilterFactory.And(
                FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("general,")),
                FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("distribution")));
            Filter<string> filter = FilterFactory.FullText<string>(clause);
            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFullTextOrQuery()
        {
            string query = "[`full-text`,[`or`,"
                + "[`full-text`,[`'`,`\\``]],"
                + "[`full-text`,[`'`,`test<abc`]],"
                + "[`full-text`,[`'`,`test>abc`]]"
                + "]]";
            FullTextFilter clause1 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("\"") });
            FullTextFilter clause2 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test<abc") });
            FullTextFilter clause3 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test>abc") });
            FullTextFilter orTerms = FullTextFilterFactory.Or(new FullTextFilter[] { clause1, clause2, clause3 });
            Filter<string> filter = FilterFactory.FullText<string>(orTerms);
            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFullTextNotQuery()
        {
            string query = "[`full-text`,[`not`,[`full-text`,[`'`,`test>abc`]]]]";
            FullTextFilter clause = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test>abc") });
            FullTextFilter notClause = FullTextFilterFactory.Not(clause);
            Filter<string> filter = FilterFactory.FullText<string>(notClause);
            VerifyVqllParsing(query, filter);
        }

        [Fact]
        public void TestParseFieldFullTextQuery()
        {
            string query = "[`field-full-text`,"
                + "[`TITLE`,`CONTENT_PRIMARY`],"
                + "[`or`,"
                + "[`near`,3,[[`'`,`Ja`],[`*`]],[[`'`,`Doe`]]],"
                + "[`dnear`,1,[[`'`,`jo`],[`?`]],[[`'`,`do`],[`*`]]],"
                + "[`full-text`,[`'`,`Virus`],[`*`]],"
                + "[`full-text`,[`'`,`Worm`],[`?`]],"
                + "[`full-text`,[`'`,`test`]],"
                + "[`and`,[`full-text`,[`'`,`general,`]],[`full-text`,[`'`,`distribution`]]],"
                + "[`full-text`,[`'`,`Classified`],[`'`,`Documents`]],"
                + "[`full-text`,[`'`,`Top`],[`'`,`(Secret)`]],"
                + "[`full-text`,[`'`,`J\\\\on:es`],[`'`,`\\\\Fin=an/cial`],[`'`,`{abc}`],[`'`,`(Edw+ard`],[`'`,`Jon-es)`]],"
                + "[`full-text`,[`'`,`[Tom]\\\\<Cook>`],[`'`,`~you`]],"
                + "[`full-text`,[`'`,`\\\\`]],"
                + "[`full-text`,[`'`,`\\``]],"
                + "[`full-text`,[`'`,`test<abc`]],"
                + "[`full-text`,[`'`,`test>abc`]],"
                + "[`full-text`,[`'`,`test/abc`]]"
                + "]"
                + "]";

            FullTextFilter clause1 = FullTextFilterFactory.Near(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Ja"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() },
                new LikeToken[] { LikeTokenFactory.StringLiteral("Doe") },
                3);
            FullTextFilter clause2 = FullTextFilterFactory.Dnear(
                new LikeToken[] { LikeTokenFactory.StringLiteral("jo"), LikeTokenFactory.SingleCharacterWildcard() },
                new LikeToken[] { LikeTokenFactory.StringLiteral("do"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() },
                1);
            FullTextFilter clause3 = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Virus"), LikeTokenFactory.ZeroOrMoreCharactersWildcard() });
            FullTextFilter clause4 = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Worm"), LikeTokenFactory.SingleCharacterWildcard() });
            FullTextFilter clause5 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test") });
            FullTextFilter clause6 = FullTextFilterFactory.And(
                FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("general,")),
                FullTextFilterFactory.FullText(LikeTokenFactory.StringLiteral("distribution")));
            FullTextFilter clause7 = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Classified"), LikeTokenFactory.StringLiteral("Documents") });
            FullTextFilter clause8 = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("Top"), LikeTokenFactory.StringLiteral("(Secret)") });
            FullTextFilter clause9 = FullTextFilterFactory.FullText(
                new LikeToken[]{LikeTokenFactory.StringLiteral("J\\on:es"),
                            LikeTokenFactory.StringLiteral("\\Fin=an/cial"),
                            LikeTokenFactory.StringLiteral("{abc}"),
                            LikeTokenFactory.StringLiteral("(Edw+ard"),
                            LikeTokenFactory.StringLiteral("Jon-es)")});
            FullTextFilter clause10 = FullTextFilterFactory.FullText(
                new LikeToken[] { LikeTokenFactory.StringLiteral("[Tom]\\<Cook>"), LikeTokenFactory.StringLiteral("~you") });
            FullTextFilter clause11 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("\\") });
            FullTextFilter clause12 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("\"") });
            FullTextFilter clause13 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test<abc") });
            FullTextFilter clause14 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test>abc") });
            FullTextFilter clause15 = FullTextFilterFactory.FullText(new LikeToken[] { LikeTokenFactory.StringLiteral("test/abc") });

            FullTextFilter orTerms = FullTextFilterFactory.Or(new FullTextFilter[] { clause1, clause2, clause3, clause4, clause5, clause6,
                                                                                  clause7, clause8, clause9, clause10, clause11, clause12,
                                                                                  clause13, clause14, clause15 });

            Filter<string> clause = FilterFactory.FieldFullText(new string[] { "TITLE", "CONTENT_PRIMARY" }, orTerms);

            VerifyVqllParsing(query, clause);
        }

        private static void VerifyVqllParsing(string vqllQuery, Filter<string> expectedClause)
        {
            JsonNode vqllNode = GetJsonNode(vqllQuery);
            Filter<string> actualClause = SystemJsonFilterReader.ReadFromJsonArray(vqllNode, _filterReaderlogger);

            Assert.NotNull(actualClause);

            string transformedActualClause = ToVqll(actualClause);
            string transformedExpectedClause = ToVqll(expectedClause);

            _logger.LogInformation("Comparing expected transformation '{0}' to actual '{1}'", transformedExpectedClause,
                        transformedActualClause);

            Assert.Equal(transformedExpectedClause, transformedActualClause);
        }

        private static JsonNode GetJsonNode(string backtickVqll)
        {
            string vqll = backtickVqll.Replace('`', '"');
            return JsonNode.Parse(vqll)!;
        }

        private static string ToVqll(Filter<string> filter)
        {
            using var stream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(stream))
            {
                filter.WriteToJsonArray(jsonWriter);
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
