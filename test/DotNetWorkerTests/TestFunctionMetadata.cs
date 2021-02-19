// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Azure.Functions.Worker.Tests
{
    internal class TestFunctionMetadata : FunctionMetadata
    {
        private IEnumerable<string> _bindingNames = Enumerable.Empty<string>();

        public override string PathToAssembly { get; set; }

        public override string EntryPoint { get; set; }

        public override string FunctionId { get; set; }

        public override string Name { get; set; }

        public override IEnumerable<string> BindingNames
        {
            get
            {
                return _bindingNames;
            }
            set
            {
                _bindingNames = value;
            }
        }
    }
}
