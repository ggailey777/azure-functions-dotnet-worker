// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.Functions.Worker.Invocation
{
    internal class DefaultFunctionExecutor : IFunctionExecutor
    {
        private const string FunctionReturnType = "FunctionsWorkerReturnReserved";
        private static readonly Type HttpType = typeof(HttpResponseData);

        public async Task ExecuteAsync(FunctionContext context)
        {
            var invoker = context.FunctionDefinition.Invoker;
            object? instance = invoker.CreateInstance(context.InstanceServices);
            object? result = await invoker.InvokeAsync(instance, context.FunctionDefinition.Parameters.Select(p => p.Value).ToArray());

            object? returnVal = null;
            if (result is not null)
            {
                object? returnFromType = AddOutputBindingsAndGetReturn(context, result);
                returnVal = returnFromType ?? result;
            }

            context.InvocationResult = returnVal;
        }

        internal static object? AddOutputBindingsAndGetReturn(FunctionContext context, object result)
        {
            object? returnVal = null;
            Type resultType = result.GetType();

            if (HttpType.IsAssignableFrom(resultType))
            {
                returnVal = result;
            }
            else if (HasSingleMethodReturnBinding(context))
            {
                context.OutputBindings[FunctionReturnType] = result;
            }
            else
            {
                // Look in the return type properties for output bindings
                // If an HttpResponseData was found, that needs to be the invocationResult
                returnVal = AddOutputBindingsAndGetReturn(context, result, resultType);
            }

            return returnVal;
        }

        internal static object? AddOutputBindingsAndGetReturn(FunctionContext context, object result, Type resultType)
        {
            object? returnType = null;

            foreach (var prop in resultType.GetProperties())
            {
                if (HttpType.IsAssignableFrom(prop.PropertyType))
                {
                    // HttpResponseData property always implies that
                    // it is to be used for HttpTrigger output, which is sent back to Host
                    // via the $return property
                    returnType = prop.GetValue(result);
                }

                // TODO: Should we just assume that all properties in return types are bindings?
                else if (HasBindingWithName(context, prop.Name))
                {
                    var propVal = prop.GetValue(result);
                    if (propVal is not null)
                    {
                        context.OutputBindings[prop.Name] = propVal;
                    }
                }
            }

            return returnType;
        }

        internal static bool HasBindingWithName(FunctionContext context, string name)
        {
            return context.FunctionDefinition.Metadata.BindingNames
                    .Any(b => string.Equals(name, b, StringComparison.OrdinalIgnoreCase));
        }

        internal static bool HasSingleMethodReturnBinding(FunctionContext context)
        {
            return context.FunctionDefinition.Metadata.BindingNames.Any(b => b == FunctionReturnType);
        }
    }
}
