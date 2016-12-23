using System.Net;
using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    [Subject("Accept-Encoding")]
    public class when_preparing_a_web_request
    {
        Establish context = () =>
        {
            http = new HttpClient()
            {
                Request = { Uri = "http://github.com/" } // a Uri is required by the PrepareRequest() method
            };
        };

        Because of = () =>
            {
                underlyingRequest = http.Request.PrepareRequest();
            };

        It should_enable_automatic_gzip_compression_on_the_underlying_web_request_by_default = () =>
            {
                underlyingRequest.AutomaticDecompression.ShouldHaveFlag(DecompressionMethods.GZip);
            };

        It should_enable_automatic_deflate_compression_on_the_underlying_web_request_by_default = () =>
            {
                underlyingRequest.AutomaticDecompression.ShouldHaveFlag(DecompressionMethods.Deflate);
            };
        static HttpClient http;
        private static HttpWebRequest underlyingRequest;
    }

    [Subject("Accept-Encoding")]
    public class when_disabling_automatic_compression
    {
        Establish context = () =>
        {
            http = new HttpClient()
            {
                Request = {
                    Uri = "http://github.com/" , // a Uri is required by the PrepareRequest() method
                    DisableAutomaticCompression = true
                }
            };
        };

        Because of = () =>
            {
                underlyingRequest = http.Request.PrepareRequest();
            };

        It should_disable_automatic_gzip_compression_on_the_underlying_web_request = () =>
            {
                underlyingRequest.AutomaticDecompression.ShouldNotHaveFlag(DecompressionMethods.GZip);
            };

        It should_disable_automatic_deflate_compression_on_the_underlying_web_request = () =>
            {
                underlyingRequest.AutomaticDecompression.ShouldNotHaveFlag(DecompressionMethods.Deflate);
            };
        static HttpClient http;
        private static HttpWebRequest underlyingRequest;
    }
}