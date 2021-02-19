// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Net;

namespace Microsoft.Azure.Functions.Worker.Http
{
    /// <summary>
    /// A representation of the outgoing HTTP response.
    /// </summary>
    public abstract class HttpResponseData
    {
        /// <summary>
        /// Gets or sets the status code for the response.
        /// </summary>
        public abstract HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="HttpHeadersCollection"/> containing the response headers
        /// </summary>
        public abstract HttpHeadersCollection Headers { get; set; }

        /// <summary>
        /// Gets or sets the response body stream.
        /// </summary>
        public abstract Stream Body { get; set; }

        /// <summary>
        /// Gets an <see cref="HttpCookies"/> instance containing the request cookies.
        /// </summary>
        public abstract HttpCookies Cookies { get; }
    }
}
