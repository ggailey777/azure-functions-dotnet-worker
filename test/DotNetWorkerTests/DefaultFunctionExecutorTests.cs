// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker.Invocation;
using Xunit;

namespace Microsoft.Azure.Functions.Worker.Tests
{
    public class DefaultFunctionExecutorTests
    {
        [Fact]
        public void AddsStorageOutput_AndHttpReturn_FromReturnType()
        {
            FunctionContext context = GetContextWithBindingNames(nameof(HttpAndStorage.MyQueueOutput), nameof(HttpAndStorage.MyBlobOutput));
            var emptyHttp = new HttpResponseData(HttpStatusCode.OK);

            HttpAndStorage result = new HttpAndStorage()
            {
                MyQueueOutput = "queueStuff",
                MyBlobOutput = "blobStuff",
                MyRandomValue = "ShouldNotAppear",
                MyHttpResponseData = emptyHttp
            };

            Assert.Equal(0, context.OutputBindings.Count);
            var returnedVal = DefaultFunctionExecutor.AddOutputBindingsAndGetReturn(context, result);

            Assert.Same(emptyHttp, returnedVal);
            Assert.Equal(2, context.OutputBindings.Count);

            AssertDictionary(context.OutputBindings, new Dictionary<string, object>()
            {
                { "MyQueueOutput", "queueStuff" },
                { "MyBlobOutput", "blobStuff" }
            });
        }

        [Fact]
        public void AddsStorageOutput_FromReturnType()
        {
            FunctionContext context = GetContextWithBindingNames(nameof(JustStorage.MyQueueOutput), nameof(JustStorage.MyBlobOutput));

            JustStorage result = new JustStorage()
            {
                MyQueueOutput = "queueStuff",
                MyBlobOutput = "blobStuff"
            };

            Assert.Equal(0, context.OutputBindings.Count);
            var returnedVal = DefaultFunctionExecutor.AddOutputBindingsAndGetReturn(context, result);

            Assert.Null(returnedVal);
            Assert.Equal(2, context.OutputBindings.Count);

            AssertDictionary(context.OutputBindings, new Dictionary<string, object>()
            {
                { "MyQueueOutput", "queueStuff" },
                { "MyBlobOutput", "blobStuff" }
            });
        }

        [Fact]
        public void AddsSingleOutput_FromMethodReturn()
        {
            // Special binding name indiciating return
            FunctionContext context = GetContextWithBindingNames("FunctionsWorkerReturnReserved");
            string result = "MyStorageData";

            Assert.Equal(0, context.OutputBindings.Count);
            var returnedVal = DefaultFunctionExecutor.AddOutputBindingsAndGetReturn(context, result);

            Assert.Null(returnedVal);
            Assert.Single(context.OutputBindings);

            AssertDictionary(context.OutputBindings, new Dictionary<string, object>()
            {
                { "FunctionsWorkerReturnReserved", "MyStorageData" }
            });
        }

        [Fact]
        public void JustHttp_FromReturnType()
        {
            // if a binding was declared
            FunctionContext context = GetContextWithBindingNames("randomBinding");
            var emptyHttp = new HttpResponseData(HttpStatusCode.OK);

            JustHttp result = new JustHttp()
            {
                MyHttpResponseData = emptyHttp
            };

            Assert.Equal(0, context.OutputBindings.Count);
            var returnedVal = DefaultFunctionExecutor.AddOutputBindingsAndGetReturn(context, result);

            Assert.Same(emptyHttp, returnedVal);
            Assert.Equal(0, context.OutputBindings.Count);

            // if no bindings were declared
            context = GetContextWithBindingNames();
            emptyHttp = new HttpResponseData(HttpStatusCode.OK);

            result = new JustHttp()
            {
                MyHttpResponseData = emptyHttp
            };

            Assert.Equal(0, context.OutputBindings.Count);
            returnedVal = DefaultFunctionExecutor.AddOutputBindingsAndGetReturn(context, result);

            Assert.Same(emptyHttp, returnedVal);
            Assert.Equal(0, context.OutputBindings.Count);
        }

        private static void AssertDictionary<K, V>(IDictionary<K, V> dict, IDictionary<K, V> expected)
        {
            Assert.Equal(expected.Count, dict.Count);

            foreach (var kvp in expected)
            {
                Assert.Equal(kvp.Value, dict[kvp.Key]);
            }
        }

        private FunctionContext GetContextWithBindingNames(params string[] bindingNames)
        {
            var metadata = new TestFunctionMetadata()
            {
                BindingNames = new List<string>(bindingNames)
            };
            var defintion = new TestFunctionDefinition()
            {
                Metadata = metadata
            };
            var context = new TestFunctionContext()
            {
                FunctionDefinition = defintion
            };

            return context;
        }

        public class JustStorage
        {
            public object MyQueueOutput { get; set; }

            public object MyBlobOutput { get; set; }
        }

        public class HttpAndStorage
        {
            public object MyQueueOutput { get; set; }

            public object MyBlobOutput { get; set; }

            public object MyRandomValue { get; set; }

            public HttpResponseData MyHttpResponseData { get; set; }
        }

        public class JustHttp
        {
            public HttpResponseData MyHttpResponseData { get; set; }
        }
    }
}
