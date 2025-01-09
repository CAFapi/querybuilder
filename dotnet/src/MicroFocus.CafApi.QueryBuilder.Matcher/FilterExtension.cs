/**
 * Copyright 2022-2025 Open Text.
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
using System.Collections.Generic;

namespace MicroFocus.CafApi.QueryBuilder.Matcher
{
    public static class FilterExtension
    {
        public static bool? IsMatch<Document>(
            this Filter<IMatcherFieldSpec<Document>> that,
            Document document
        )
        {
            return that.IsMatch(document, null);
        }

        public static bool? IsMatch<Document>(
            this Filter<IMatcherFieldSpec<Document>> that,
            Document document,
            IEnumerable<IMatcherFieldSpec<Document>> allFullTextFieldSpecs
        )
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            var visitor = new MatcherQueryVisitor<Document>(document, allFullTextFieldSpecs);
            that.Invoke(visitor);

            return visitor.GetResult();
        }
    }
}
