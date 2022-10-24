# Query Builder Matcher

The query builder matcher is a library that allows users to match a document to a `Filter`.

### Creating Filters
Filters can be constructed using the `FilterFactory` methods to create `Filter<string>` objects.  
The following Filters can be created:

- `Equals(fieldName, bool|long|string)` match value of Field exactly
- `NotEquals(fieldName, bool|long|string)` match if value is not equal to Field value
- `In(fieldName, long[]|IEnumerable<string>)` match Field value to any of the listed values
- `Contains(fieldName, string)` match if the Field value contains a substring specified by the search term
- `StartsWith(fieldName, string)` match if the Field value starts with the string specified by the search term
- `Between(fieldName, long, long)` match if the Field value contains a value between startValue and endValue inclusive.
- `LessThan(fieldName, long|string)` match if Field value is less than specified value
- `LessThanOrEquals(fieldName, long|string)` match if Field value is less than or equal to specified value
- `GreaterThan(fieldName, long|string)` match if Field value is greater than specified value
- `GreaterThanOrEquals(fieldName, long|string)` match if Field value is greater than or equal to specified value
- `Exists(fieldName)` match if the specified Field exists (even if it contains no value)
- `Empty(fieldName)` match if the specified Field does not exist or contains no value
- `Or(IEnumerable<Filter>)` match any of the specified Filters
- `And(IEnumerable<Filter>)` match all of the specified Filters
- `Not(Filter<string>)` match if the specified Field does not match the specified Filter

#### Usage
To create a filter on `CONTENT_PRIMARY` containing the text `address`  
`Filter<string> filter = FilterFactory.Contains("CONTENT_PRIMARY", "address");`
    
To create a filter on `FILESIZE` greater than `10`   
`Filter<string> filter = FilterFactory.GreaterThan("FILESIZE", 10);`

### Matching a document to filter
To match a document to a filter, first create a filter, and then invoke the `filter.IsMatch` method mapping the `Filter<string>` 
to a `Filter<IMatcherFieldSpec>` using the `FilterMapper.Map` method.

#### Usage
    Filter<string> filter = FilterFactory.Contains("CONTENT_PRIMARY", "address");
    var remappedFilter = FilterMapper.Map(filter, x => new SchemaFieldAdapter(x));
    remappedFilter.IsMatch(document);
