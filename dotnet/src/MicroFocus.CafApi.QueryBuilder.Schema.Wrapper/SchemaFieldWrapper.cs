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
using MicroFocus.CafApi.QueryBuilder.Matcher;
using MicroFocus.FAS.AdapterSdkSchema;
using System.Collections.Generic;

namespace MicroFocus.CafApi.QueryBuilder.Schema.Wrapper
{
    public abstract class SchemaFieldWrapper<T> : IMatcherFieldSpec<T>
    {
        private readonly IField _field;

        public SchemaFieldWrapper(IField field)
        {
            _field = field;
        }

        public abstract IEnumerable<IMatcherFieldValue> GetFieldValues(T document);

        public bool IsCaseInsensitive => _field.IsCaseInsensitive;

        public bool IsTokenizedPath => _field.IsTokenizedPath;
    }
}
