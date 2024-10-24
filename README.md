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
        "Guarantor hereby irrevocably and unconditionally guarantees timely payment" } }
}
```
In VQLL a Title equals "some string" is specified as
```
var vqll = @"[""=="", ""TITLE"", ""Sciullo Properties -- Appraisal""]",
```
Parse the vqll criteria which is a json string and get a JsonNode
```
var vqllJsonNode = JsonNode.Parse(vqll);
```

Interpret the vqll and create a Filter object
```
Filter<string> filter = SystemJsonFilterReader.ReadFromJsonArray(vqllJsonNode);
```

Map the 'string' type Filter object to a 'MapKeyMatcherFieldSpec' type Filter.
The 'fields' specified in the filter which are strings are mapped to "keys" in a Dictionary representation of a document
```
bool? isMatch = filter.Map(x => new MapKeyMatcherFieldSpec(x)).IsMatch(document);
```

The `Filter.IsMatch(document)` function follows the [three-valued logic](https://en.wikipedia.org/wiki/Three-valued_logic) and returns  
- `true` if the document matches the filter  
- `false` if the document does not match the filter  
- `null` if fields in the filter are not present in the document and the filter cannot be evaluated

### Sample MatcherFieldSpec
```
public sealed class MapKeyMatcherFieldSpec : IMatcherFieldSpec<Dictionary<string, List<string>>>
{
    private readonly string _key;

    public MapKeyMatcherFieldSpec(string key)
    {
        _key = key;
    }

    public IEnumerable<IMatcherFieldValue>? GetFieldValues(Dictionary<string, List<string>> document)
    {
        return document.TryGetValue(_key, out var value)
            ? value.Select(value => new MatcherFieldValue(value))
            : null;
    }

    public bool IsCaseInsensitive => true;

    public bool IsTokenizedPath => false;

    class MatcherFieldValue : IMatcherFieldValue
    {
        private readonly string _value;
        public MatcherFieldValue(string value)
        {
            _value = value;
        }

        public string StringValue => _value;
        public bool IsReference => false;
    }
}
```
