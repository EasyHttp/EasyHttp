using System;

namespace EasyHttp.Http.Abstractions
{
    /// <summary>
    /// Used by <see cref="HttpClient"/> to match against a <see cref="HttpRequest"/>
    /// in the <code>HttpClient.ProcessRequst(...)</code>.
    /// </summary>
    public interface IHttpRequestInterception
    {
        /// <summary>
        /// Used to identify when this interceptor should
        /// override a particular <see cref="HttpRequest"/>
        /// </summary>
        /// <remarks>
        /// Returns <code>true</code> when this iterceptor
        /// should replace the given <see cref="HttpRequest"/>.
        /// </remarks>
        Func<HttpRequest, bool> Matches { get; }

        /// <summary>
        /// Builds the <see cref="IHttpWebResponse"/> to insert
        /// into the <see cref="HttpResponse"/> for decoding.
        /// </summary>
        /// <returns>
        /// A new <see cref="IHttpWebResponse"/> customized by calls
        /// to the <see cref="IHttpRequestInterceptionBuilder"/>
        /// interface.
        /// </returns>
        IHttpWebResponse GetInjectedResponse();
    }
}