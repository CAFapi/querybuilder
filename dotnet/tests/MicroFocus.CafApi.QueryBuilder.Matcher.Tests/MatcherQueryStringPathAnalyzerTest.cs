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
namespace MicroFocus.CafApi.QueryBuilder.Matcher.Tests
{
    public sealed class MatcherQueryStringPathAnalyzerTest : MatcherQueryStringTestBase
    {
        private static readonly string SHAREPOINT_PATH = "SHAREPOINT_PATH";
        private static readonly string CASE_SENSITIVE = "CASE_SENSITIVE";
        private static readonly string SINGLE_VALUE_PATH = "SINGLE_VALUE_PATH";
        private static readonly string MIXED_SEPARATOR_VALUE_PATH = "MIXED_SEPARATOR_VALUE_PATH";
        private static readonly string MULTIPLE_VALUE_PATH = "MULTIPLE_VALUE_PATH";

        //  Equality Testing
        [Fact]
        public void SingleValueFieldEqualsString()
        {
            TestEquals(SINGLE_VALUE_PATH, "/A/B/C/D");
            TestEquals(SINGLE_VALUE_PATH, "/A/B/C");
            TestEquals(SINGLE_VALUE_PATH, "/A/B");
            TestEquals(SINGLE_VALUE_PATH, "/A");
            TestEquals(SINGLE_VALUE_PATH, "/a/B/C/D");
            TestEquals(SINGLE_VALUE_PATH, "/a/B/C");
            TestEquals(SINGLE_VALUE_PATH, "/a/B");
            TestEquals(SINGLE_VALUE_PATH, "/a");
            TestEquals(SINGLE_VALUE_PATH, "/Z/A/B/C/D", false);
        }

        [Fact]
        public void SingleValueFieldNotEqualToString()
        {
            TestNotEquals(SINGLE_VALUE_PATH, "/Z/A/B/C/D");
            TestNotEquals(SINGLE_VALUE_PATH, "/Z/a/B/C/D");
            TestNotEquals(SINGLE_VALUE_PATH, "/a", false);
        }

        [Fact]
        public void MixedSeparatorValueFieldEqualsString()
        {
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/A/B/C/D");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/A/B/C");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/A/B");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/A");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/a/B/C/D");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/a/B/C");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/a/B");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/a");
            TestEquals(MIXED_SEPARATOR_VALUE_PATH, "/Z/A/B/C/D", false);
        }

        [Fact]
        public void MixedSeparatorValueFieldNotEqualToString()
        {
            TestNotEquals(MIXED_SEPARATOR_VALUE_PATH, "/Z/A/B/C/D");
            TestNotEquals(MIXED_SEPARATOR_VALUE_PATH, "/Z/a/B/C/D");
            TestNotEquals(MIXED_SEPARATOR_VALUE_PATH, "/a", false);
        }
        [Fact]
        public void MultipleValueFieldEqualsString()
        {
            TestEquals(MULTIPLE_VALUE_PATH, "/E/F/G/H");
            TestEquals(MULTIPLE_VALUE_PATH, "/E/F/G");
            TestEquals(MULTIPLE_VALUE_PATH, "/E/F");
            TestEquals(MULTIPLE_VALUE_PATH, "/E");
            TestEquals(MULTIPLE_VALUE_PATH, "/e/F/G/H");
            TestEquals(MULTIPLE_VALUE_PATH, "/e/F/G");
            TestEquals(MULTIPLE_VALUE_PATH, "/e/F");
            TestEquals(MULTIPLE_VALUE_PATH, "/e");
            TestEquals(MULTIPLE_VALUE_PATH, "/Z/e/f/g/h", false);
        }

        [Fact]
        public void MultipleValueFieldDoesNotEqualsString()
        {
            TestNotEquals(MULTIPLE_VALUE_PATH, "/Z/E/F/G/H");
            TestNotEquals(MULTIPLE_VALUE_PATH, "/Z/e/f/g/h");
            TestNotEquals(MULTIPLE_VALUE_PATH, "/E/F/G/H", false);
        }

        //  In Testing
        [Fact]
        public void SingleValueFieldHasStringInArray()
        {
            TestIn(SINGLE_VALUE_PATH, new string[] { "/A/B/C/D", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/a/B/C/D", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, false, new string[] { "/z/a/b/c/d", "/d/e/f" });
        }

        [Fact]
        public void SingleValueFieldHasStringInList()
        {
            TestIn(SINGLE_VALUE_PATH, new string[] { "/A/B/C/D", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/A/B/C", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/A/B", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/A", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/a/B/C/D", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/a/B/C", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/a/B", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, new string[] { "/a", "/Z/E/F/G/H" });
            TestIn(SINGLE_VALUE_PATH, false, new List<string> { "/z/a/b/c/d", "/d/e/f" });
        }

        [Fact]
        public void MultipleValueFieldHasStringInList()
        {
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/E/F/G/H" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/E/F/G" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/E/F" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/E" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/e/F/G/H" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/e/F/G" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/e/F" });
            TestIn(MULTIPLE_VALUE_PATH, new string[] { "/A/B/C/D", "/e" });
            TestIn(MULTIPLE_VALUE_PATH, false, new string[] { "/X/Y/Z/2", "/A/B/C/D" });
        }

        [Fact]
        public void MultipleValueFieldHasStringInArray()
        {
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/E/F/G/H" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/E/F/G" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/E/F" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/E" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/e/F/G/H" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/e/F/G" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/e/F" });
            TestIn(MULTIPLE_VALUE_PATH, new List<string> { "/A/B/C/D", "/e" });
            TestIn(MULTIPLE_VALUE_PATH, false, new List<string> { "/X/Y/Z/2", "/A/B/C/D" });
        }

        // Contains Testing
        [Fact]
        public void SingleValueFieldContainsString()
        {
            TestContains(SINGLE_VALUE_PATH, "A");
            TestContains(SINGLE_VALUE_PATH, "A/B");
            TestContains(SINGLE_VALUE_PATH, "B/C");
            TestContains(SINGLE_VALUE_PATH, "C/D");
            TestContains(SINGLE_VALUE_PATH, "D");
            TestContains(SINGLE_VALUE_PATH, "a");
            TestContains(SINGLE_VALUE_PATH, "a/b");
            TestContains(SINGLE_VALUE_PATH, "b/c");
            TestContains(SINGLE_VALUE_PATH, "c/d");
            TestContains(SINGLE_VALUE_PATH, "d");
            TestContains(SINGLE_VALUE_PATH, "F/G", false);
        }

        [Fact]
        public void MultiValuedFieldContainsString()
        {
            TestContains(MULTIPLE_VALUE_PATH, "E");
            TestContains(MULTIPLE_VALUE_PATH, "E/F");
            TestContains(MULTIPLE_VALUE_PATH, "F/G");
            TestContains(MULTIPLE_VALUE_PATH, "G/H");
            TestContains(MULTIPLE_VALUE_PATH, "H");
            TestContains(MULTIPLE_VALUE_PATH, "e");
            TestContains(MULTIPLE_VALUE_PATH, "e/F");
            TestContains(MULTIPLE_VALUE_PATH, "f/G");
            TestContains(MULTIPLE_VALUE_PATH, "g/H");
            TestContains(MULTIPLE_VALUE_PATH, "h");
            TestContains(MULTIPLE_VALUE_PATH, "X/Y", false);
        }

        // starts with Testing
        [Fact]
        public void SingleValueFieldStartsWithString()
        {
            TestStartsWith(SINGLE_VALUE_PATH, "/A/B");
            TestStartsWith(SINGLE_VALUE_PATH, "/B/C", false);
        }

        [Fact]
        public void MultiValuedFieldStartsWithString()
        {
            TestStartsWith(MULTIPLE_VALUE_PATH, "/E/F");
            TestStartsWith(MULTIPLE_VALUE_PATH, "/F/G", false);
        }

        [Fact]
        public void SingleValueFieldIsLessThan()
        {
            TestLessThan(SINGLE_VALUE_PATH, "/Z/Z/Z");
            TestLessThan(SINGLE_VALUE_PATH, "/A", false);
            TestLessThan(SINGLE_VALUE_PATH, "/z/z/z");
            TestLessThan(SINGLE_VALUE_PATH, "/a", false);
        }

        [Fact]
        public void MultipleValueFieldIsLessThan()
        {
            TestLessThan(MULTIPLE_VALUE_PATH, "/Z/Z/Z");
            TestLessThan(MULTIPLE_VALUE_PATH, "/A", false);
            TestLessThan(MULTIPLE_VALUE_PATH, "/z/z/z");
            TestLessThan(MULTIPLE_VALUE_PATH, "/a", false);
        }

        [Fact]
        public void SingleValueFieldIsLessThanOrEqual()
        {
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/Z");
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A/B", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/D", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/D/file.ext");
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/file.ext");
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/z");
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a/b", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/d", false);
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/d/file.ext");
            TestLessThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/file.ext");
        }

        [Fact]
        public void MultipleValueFieldIsLessThanOrEqual()
        {
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/M/");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/M/N");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/M/N/O");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "M/N/O/P");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "M/N/O/P/file.ext");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/A/file.ext", false);
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/m/");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/m/n");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/m/n/o");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/m/n/o/p");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/m/n/o/p/file.ext");
            TestLessThanOrEqual(MULTIPLE_VALUE_PATH, "/a/file.ext", false);
        }

        [Fact]
        public void SingleValueFieldIsGreaterThan()
        {
            TestGreaterThan(SINGLE_VALUE_PATH, "/A");
            TestGreaterThan(SINGLE_VALUE_PATH, "/A/B");
            TestGreaterThan(SINGLE_VALUE_PATH, "/A/B/C");
            TestGreaterThan(SINGLE_VALUE_PATH, "/A/B/C/D");
            TestGreaterThan(SINGLE_VALUE_PATH, "/A/B/C/D/file.ext", false);
            TestGreaterThan(SINGLE_VALUE_PATH, "/A/B/C/D/E/file.ext");
            TestGreaterThan(SINGLE_VALUE_PATH, "/a");
            TestGreaterThan(SINGLE_VALUE_PATH, "/a/b");
            TestGreaterThan(SINGLE_VALUE_PATH, "/a/b/c");
            TestGreaterThan(SINGLE_VALUE_PATH, "/a/b/c/d");
            TestGreaterThan(SINGLE_VALUE_PATH, "/a/b/c/d/file.ext", false);
            TestGreaterThan(SINGLE_VALUE_PATH, "/a/b/c/d/e/file.ext");
        }

        [Fact]
        public void MultipleValueFieldIsGreaterThan()
        {
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/I");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/I/J");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/I/J/K");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/I/J/K/L");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/I/J/K/L/file.ext", false);
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/Z/file.ext", false);
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/i/");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/i/j");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/i/j/k");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/i/j/k/l");
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/i/j/k/l/file.ext", false);
            TestGreaterThan(MULTIPLE_VALUE_PATH, "/z/file.ext", false);
        }

        [Fact]
        public void SingleValueFieldIsGreaterThanOrEqual()
        {
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/Z", false);
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A/B");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/D");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/D/file.ext");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/A/B/C/D/E/file.ext");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/z", false);
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a/b");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/d");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/d/file.ext");
            TestGreaterThanOrEqual(SINGLE_VALUE_PATH, "/a/b/c/d/e/file.ext");
        }

        [Fact]
        public void MultipleValueFieldIsGreaterThanOrEqual()
        {
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/E/");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/E/F");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/A/F/G");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/E/F/G/H");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/E/F/G/H/file.ext");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/Z/file.ext", false);
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/e/");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/e/F");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/e/F/G");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/e/F/G/H");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/e/F/G/H/file.ext");
            TestGreaterThanOrEqual(MULTIPLE_VALUE_PATH, "/z/file.ext", false);
        }

        [Fact]
        public void CheckSharepointPathDoesNotThrowException()
        {
            TestEquals(SHAREPOINT_PATH, "http://someserver:8080", false);
            TestEquals(SHAREPOINT_PATH, "http:/someserver:8080");
        }

        [Fact]
        public void CheckIPPathDoesNotThrowException()
        {
            TestEquals("IP_PATH", "\\\\16.103.38.129/testone", false);
            TestEquals("IP_PATH", "//16.103.38.129/testone");
        }

        [Fact]
        public void CheckNoRootPathDoesNotThrowException()
        {
            TestEquals("NO_ROOT_PATH", "SharedFolders/TestData/MSG", false);
            TestEquals("NO_ROOT_PATH", "Shared Folders/TestData/MSG");
        }

        [Fact]
        public void CheckNoRootWindowsPathDoesNotThrowException()
        {
            TestEquals("NO_ROOT_WINDOWS_PATH", "SharedFolders/TestData", false);
            TestEquals("NO_ROOT_WINDOWS_PATH", "Shared Folders/TestData/MSG");
        }

        [Fact]
        public void CheckHostNamePathDoesNotThrowException()
        {
            TestEquals("HOST_NAME_PATH", "\\sourabh-agent1\\Shared Folders\\TestData", false);
            TestEquals("HOST_NAME_PATH", "//sourabh-agent1/Shared Folders/TestData");
        }

        [Fact]
        public void CheckFilePathDoesNotThrowException()
        {
            TestEquals("FILE_PATH", "TestFive.txt", false);
            TestEquals("FILE_PATH", "Test Five.txt");
        }

        protected override Dictionary<string, List<string>> GetDocument()
        {
            Dictionary<string, List<string>> newDocument = new()
            {
                { SHAREPOINT_PATH, new List<string> { "http://someserver:8080/some/path" } },
                { "WINDOWS_PATH", new List<string> { "c:\\A\\B\\C\\D\\file.ext" } },
                { "IP_PATH", new List<string> { "\\\\16.103.38.129\\testone" } },
                { "NO_ROOT_PATH", new List<string> { "Shared Folders/TestData/MSG" } },
                { "NO_ROOT_WINDOWS_PATH", new List<string> { "Shared Folders\\TestData\\MSG" } },
                { "HOST_NAME_PATH", new List<string> { "\\\\sourabh-agent1\\Shared Folders\\TestData\\MSG\\CAll MSG\\Outgoing \"call\" to Jinka Chandrasekhar.msg" } },
                { "FILE_PATH", new List<string> { "Test Five.txt" } },
                { CASE_SENSITIVE, new List<string> { "/A/B/C/D/file.ext" } },
                { SINGLE_VALUE_PATH, new List<string> { "/A/B/C/D/file.ext" } },
                { MIXED_SEPARATOR_VALUE_PATH, new List<string> { "/A/B\\C/D\\file.ext" } },
                { MULTIPLE_VALUE_PATH, new List<string> { "/E/F/G/H/file.ext", "/I/J/K/L/file.ext" } }
            };
            return newDocument;
        }
    }
}
