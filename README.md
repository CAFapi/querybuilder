# Query Builder

The query builder is a library that helps interpret or construct a VQLL query.  
[VQLL](dotnet/src/MicroFocus.CafApi.QueryBuilder.Vqll.Builders.SystemJson) is a Lisp-style grammar that writes values out in JSON arrays.


## Usage
Set up the package source for the QueryBuilder packages in a `nuget.config` file
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="sepg-opensource-nuget-prerelease"
         value="https://svsartifactory.swinfra.net/artifactory/api/nuget/sepg-opensource-nuget-prerelease" />
    </packageSources>
    <packageSourceCredentials>
        <sec-cyberres-nuget-prerelease>
            <add key="Username" value="%SEPG_NUGET_USERNAME%" />
            <add key="ClearTextPassword" value="%SEPG_NUGET_PASSWORD%" />
        </sec-cyberres-nuget-prerelease>
    </packageSourceCredentials>
</configuration>
```


Include the following package dependencies in the project:

```xml
<ItemGroup>
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder" Version="*" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Mapper" Version="*" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Matcher" Version="*" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Vqll.Parsers.SystemJson" Version="*" />
</ItemGroup>
```


# VQLL Parser
The [QueryBuilder.Parsers.SystemJson](dotnet/src/MicroFocus.CafApi.QueryBuilder.Vqll.Parsers.SystemJson) is a library that helps create field filters/clauses by analyzing a query string that conforms to VQLL.


# Filter conversions
The [QueryBuilder.Mapper](dotnet/src/MicroFocus.CafApi.QueryBuilder.Mapper) module can be used to change the type parameter of Filter objects by applying a mapping function lazily.


# Filter matching
The [QueryBuilder.Matcher](dotnet/src/MicroFocus.CafApi.QueryBuilder.Matcher) is a library that allows users to match a document to a `Filter`.

### Matching a document to filter
To match a document to a filter, first create a filter, and then invoke the `filter.IsMatch` method mapping the `Filter<string>` 
to a `Filter<IMatcherFieldSpec>` using the `FilterMapper.Map` method.


### Sample console application

```
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using MicroFocus.CafApi.QueryBuilder;
using MicroFocus.CafApi.QueryBuilder.Mapper;
using MicroFocus.CafApi.QueryBuilder.Matcher;
using MicroFocus.CafApi.QueryBuilder.Vqll.Parsers.SystemJson;

namespace MicroFocus.Verity.QueryBuilderUsage
{
    /*
     * This console application demonstrates how to filter documents based on criteria specified in VQLL.
     */
    public sealed class QueryBuilderSample
    {
        // Create a logger to be passed to the vqll parser
        private static readonly ILogger _filterReaderlogger =
            LoggerFactory
            .Create(b =>b.AddDebug().AddConsole().AddFilter("SystemJsonFilterReader", LogLevel.Error))
            .CreateLogger("SystemJsonFilterReader");


        static void Main(string[] args)
        {
            Console.WriteLine("Prepare some test documents...");
            // Each document is a Dictionary which has field names mapped to a list of string field values 
            IEnumerable<Dictionary<string, List<string>>> documents = GetDocuments();

            Console.WriteLine("Prepare some test criteria...");
            // Filter criteria specified in VQLL
            var vqllQueries = new List<string>
            {
                @"[""=="", ""TITLE"",""Sciullo Properties -- Appraisal""]", // titleEquals
                @"[""between-numbers"",""REPOSITORY_ID"",100,102]" , // repoIdBetween
                @"[""contains"",
                    ""FILE_PATH"",
                    ""//alpha-agent04/EnronData/bailey-s_000/bailey-s/Deleted Items""]" , //filePathContains
                @"[""starts-with"",""TITLE"",""Cheryl""]" , // titleStartsWith
                @"[""and"",
                    [""in"",
                    ""REPOSITORY_ID"",100,101],[""=="",""COLLECTION_STATUS"",""CONTENT""]]" // andQuery
            };

            Console.WriteLine("Check which documents match each of these criteria...");
            // Check which documents match each of these criteria
            foreach (string vqll in vqllQueries)
            { 
                FilterDocuments(vqll, documents);
            }
            Console.WriteLine("Document matching completed.");
        }

        private static void FilterDocuments(string vqll, IEnumerable<Dictionary<string, List<string>>> documents)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Parsing vqll query: '{vqll}'");
            Console.WriteLine("-------------------------------------");
            // Parse vqll to get a Filter object
            Filter<string> filter = ParseVqll(vqll, _filterReaderlogger);

            // Map the 'string' type Filter object to a 'MapKeyMatcherFieldSpec' type Filter.
            // The 'fields' specified in the filter which are strings are mapped to
            // "keys" in a Dictionary representation of a document
            var mappedFilter = FilterMapper<string, MapKeyMatcherFieldSpec>.Map(filter, MapKeyMatcherFieldSpec.Create);

            // Check which documents match the filter
            foreach (Dictionary<string, List<string>> document in documents)
            {
                Console.WriteLine("---- Match: {1} : Document {0} ----",
                    Print(document), mappedFilter.IsMatch(document));
            }
        }

        private static IEnumerable<Dictionary<string, List<string>>> GetDocuments()
        {
            return new List<Dictionary<string, List<string>>>
            {
                {
                    new Dictionary<string, List<string>>()
                    {
                        { "ID", new List<string>() { "1" } },
                        { "IS_ROOT", new List<string>() { "true" } },
                        { "CLASSIFIED", new List<string>() { "true" } },
                        { "COLLECTION_STATUS", new List<string>() { "CONTENT" } },
                        { "FILE_PATH", new List<string>() {
                            "//alpha-agent04/EnronData/Sent Items/enron guaranty (forest oil - conf).doc" } },
                        { "REPOSITORY_ID", new List<string>() { "100" } },
                        { "TITLE", new List<string>() { "enron guaranty (forest oil - conf).doc" } },
                        { "CONTENT", new List<string>() {
                            "ENRON CORP.Guaranty This Guaranty (this “Guaranty”), dated effective as of August 16, 2001 "
                        + " In consideration of Counterparty entering into the Contract, Guarantor agrees as follows:"
                        + " 1. Guarantor hereby irrevocably and unconditionally guarantees"
                        + " the timely payment when due of the obligations of Enron to Counterparty under the Contract." } }
                    }
                },
                {
                    new Dictionary<string, List<string>>()
                    {
                        { "ID", new List<string>() { "2" } },
                        { "IS_ROOT", new List<string>() { "true" } },
                        { "CLASSIFIED", new List<string>() { "false" } },
                        { "COLLECTION_STATUS", new List<string>() { "CONTENT" } },
                        { "FILE_PATH", new List<string>() { "//alpha-agent04/EnronData/Deleted Items/Cheryl N_kv191.msg" } },
                        { "REPOSITORY_ID", new List<string>() { "101" } },
                        { "TITLE", new List<string>() { "Cheryl Nelson - Working From Home" } },
                        { "CONTENT", new List<string>() {
                            "Good Afternoon all! I just received a call from Cheryl detailing that she will be working from home "
                            + "on a project for Frank Sayre." } }
                    }
                },
                {
                    new Dictionary<string, List<string>>()
                    {
                        { "ID", new List<string>() { "3" } },
                        { "IS_ROOT", new List<string>() { "true" } },
                        { "CLASSIFIED", new List<string>() { "true" } },
                        { "COLLECTION_STATUS", new List<string>() { "CONTENT" } },
                        { "FILE_PATH", new List<string>() { "//alpha-agent04/EnronData/Deleted Items/Time exc_kv515.msg" } },
                        { "REPOSITORY_ID", new List<string>() { "101" } },
                        { "TITLE", new List<string>() {
                            "Time exceptions for period from Feb 1st thru Feb 15th--PLEASE EMAIL SUZANNE ADAMS" } },
                        { "CONTENT", new List<string>() {
                            "PLEASE EMAIL YOUR TIME EXCEPTIONS TO SUZANNE ADAMS FOR THE ABOVE-REFERENCED TIME PERIOD.  "
                        + "SHE WILL BE TAKING CARE OF THEM--YOU HAVE CHANGED COST CENTERS--YOU ARE NOW IN COST CENTER 105654.  "
                        + "SINCE SHE WILL HAVE QUITE A FEW THIS TIME, PLEASE GET THEM TO HER AS SOON AS YOU CAN. " } }
                    }
                },
                {
                    new Dictionary<string, List<string>>()
                    {
                        { "ID", new List<string>() { "4" } },
                        { "IS_ROOT", new List<string>() { "false" } },
                        { "CLASSIFIED", new List<string>() { "true" } },
                        { "COLLECTION_STATUS", new List<string>() { "CONTENT" } },
                        { "FILE_PATH", new List<string>() { "//alpha-agent04/EnronData/Sent Items/PCS Nito_kv29.msg" } },
                        { "REPOSITORY_ID", new List<string>() { "102" } },
                        { "TITLE", new List<string>() { "Sciullo Properties -- Appraisal" } },
                        { "CONTENT", new List<string>() {
                            "Leonard, WELCOME BACK !!!!!   Hope you and Dorothy had a pleasant trip to Germany???   "
                        + "Hope all is well with Kasie??? While you were gone, Maria Pirro contacted me -- "
                        + "she is with West Penn Appraisers, Inc. and she faxed their fee schedule "
                        + "and we can draw up a contract." } }
                    }
                },
                {
                    new Dictionary<string, List<string>>()
                    {
                        { "ID", new List<string>() { "5" } },
                        { "IS_ROOT", new List<string>() { "true" } },
                        { "CLASSIFIED", new List<string>() { "true" } },
                        { "COLLECTION_STATUS", new List<string>() { "METADATA" } },
                        { "FILE_PATH", new List<string>() { "//alpha-agent04/EnronData/All documents/Swap Can_kv1905.msg" } },
                        { "REPOSITORY_ID", new List<string>() { "103" } },
                        { "TITLE", new List<string>() { "Swap Candidates -- (EB38C1)" } }
                    }
                }
            };
        }

        private static Filter<string> ParseVqll(string vqll, ILogger logger)
        {
            // Parse the vqll criteria which is a json string and get a JsonNode
            var vqllJsonNode = JsonNode.Parse(vqll)!;
            // Interpret the vqll and create a Filter object
            return SystemJsonFilterReader.ReadFromJsonArray(vqllJsonNode, logger);
        }

        private static string Print(Dictionary<string, List<string>> document)
        {
            return "{\n" + string.Join(",\n",
                document.Select(kv => kv.Key + " = " + Print(kv.Value)).ToArray()) + "\n}";
        }

        private static string PrintOnlyID(Dictionary<string, List<string>> document)
        {
            return "{\n" + string.Join(",\n",
                document
                .Where(kv => kv.Key == "ID")
                .Select(kv => kv.Key + " = " + Print(kv.Value)).ToArray()) + "\n}";
        }

        private static string Print(List<string> values)
        {
            return "[" + string.Join(", ", values) + "]";
        }

    }
}

```

### Sample MatcherFieldSpec
```
using System.Diagnostics.CodeAnalysis;
using MicroFocus.CafApi.QueryBuilder.Matcher;

namespace MicroFocus.Verity.QueryBuilderUsage
{
    public sealed class MapKeyMatcherFieldSpec : IMatcherFieldSpec<Dictionary<string, List<string>>>
    {
        private readonly string _key;
        public static MapKeyMatcherFieldSpec Create(string key)
        {
            return new MapKeyMatcherFieldSpec(key);
        }
        private MapKeyMatcherFieldSpec(string key)
        {
            _key = key;
        }

        [return: NotNull]
        IEnumerable<IMatcherFieldValue> 
            IMatcherFieldSpec<Dictionary<string, List<string>>>.GetFieldValues(Dictionary<string, List<string>> document)
        {
            if (document.ContainsKey(_key))
            {
                return document[_key].Select(value => new MatcherFieldValue(value));
            }
            else
            {
                return Enumerable.Empty<IMatcherFieldValue>();
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsCaseInsensitive
        {
            get
            {
                return _key switch
                {
                    "REPOSITORY_PATH"
                    or "FILE_PATH"
                    or "CLASSIFICATION"
                    => true,
                    _
                    => false,
                };
            }
        }

        bool IMatcherFieldSpec<Dictionary<string, List<string>>>.IsTokenizedPath
        {
            get
            {
                return _key switch
                {
                    "REPOSITORY_PATH"
                    or "FILE_PATH"
                    => true,
                    _
                    => false,
                };
            }
        }

        public override string ToString()
        {
            return "MapKeyMatcherFieldSpec [key=" + _key + "]";
        }

        class MatcherFieldValue : IMatcherFieldValue
        {
            private readonly string _value;
            public MatcherFieldValue(string value)
            {
                _value = value;
            }
            string IMatcherFieldValue.StringValue => _value;

            bool IMatcherFieldValue.IsReference => _value.StartsWith("ref:");
        }

    }
}
```
