// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Azure.WebJobs.Script.Grpc.Messages;

namespace Microsoft.Azure.Functions.Worker.Definition
{
    internal class GrpcFunctionMetadata : FunctionMetadata
    {
        public GrpcFunctionMetadata(FunctionLoadRequest loadRequest)
        {
            EntryPoint = loadRequest.Metadata.EntryPoint;
            Name = loadRequest.Metadata.Name;
            PathToAssembly = Path.GetFullPath(loadRequest.Metadata.ScriptFile);
            FunctionId = loadRequest.FunctionId;
            BindingNames = loadRequest.Metadata.Bindings.Keys;
        }

        public override string PathToAssembly { get; set; }

        public override string EntryPoint { get; set; }

        public override string FunctionId { get; set; }

        public override string Name { get; set; }

        public override IEnumerable<string> BindingNames { get; set; }
    }
}
