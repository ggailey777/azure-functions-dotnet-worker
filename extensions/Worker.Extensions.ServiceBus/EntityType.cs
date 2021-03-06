// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Azure.Functions.Worker.Extensions.ServiceBus
{
    /// <summary>
    /// Service Bus entity type.
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// Service Bus Queue
        /// </summary>
        Queue,

        /// <summary>
        /// Service Bus Topic
        /// </summary>
        Topic
    }
}
