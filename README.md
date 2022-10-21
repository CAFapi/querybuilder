# Query Builder

The query builder is a library that helps interpret or construct a VQLL query.  
[VQLL](dotnet/src/MicroFocus.CafApi.QueryBuilder.Vqll.Builders.SystemJson) is a Lisp-style grammar that writes values out in JSON arrays.


## Usage

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
to a `Filter<IMatcherFieldSpec>` using the `filter.Map` method.


### Sample application

A sample document is represented by a Dictionary which has field names mapped to a list of string field values 
```
    var document = new Dictionary<string, List<string>>()
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
```
In VQLL a Title equals "<som string>" is specified as
```
    var vqll = @"[""=="", ""TITLE"",""Sciullo Properties -- Appraisal""]",
```
Parse the vqll criteria which is a json string and get a JsonNode
```
    var vqllJsonNode = JsonNode.Parse(vqll)!;
```

Create a logger to be passed to the vqll parser
```
    ILogger logger =
        LoggerFactory
            .Create(b =>b.AddDebug().AddConsole().AddFilter("SystemJsonFilterReader", LogLevel.Error))
            .CreateLogger("SystemJsonFilterReader");
```

// Interpret the vqll and create a Filter object
```
    Filter<string> filter = SystemJsonFilterReader.ReadFromJsonArray(vqllJsonNode, logger);
```

Map the 'string' type Filter object to a 'MapKeyMatcherFieldSpec' type Filter.
The 'fields' specified in the filter which are strings are mapped to "keys" in a Dictionary representation of a document
```
    bool isMatch = filter.Map(MapKeyMatcherFieldSpec.Create).IsMatch(document);
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
