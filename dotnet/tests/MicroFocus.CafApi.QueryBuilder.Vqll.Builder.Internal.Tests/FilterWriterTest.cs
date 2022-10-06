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

using Moq;
using Xunit;

namespace MicroFocus.CafApi.QueryBuilder.Vqll.Builders.Internal.Tests
{
    public sealed class FilterWriterTest
    {
        private Mock<IJsonBuilder> _jsonBuilder;

        public FilterWriterTest()
        {
            _jsonBuilder = new Mock<IJsonBuilder>(MockBehavior.Strict);
        }

        [Fact]
        public void TestTransformEquals()
        {
            Filter<string> clause = FilterFactory.Equals("EMBEDDED", true);

            int callOrder = 0;
            _jsonBuilder.Setup(x => x.WriteStartArray()).Callback(() => Assert.Equal(0, callOrder++));
            _jsonBuilder.Setup(x => x.WriteString("==")).Callback(() => Assert.Equal(1, callOrder++));
            _jsonBuilder.Setup(x => x.WriteString("EMBEDDED")).Callback(() => Assert.Equal(2, callOrder++));
            _jsonBuilder.Setup(x => x.WriteBoolean(true)).Callback(() => Assert.Equal(3, callOrder++));
            _jsonBuilder.Setup(x => x.WriteEndArray()).Callback(() => Assert.Equal(4, callOrder++));
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }

        [Fact]
        public void TestTransformNotEquals()
        {
            Filter<string> clause = FilterFactory.NotEquals("CLASSIFICATION", "classified");
            int callOrder = 0;
            _jsonBuilder.Setup(x => x.WriteStartArray()).Callback(() => Assert.Equal(0, callOrder++));
            _jsonBuilder.Setup(x => x.WriteString("!=")).Callback(() => Assert.Equal(1, callOrder++));
            _jsonBuilder.Setup(x => x.WriteString("CLASSIFICATION")).Callback(() => Assert.Equal(2, callOrder++));
            _jsonBuilder.Setup(x => x.WriteString("classified")).Callback(() => Assert.Equal(3, callOrder++));
            _jsonBuilder.Setup(x => x.WriteEndArray()).Callback(() => Assert.Equal(4, callOrder++));
            _jsonBuilder.VerifyNoOtherCalls();

            FilterWriter.WriteToJsonArray(clause, _jsonBuilder.Object);
        }
    }
}
