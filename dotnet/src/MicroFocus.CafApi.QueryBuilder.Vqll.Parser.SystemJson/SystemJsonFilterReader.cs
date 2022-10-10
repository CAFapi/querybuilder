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
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Parser.SystemJson
{
    public sealed class SystemJsonFilterReader
    {
        private readonly ILogger _logger;

        private SystemJsonFilterReader(ILogger logger)
        {
            _logger = logger;
        }

        public static Filter<string> ReadFromJsonArray(JsonArray vqll, ILogger logger)
        {
            return new SystemJsonFilterReader(logger).CreateFilter(vqll);
        }

        private Filter<string> CreateFilter(JsonArray node)
        {
            _logger.LogDebug("Parsing: {0}", node);
            string filterType = node[0].GetValue<string>();
            switch (filterType)
            {
                case "==":
                    return CreateEqualsFilter(node);
                case "!=":
                    return CreateNotEqualsFilter(node);
                case "in":
                    return CreateInFilter(node);
                case "in-numbers":
                    return CreateInNumbersFilter(node);
                case "in-strings":
                    return CreateInstringsFilter(node);
                case "contains":
                    return CreateContainsFilter(node);
                case "starts-with":
                    return CreateStartsWithFilter(node);
                case "like":
                    return CreateLikeFilter(node);
                case "between":
                    return CreateBetweenFilter(node);
                case "between-numbers":
                    return CreateBetweenNumbersFilter(node);
                case "between-strings":
                    return CreateBetweenstringsFilter(node);
                case "<":
                    return CreateLessThanFilter(node);
                case "<=":
                    return CreateLessThanOrEqualsFilter(node);
                case ">":
                    return CreateGreaterThanFilter(node);
                case ">=":
                    return CreateGreaterThanOrEqualsFilter(node);
                case "exists":
                    return CreateExistsFilter(node);
                case "empty":
                    return CreateEmptyFilter(node);
                case "and":
                    return CreateAndFilter(node);
                case "or":
                    return CreateOrFilter(node);
                case "not":
                    return CreateNotFilter(node);
                case "full-text":
                    return CreateFullTextFilter(node);
                case "field-full-text":
                    return CreateFieldFullTextFilter(node);
                default:
                    throw new ArgumentException("Unexpected 'filter type' in vqll: " + filterType);
            }
        }

        private Filter<string> CreateEqualsFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return FilterFactory.Equals(node[1].GetValue<string>(), node[2].GetValue<bool>());
                case JsonValueKind.Number:
                    return FilterFactory.Equals(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.Equals(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'equals' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateNotEqualsFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return FilterFactory.NotEquals(node[1].GetValue<string>(), node[2].GetValue<bool>());
                case JsonValueKind.Number:
                    return FilterFactory.NotEquals(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.NotEquals(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'not equals' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateInFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.Number:
                    return FilterFactory.In(node[1].GetValue<string>(), GetLongValues(node));
                case JsonValueKind.String:
                    return FilterFactory.In(node[1].GetValue<string>(), GetStringValues(node));
                default:
                    throw new ArgumentException("Unexpected value type in 'in' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateInNumbersFilter(JsonArray node)
        {
            if (node.Count == 2)
            {
                return FilterFactory.In(node[1].GetValue<string>(), Array.Empty<long>());
            }
            else
            {
                return CreateInFilter(node);
            }
        }

        private Filter<string> CreateInstringsFilter(JsonArray node)
        {
            if (node.Count == 2)
            {
                return FilterFactory.In(node[1].GetValue<string>(), Array.Empty<string>());
            }
            else
            {
                return CreateInFilter(node);
            }
        }

        private long[] GetLongValues(JsonArray node)
        {
            var inValues = new List<long>();
            IEnumerator<JsonNode> iter = GetFilterArgumentsElements(node);
            while (iter.MoveNext())
            {
                inValues.Add(iter.Current.GetValue<long>());
            }
            return inValues.ToArray();
        }

        private string[] GetStringValues(JsonArray node)
        {
            var inValues = new List<string>();

            IEnumerator<JsonNode> iter = GetFilterArgumentsElements(node);
            while (iter.MoveNext())
            {
                inValues.Add(iter.Current.GetValue<string>());
            }
            return inValues.ToArray();
        }

        private Filter<string> CreateContainsFilter(JsonArray node)
        {
            return FilterFactory.Contains(node[1].GetValue<string>(), node[2].GetValue<string>());
        }

        private Filter<string> CreateStartsWithFilter(JsonArray node)
        {
            return FilterFactory.StartsWith(node[1].GetValue<string>(), node[2].GetValue<string>());
        }

        private Filter<string> CreateLikeFilter(JsonArray node)
        {
            return FilterFactory.Like(node[1].GetValue<string>(), GetLikeTokens(GetFilterArgumentsElements(node)));
        }

        private IEnumerator<JsonNode> GetFilterArgumentsElements(JsonArray node)
        {
            IEnumerator<JsonNode> iter = node.GetEnumerator();
            iter.MoveNext(); // filter type
            iter.MoveNext(); // field name
            return iter;
        }

        private Filter<string> CreateBetweenFilter(JsonArray node)
        {
            bool isStartANumber = false;
            Object start;
            if (node[2] == null)
            {
                start = null;
            }
            else
            {
                switch (node[2].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.Number:
                        start = node[2].GetValue<long?>();
                        isStartANumber = true;
                        break;
                    case JsonValueKind.String:
                        start = node[2].GetValue<string>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected start value type in 'between' filter: " + node[2].GetValue<JsonElement>().ValueKind);
                }
            }
            bool isEndANumber = false;
            Object end;
            if (node[3] == null)
            {
                end = null;
            }
            else
            {
                switch (node[3].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.Number:
                        end = node[3].GetValue<long?>();
                        isEndANumber = true;
                        break;
                    case JsonValueKind.String:
                        end = node[3].GetValue<string>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected end value type in 'between' filter: " + node[3].GetValue<JsonElement>().ValueKind);
                }
            }

            if (isStartANumber || isEndANumber)
            {
                Filter<string> betweenFilter =
                    start == null
                    ? FilterFactory.Between(node[1].GetValue<string>(), null, Convert.ToInt64(end))
                    : FilterFactory.Between(node[1].GetValue<string>(), Convert.ToInt64(start), null);
                return betweenFilter;
            }
            else
            {
                return FilterFactory.Between(
                    node[1].GetValue<string>(),
                    start == null ? null : (string)start,
                    end == null ? null : (string)end
                );
            }
        }

        private Filter<string> CreateBetweenNumbersFilter(JsonArray node)
        {
            long? start;
            if (node[2] == null)
            {
                start = null;
            }
            else
            {
                switch (node[2].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.Number:
                        start = node[2].GetValue<long?>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected start value type in 'between numbers' filter : " + node[2].GetValue<JsonElement>().ValueKind);
                }
            }

            long? end;
            if (node[3] == null)
            {
                end = null;
            }
            else
            {
                switch (node[3].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.Number:
                        end = node[3].GetValue<long?>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected end value type in 'between numbers' filter : " + node[3].GetValue<JsonElement>().ValueKind);
                }
            }
            return FilterFactory.Between(node[1].GetValue<string>(), start, end);
        }

        private Filter<string> CreateBetweenstringsFilter(JsonArray node)
        {
            string start;
            if (node[2] == null)
            {
                start = null;
            }
            else
            {
                switch (node[2].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.String:
                        start = node[2].GetValue<string>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected start value type in 'between strings' filter : " + node[2].GetValue<JsonElement>().ValueKind);
                }
            }
            string end;
            if (node[3] == null)
            {
                end = null;
            }
            else
            {
                switch (node[3].GetValue<JsonElement>().ValueKind)
                {
                    case JsonValueKind.String:
                        end = node[3].GetValue<string>();
                        break;
                    default:
                        throw new ArgumentException("Unexpected end value type in 'between strings' filter : " + node[3].GetValue<JsonElement>().ValueKind);
                }
            }
            return FilterFactory.Between(node[1].GetValue<string>(), start, end);
        }

        private Filter<string> CreateLessThanFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.Number:
                    return FilterFactory.LessThan(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.LessThan(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'less than' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateLessThanOrEqualsFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.Number:
                    return FilterFactory.LessThanOrEquals(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.LessThanOrEquals(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'less than or equals' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateGreaterThanFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.Number:
                    return FilterFactory.GreaterThan(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.GreaterThan(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'greater than' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateGreaterThanOrEqualsFilter(JsonArray node)
        {
            switch (node[2].GetValue<JsonElement>().ValueKind)
            {
                case JsonValueKind.Number:
                    return FilterFactory.GreaterThanOrEquals(node[1].GetValue<string>(), node[2].GetValue<long>());
                case JsonValueKind.String:
                    return FilterFactory.GreaterThanOrEquals(node[1].GetValue<string>(), node[2].GetValue<string>());
                default:
                    throw new ArgumentException("Unexpected value type in 'greater than or equals' filter: " + node[2].GetValue<JsonElement>().ValueKind);
            }
        }

        private Filter<string> CreateExistsFilter(JsonArray node)
        {
            return FilterFactory.Exists(node[1].GetValue<string>());
        }

        private Filter<string> CreateEmptyFilter(JsonArray node)
        {
            return FilterFactory.Empty(node[1].GetValue<string>());
        }

        private Filter<string> CreateOrFilter(JsonArray node)
        {
            return FilterFactory.Or(GetClauses(node));
        }

        private Filter<string> CreateAndFilter(JsonArray node)
        {
            return FilterFactory.And(GetClauses(node));
        }

        private List<Filter<string>> GetClauses(JsonArray node)
        {
            IEnumerator<JsonNode> iter = node.GetEnumerator();
            iter.MoveNext(); // move from first element which is the filter type
            var clauses = new List<Filter<string>>();
            while (iter.MoveNext())
            {
                clauses.Add(CreateFilter(iter.Current.AsArray()));
            }
            return clauses;
        }

        private Filter<string> CreateNotFilter(JsonArray node)
        {
            Filter<string> notClause = CreateFilter((JsonArray)node[1]);
            return FilterFactory.Not(notClause);
        }

        private Filter<string> CreateFullTextFilter(JsonArray node)
        {
            FullTextFilter fullTextFilter = CreateFullTextClause((JsonArray)node[1]);
            return FilterFactory.FullText<string>(fullTextFilter);
        }

        private FullTextFilter CreateFullTextClause(JsonArray node)
        {
            switch (node[0].GetValue<string>())
            {
                case "near":
                    return FullTextFilterFactory.Near(
                        GetLikeTokens(node[2].AsArray().GetEnumerator()),
                        GetLikeTokens(node[3].AsArray().GetEnumerator()),
                        node[1].GetValue<int>());
                case "dnear":
                    return FullTextFilterFactory.Dnear(
                        GetLikeTokens(node[2].AsArray().GetEnumerator()),
                        GetLikeTokens(node[3].AsArray().GetEnumerator()),
                        node[1].GetValue<int>());
                case "full-text":
                    IEnumerator<JsonNode> likeTokensIter = node.GetEnumerator();
                    likeTokensIter.MoveNext();
                    return FullTextFilterFactory.FullText(GetLikeTokens(likeTokensIter));
                case "and":
                    return CreateFulltextAndClause(node);
                case "or":
                    return CreateFulltextOrClause(node);
                case "not":
                    return CreateFulltextNotClause(node);
                default:
                    throw new ArgumentException("Unexpected value type in 'fulltext' clause: " + node[0].GetValue<string>());
            }
        }

        private LikeToken[] GetLikeTokens(IEnumerator<JsonNode> likeTokensIterator)
        {
            var likeTokens = new List<LikeToken>();
            while (likeTokensIterator.MoveNext())
            {
                likeTokens.Add(CreateLikeToken(likeTokensIterator.Current.AsArray()));
            }
            return likeTokens.ToArray<LikeToken>();
        }

        private FullTextFilter CreateFulltextOrClause(JsonArray node)
        {
            return FullTextFilterFactory.Or(GetFulltextClauses(node));
        }

        private FullTextFilter CreateFulltextAndClause(JsonArray node)
        {
            return FullTextFilterFactory.And(GetFulltextClauses(node));
        }

        private List<FullTextFilter> GetFulltextClauses(JsonArray node)
        {
            node.RemoveAt(0);// move from first element which is the filter type
            IEnumerator<JsonNode> iter = node.GetEnumerator();
            var clauses = new List<FullTextFilter>();
            while (iter.MoveNext())
            {
                clauses.Add(CreateFullTextClause(iter.Current.AsArray()));
            }
            return clauses;
        }

        private FullTextFilter CreateFulltextNotClause(JsonArray node)
        {
            FullTextFilter notClause = CreateFullTextClause((JsonArray)node[1]);
            return FullTextFilterFactory.Not(notClause);
        }

        private Filter<string> CreateFieldFullTextFilter(JsonArray node)
        {
            var fields = new List<string>();
            IEnumerator<JsonNode> iter = node[1].AsArray().GetEnumerator();
            while (iter.MoveNext())
            {
                fields.Add(iter.Current.GetValue<string>());
            }
            FullTextFilter fullTextFilter = CreateFullTextClause((JsonArray)node[2]);
            return FilterFactory.FieldFullText(fields, fullTextFilter);
        }

        private LikeToken CreateLikeToken(JsonArray node)
        {
            switch (node[0].GetValue<string>())
            {
                case "'":
                    return LikeTokenFactory.StringLiteral(node[1].GetValue<string>());
                case "?":
                    return LikeTokenFactory.SingleCharacterWildcard();
                case "*":
                    return LikeTokenFactory.ZeroOrMoreCharactersWildcard();
                default:
                    throw new ArgumentException("Unexpected value type in 'like token': " + node[0].GetValue<string>());
            }
        }
    }
}
