// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace Microsoft.Azure.Functions.Worker
{
    /// <summary>
    /// A representation of the outgoing HTTP response.
    /// </summary>
    public class HttpResponseData
    {
        private Stream _body;
        private HttpHeadersCollection _headers;

        public HttpResponseData(HttpStatusCode statusCode)
            : this(statusCode, Stream.Null) { }

        public HttpResponseData(HttpStatusCode statusCode, Stream body)
        {
            _body = body ?? throw new ArgumentNullException(nameof(body));
            _headers = new HttpHeadersCollection();
            StatusCode = statusCode;
            Cookies = new List<IHttpCookie>();
        }

        /// <summary>
        /// Gets or sets the status code for the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="HttpHeadersCollection"/> containing the response headers
        /// </summary>
        public HttpHeadersCollection Headers { get => _headers; set => _headers = value ?? throw new InvalidOperationException($"{nameof(Headers)} property cannot be null"); }


        /// <summary>
        /// Gets or sets the response body stream.
        /// </summary>
        public Stream Body { get => _body; set => _body = value ?? throw new InvalidOperationException($"{nameof(Body)} property cannot be null"); }

        /// <summary>
        /// Gets an <see cref="IList{IHttpCookie}"/> containing the request cookies.
        /// </summary>
        public IList<IHttpCookie> Cookies { get; }
    }

    public static class Coodk
    {
        public static void Append(this IList<IHttpCookie> cookies, string name, string value)
        {
            cookies.Add(new Http)
        }
    }

    public 
}
