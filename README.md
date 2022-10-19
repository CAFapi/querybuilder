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


Include the following packages in the project:

```xml
<ItemGroup>
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder" Version="1.0.0-US-584051-SNAPSHOT-00020" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Mapper" Version="1.0.0-US-404006-SNAPSHOT-00018" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Matcher" Version="1.0.0-US-584051-SNAPSHOT-00020" />
    <PackageReference Include="MicroFocus.CafApi.QueryBuilder.Vqll.Parsers.SystemJson" Version="1.0.0-US-569038-SNAPSHOT-00009" />
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
            LoggerFactory.Create(b => b.AddDebug().AddConsole().AddFilter("SystemJsonFilterReader", LogLevel.Debug))
            .CreateLogger("SystemJsonFilterReader");
        static void Main(string[] args)
        {
            Console.WriteLine("Prepare some test documents...");
            // Each document is a Dictionary which has field names mapped to a list of string field values 
            IEnumerable<Dictionary<string, List<string>>> documents = GetDocuments();

            // Filter criteria specified in VQLL
            var vqllQueries = new List<string>
            {
                @"[""=="", ""TITLE"",""Sciullo Properties -- Appraisal""]",
                @"[""between-numbers"",""REPOSITORY_ID"",100,102]",
                @"[""contains"",""FILE_PATH"",""//alpha-agent04/EnronData/bailey-s_000/bailey-s/Deleted Items""]",
                @"[""starts-with"",""TITLE"",""Cheryl""]",
                @"[""and"",[""in"",""REPOSITORY_ID"",100,101],[""=="",""COLLECTION_STATUS"",""CONTENT""]]"
            };

            foreach (string vqll in vqllQueries)
            { 
                FilterDocuments(vqll, documents);
            }

            string fulltextQuery = @"[""full-text"",[""full-text"",[""'"",""contract""],[""*""]]]"; // document Has Term
            FulltextFilterDocuments(fulltextQuery, documents);
        }

        private static void FilterDocuments(string vqll, IEnumerable<Dictionary<string, List<string>>> documents)
        {
            Console.WriteLine($"Parsing vqll query: '{vqll}'");
            // Parse vqll to get a Filter object
            Filter<string> filter = ParseVqll(vqll, _filterReaderlogger);

            // Map the 'string' type Filter object to a 'MapKeyMatcherFieldSpec' type Filter.
            // The 'fields' specified in the filter which are strings are mapped
            // to "keys" in a Dictionary representation of a document
            var mappedFilter = FilterMapper<string, MapKeyMatcherFieldSpec>.Map(filter, MapKeyMatcherFieldSpec.Create);

            // Check which documents match the filter
            foreach (Dictionary<string, List<string>> document in documents)
            {
                Console.WriteLine("Document {0} matches filter: {1}", Print(document), mappedFilter.IsMatch(document));
            }
        }

        private static void FulltextFilterDocuments(string vqll, IEnumerable<Dictionary<string, List<string>>> documents)
        {
            Console.WriteLine($"Parsing fulltext vqll query: '{vqll}'");
            // Parse vqll to get a Filter object
            Filter<string> filter = ParseVqll(vqll, _filterReaderlogger);

            // Map the 'string' type Filter object to a 'MapKeyMatcherFieldSpec' type Filter.
            // The 'fields' specified in the filter which are strings are mapped
            // to "keys" in a Dictionary representation of a document
            var mappedFilter = FilterMapper<string, MapKeyMatcherFieldSpec>.Map(filter, MapKeyMatcherFieldSpec.Create);

            List<string> fulltextFields = new() { "CONTENT" };

            // Map the 'string' type fields to a 'MapKeyMatcherFieldSpec'.
            List<MapKeyMatcherFieldSpec>? mappedFullTextFields = fulltextFields?.Select(MapKeyMatcherFieldSpec.Create).ToList();

            // Check which documents match the fulltext filter
            foreach (Dictionary<string, List<string>> document in documents)
            {
                Console.WriteLine("Document {0} matches filter: {1}",
                    Print(document), mappedFilter.IsMatch(document, mappedFullTextFields));
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
                        { "FILE_PATH", new List<string>()
                        { "//alpha-agent04/EnronData/Sent Items/enron guaranty (forest oil - conf).doc" } },
                        { "REPOSITORY_ID", new List<string>() { "100" } },
                        { "TITLE", new List<string>() { "enron guaranty (forest oil - conf).doc" } },
                        { "CONTENT", new List<string>() { "ENRON CORP.\n\nGuaranty\n\nThis Guaranty (this “Guaranty”), "
                        + "dated effective as of August 16, 2001 Guarantor will directly or "
                        + "indirectly benefit from the transactions to be entered into between Enron and Counterparty;"
                        + "NOW THEREFORE, in consideration of Counterparty entering into the Contract, Guarantor hereby "
                        + "covenants and agrees as follows:1.  GUARANTY.  Subject to the provisions "
                        + "hereof, Guarantor hereby irrevocably and unconditionally guarantees the timely payment when due "
                        + "of the obligations of Enron (the “Obligations”) to Counterparty under the Contract." } }
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
                        { "CONTENT", new List<string>() { "Good Afternoon all! I just received a call from Cheryl detailing"
                        + " that she will be working from home on a project for Frank Sayre."
                        + "If you need to reach her you may call her at home at (713)785-6152." } }
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
                        { "TITLE", new List<string>() { "Time exceptions for period from Feb 1st thru Feb 15th--PLEASE EMAIL"
                        + " SUZANNE ADAMS" } },
                        { "CONTENT", new List<string>() { "PLEASE EMAIL YOUR TIME EXCEPTIONS TO SUZANNE ADAMS FOR THE "
                        + "ABOVE-REFERENCED TIME PERIOD.   SHE WILL BE TAKING CARE OF THEM--YOU HAVE CHANGED COST "
                        + "CENTERS--YOU ARE NOW IN COST CENTER 105654.  " +
                        "SINCE SHE WILL HAVE QUITE A FEW THIS TIME, PLEASE GET THEM TO HER AS SOON AS YOU CAN.\n\n" +
                        "Joanne Rozycki\nSenior Administrative Assistant\nEnron North America Corp."
                        + "1400 Smith Street, EB3880D\nHouston, TX  77002\n"
                        + "Phone:  (713) 853-5968     Fax:      (713) 646-3490\n"
                        + "Email:   joanne.rozycki@enron.com" } }
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
                        { "CONTENT", new List<string>()
                        { "Leonard,\n\nWELCOME BACK !!!!!   Hope you and Dorothy had a pleasant trip to Germany???"
                        + "Hope all is well with Kasie??? I would love to hear all about it.\n\nWhile you were gone, "
                        + "Maria Pirro contacted me --  "she is with West Penn Appraisers, Inc. and she faxed their fee"
                        + " schedule relating to the various Sciullo Properties.\n\n"
                        + "I will fax a copy to you, and we can draw up a contract."
                        + "\nCordially, \nSusan S. Bailey\nEnron North America Corp.\n1400 Smith Street, Suite 3803A"
                        + " Houston, Texas 77002\nPhone: (713) 853-4737\nFax: (713) 646-3490\nEmail: Susan.Bailey@enron.com" } }
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
            // Parse the vqll criteria wich is a json string and get a JsonNode
            var vqllJsonNode = JsonNode.Parse(vqll)!;
            return SystemJsonFilterReader.ReadFromJsonArray(vqllJsonNode, logger);
        }

        private static string Print(Dictionary<string, List<string>> document)
        {
            return "{" + string.Join(", ", document.Select(kv => kv.Key + " = " + Print(kv.Value)).ToArray()) + "}";
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
