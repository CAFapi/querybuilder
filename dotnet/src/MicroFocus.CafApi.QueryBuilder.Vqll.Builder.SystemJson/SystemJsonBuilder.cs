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
using MicroFocus.CafApi.QueryBuilder.Vqll.Builders.Internal;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Builders.SystemJson
{
    public class SystemJsonBuilder : IJsonBuilder
    {
        private Utf8JsonWriter _jsonWriter;

        public SystemJsonBuilder(Utf8JsonWriter jsonWrite)
        {
            this._jsonWriter = jsonWrite;
        }

        public void WriteBoolean(bool value)
        {
            _jsonWriter.WriteBooleanValue(value);
        }

        public void WriteEndArray()
        {
            _jsonWriter.WriteEndArray();
        }

        public void WriteNull()
        {
            _jsonWriter.WriteNullValue();
        }

        public void WriteNumber(long? value)
        {
            _jsonWriter.WriteNumberValue(Convert.ToInt64(value));
        }

        public void WriteStartArray()
        {
            _jsonWriter.WriteStartArray();
        }

        public void WriteString(string text)
        {
            _jsonWriter.WriteStringValue(text);
        }
    }
}
