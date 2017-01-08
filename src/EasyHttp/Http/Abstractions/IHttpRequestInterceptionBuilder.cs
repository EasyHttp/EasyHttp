using System.Collections.Generic;
using System.Net;

namespace EasyHttp.Http.Abstractions
{
    public interface IHttpRequestInterceptionBuilder
    {
        /// <summary>
        /// Inject status code, a single Content-Type header, and a string response body.
        /// </summary>
        void InjectResponse(HttpStatusCode injectedResponseCode, string contentType, string injectedResponseBody);

        /// <summary>
        /// Inject status code, a dictionary of headers, and a string response body.
        /// </summary>
        void InjectResponse(HttpStatusCode injectedResponseCode, Dictionary<HttpResponseHeader, string> injectedResponseHeaders, string injectedResponseBody);

        /// <summary>
        /// Inject a fully mocked <see cref="IHttpWebResponse"/>.
        /// </summary>
        void InjectResponse(IHttpWebResponse response);
    }
}