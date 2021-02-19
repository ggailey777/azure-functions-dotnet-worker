﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Text;
using System.Text.Unicode;

namespace Microsoft.Azure.Functions.Worker.Http
{
    public static class HttpResponseDataExtensions
    {
        /// <summary>
        /// Writes the provided string to the response body using UTF-8.
        /// </summary>
        /// <param name="response">The response to write the string to.</param>
        /// <param name="value">The string content to write to the request body.</param>
        public static void WriteString(this HttpResponseData response, string value)
        {
            WriteString(response, value, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the provided string to the response body using the specified encoding.
        /// </summary>
        /// <param name="response">The response to write the string to.</param>
        /// <param name="value">The string content to write to the request body.</param>
        /// <param name="encoding">The encoding to use when writing the string.</param>
        public static void WriteString(this HttpResponseData response, string value, Encoding encoding)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            byte[] bytes = encoding.GetBytes(value);
            response.Body.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the provided bytes to the response body.
        /// </summary>
        /// <param name="response">The response to write the string to.</param>
        /// <param name="value">The byte content to write to the request body.</param>
        public static void WriteBytes(this HttpResponseData response, byte[] value)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            response.Body.Write(value, 0, value.Length);
        }
    }
}
