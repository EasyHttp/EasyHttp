using System.Net;
using EasyHttp.Http;
using EasyHttp.Http.Abstractions;
using EasyHttp.Specs.Helpers;
using NUnit.Framework;

namespace EasyHttp.Specs.BugRepros
{
    [TestFixture(Category = "Accept-Encoding")]
    public class when_preparing_a_web_request
    {
        private HttpWebRequest underlyingWebRequest;

        [SetUp]
        public void BecauseOf()
        {
            var http = new HttpClient()
            {
                Request = {Uri = "http://github.com/"} // a Uri is required by the PrepareRequest() method
            };

            underlyingWebRequest = (http.Request.PrepareRequest() as HttpWebRequestWrapper).InnerRequest;
        }

        [Test]
        public void should_enable_automatic_gzip_compression_on_the_underlying_web_request_by_default()
        {
            underlyingWebRequest.AutomaticDecompression.ShouldHaveFlag(DecompressionMethods.GZip);
        }

        [Test]
        public void should_enable_automatic_deflate_compression_on_the_underlying_web_request_by_default()
        {
            underlyingWebRequest.AutomaticDecompression.ShouldHaveFlag(DecompressionMethods.Deflate);
        }
    }

    [TestFixture(Category = "Accept-Encoding")]
    public class when_disabling_automatic_compression
    {
        private HttpWebRequest underlyingWebRequest;

        [SetUp]
        public void BecauseOf()
        {
            var http = new HttpClient()
            {
                Request =
                {
                    Uri = "http://github.com/", // a Uri is required by the PrepareRequest() method
                    DisableAutomaticCompression = true
                }
            };

            underlyingWebRequest = (http.Request.PrepareRequest() as HttpWebRequestWrapper).InnerRequest;
        }

        [Test]
        public void should_disable_automatic_gzip_compression_on_the_underlying_web_request()
        {
            underlyingWebRequest.AutomaticDecompression.ShouldNotHaveFlag(DecompressionMethods.GZip);
        }

        [Test]
        public void should_disable_automatic_deflate_compression_on_the_underlying_web_request()
        {
            underlyingWebRequest.AutomaticDecompression.ShouldNotHaveFlag(DecompressionMethods.Deflate);
        }
    }
}