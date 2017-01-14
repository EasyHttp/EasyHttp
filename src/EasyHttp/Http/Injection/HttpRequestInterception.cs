using System;
using System.Collections.Generic;
using System.Net;
using EasyHttp.Http.Abstractions;

namespace EasyHttp.Http.Injection
{
    public static class RequestPredicateExtensions
    {
        public static bool MatchesMethod(this HttpRequest request, HttpMethod method)
        {
            return request.Method == method;
        }

        public static bool MatchesMethodAndUrl(this HttpRequest request, HttpMethod method, string url)
        {
            return request.MatchesMethod(method) && request.Uri.Equals(url, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class HttpRequestInterception : IHttpRequestInterception, IHttpRequestInterceptionBuilder
    {
        private HttpStatusCode _statusCode;
        private Dictionary<HttpResponseHeader, string> _responseHeaders;
        private string _responseBody;
        private IHttpWebResponse _response;

        /// <inheritdoc />
        public Func<HttpRequest, bool> Matches { get; private set; }

        public HttpRequestInterception(Func<HttpRequest, bool> requestPredicate)
        {
            Matches = requestPredicate ?? (x => true);
        }

        public HttpRequestInterception(HttpMethod methodToIntercept)
            : this(r => r.MatchesMethod(methodToIntercept))
        {
        }

        public HttpRequestInterception(HttpMethod methodToIntercept, string url = null)
            : this(r => r.MatchesMethodAndUrl(methodToIntercept, url))
        {
        }

        /// <inheritdoc />
        public void InjectResponse(HttpStatusCode injectedResponseCode, string contentType, string injectedResponseBody)
        {
            InjectResponse(
                injectedResponseCode,
                new Dictionary<HttpResponseHeader, string>
                {
                    {HttpResponseHeader.ContentType, contentType}
                },
                injectedResponseBody
            );
        }

        /// <inheritdoc />
        public void InjectResponse(HttpStatusCode injectedResponseCode, Dictionary<HttpResponseHeader, string> injectedResponseHeaders, string injectedResponseBody)
        {
            _statusCode = injectedResponseCode;
            _responseHeaders = injectedResponseHeaders;
            _responseBody = injectedResponseBody;
        }

        /// <inheritdoc />
        public void InjectResponse(IHttpWebResponse response)
        {
            _response = response;
        }

        /// <inheritdoc />
        public IHttpWebResponse GetInjectedResponse()
        {
            return _response ?? new StubbedHttpWebResponse(_statusCode, _responseHeaders, _responseBody);
        }
    }
}