using System;
using System.Net;
using EasyHttp.Http;
using EasyHttp.Specs.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    public class HttpRequestTests
    {
        [Test, Category("HttpClient")]
        public void when_making_any_type_of_request_to_invalid_host_it_should_throw_WebException()
        {
            var httpClient = new HttpClient();

            Assert.Throws<WebException>(() => httpClient.Get("http://somethinginvalid"));
        }

        [Test, Category("HttpClient")]
        public void when_making_a_DELETE_request_with_a_valid_uri_it_should_delete_the_existing_resource()
        {
            var httpClient = new HttpClient
            {
                Request = { Accept = HttpContentTypes.ApplicationJson }
            };

            // First create customer in order to then delete it
            httpClient.Put(string.Format("{0}/hello/", "http://localhost:16000"),
                new Customer() { Name = "ToDelete" }, HttpContentTypes.ApplicationJson);

            var response = httpClient.Delete(String.Format("{0}/hello", "http://localhost:16000"));

            Assert.IsNotEmpty(response.DynamicBody.Result);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_GET_request_with_valid_uri_it_should_return_a_body_with_raw_text()
        {
            var httpClient = new HttpClient();

            var httpResponse = httpClient.Get("http://localhost:16000");

            Assert.IsNotEmpty(httpResponse.RawText);
        }

        [Test, Category("HttpClient")]
        public void when_mocking_a_GET_request_with_valid_uri_to_return_a_NotFound_it_should_return_a_NotFound()
        {
            var injectedResponse = Substitute.For<HttpResponse>();
            injectedResponse.StatusCode.Returns(HttpStatusCode.NotFound);

            var httpClient = Substitute.For<HttpClient>();
            httpClient.Get(Arg.Any<string>()).Returns(injectedResponse);

            var httpResponse = httpClient.Get("http://localhost:16000");

            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }

        public class when_mocking_a_GET_request_with_valid_uri_to_inject_a_specific_response
        {
            HttpResponse _httpGetResponse;
            HttpResponse _httpPostResponse;

            [SetUp]
            public void SetUp()
            {
                var injectedResponseBody =
                    @"{
    'name': 'Serenity',
    'characterNames': [
        'Mal',
        'Wash',
        'Zoe'
    ]
}";
                var httpClient = new HttpClient();

                httpClient.OnRequest(r =>
                            r.Uri.Contains("localhost:16000")
                            && r.Method == HttpMethod.POST
                    )
                    .InjectResponse(
                        HttpStatusCode.BadRequest,
                        HttpContentTypes.ApplicationJson,
                        injectedResponseBody
                    );

                _httpGetResponse = httpClient.Get("http://localhost:16000");
                _httpPostResponse = httpClient.Post("http://localhost:16000", null, HttpContentTypes.ApplicationJson);
            }

            [Test, Category("HttpClient.Interception")]
            public void should_return_OK_for_the_Get()
            {
                Assert.AreEqual(HttpStatusCode.OK, _httpGetResponse.StatusCode);
            }

            [Test, Category("HttpClient.Interception")]
            public void should_return_BadRequest_for_the_Post()
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, _httpPostResponse.StatusCode);
            }

            [Test, Category("HttpClient.Interception")]
            public void should_return_the_injected_content_type_for_the_Post()
            {
                Assert.AreEqual(HttpContentTypes.ApplicationJson, _httpPostResponse.ContentType);
            }

            [Test, Category("HttpClient.Interception")]
            public void should_return_the_injected_body_content_for_the_Post()
            {
                var characterNames = _httpPostResponse.DynamicBody.characterNames;

                Assert.NotNull(characterNames);
                Assert.AreEqual(3, characterNames.Length);
                CollectionAssert.AreEqual(new[] { "Mal", "Wash", "Zoe" }, characterNames);
            }
        }

        [Test, Category("HttpClient")]
        public void
            when_making_a_GET_request_with_valid_uri_and_querystring_and_valid_parameters_it_should_return_dynamic_body_with_json_object
            ()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var response = httpClient.Get("http://localhost:16000/hello", new { Name = "true" });

            dynamic body = response.DynamicBody;
            string couchdb = body.Result;
            Assert.AreEqual("Hello, true", couchdb);
        }

        [Test, Category("HttpClient")]
        public void
            when_making_a_GET_request_with_valid_uri_and_querystring_and_valid_parameters_using_segments_it_should_return_dynamic_body_with_json_object
            ()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            httpClient.Request.ParametersAsSegments = true;

            var response = httpClient.Get("http://localhost:16000/hello", new { Name = "true" });
            dynamic body = response.DynamicBody;
            string couchdb = body.Result;
            Assert.AreEqual("Hello, true", couchdb);
        }

        [Test, Category("HttpClient")]
        public void
            when_making_a_GET_request_response_contains_at_sign_and_remove_at_sign_is_false_then_at_sign_remains_it_should_return_static_body_with_json_object_with_at_sign
            ()
        {
            var httpClient = new HttpClient()
            {
                ShouldRemoveAtSign = false,
                Request =
                {
                    Accept = HttpContentTypes.ApplicationJson
                }
            };

            var response = httpClient.Get("http://localhost:16000/data", new { Id = "at sign" });

            SomeDataResponse couchInformation = response.StaticBody<SomeDataResponse>();

            Assert.AreEqual(@"@bormod how are you?", couchInformation.SomeValue);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_GET_request_with_response_contains_at_sign_backward_compatability_it_should_return_static_body_with_json_object_without_at_sign()
        {
            var httpClient = new HttpClient
            {
                Request =
                {
                    Accept = HttpContentTypes.ApplicationJson
                }
            };

            var response = httpClient.Get("http://localhost:16000/data", new { Id = "at sign" });

            SomeDataResponse couchInformation = response.StaticBody<SomeDataResponse>();

            Assert.AreEqual(@"bormod how are you?", couchInformation.SomeValue);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_json_it_should_return_dynamic_body_with_json_object()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var response = httpClient.Get("http://localhost:16000/hello");

            dynamic body = response.DynamicBody;

            string result = body.Result;

            Assert.AreEqual("Hello, ", result);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_json_it_should_return_static_body_with_json_object()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var response = httpClient.Get("http://localhost:16000/hello");

            var couchInformation = response.StaticBody<ResultResponse>();

            Assert.AreEqual("Hello, ", couchInformation.Result);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_HEAD_request_with_valid_uri_it_should_return_OK_response()
        {
            var httpClient = new HttpClient();

            var response = httpClient.Head("http://localhost:16000");

            Assert.AreEqual("OK", response.StatusDescription);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_POST_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json_it_should_succeed()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var response = httpClient.Post("http://localhost:16000/hello", new Customer() { Name = "Hadi" },
                HttpContentTypes.ApplicationJson);

            string id = response.DynamicBody.Result;

            Assert.IsNotEmpty(id);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_POST_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json_and_parameters_as_segments_it_should_succeed()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            httpClient.Request.ParametersAsSegments = true;

            var response = httpClient.Post("http://localhost:16000/hello", new Customer() { Name = "Hadi" },
                HttpContentTypes.ApplicationJson);

            string id = response.DynamicBody.Result;

            Assert.IsNotEmpty(id);
        }

        [Test, Category("HttpClient")]
        public void when_making_a_PUT_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json_it_should_succeed()
        {
            var httpClient = new HttpClient();
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            var response = httpClient.Put(string.Format("{0}/{1}", "http://localhost:16000", "hello"),
                new Customer() { Name = "Put" }, HttpContentTypes.ApplicationJson);

            string result = response.DynamicBody.Result;

            Assert.IsNotEmpty(result);
        }

        [Test, Category("HttpClient")]
        public void when_making_requests_and_persisting_cookies_it_should_send_returned_cookies()
        {
            var httpClient = new HttpClient();
            httpClient.Request.PersistCookies = true;
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            httpClient.Put("http://localhost:16000/cookie", new CookieInfo { Name = "test", Value = "test cookie" },
                HttpContentTypes.ApplicationJson);
            var response = httpClient.Get("http://localhost:16000/cookie/test");

            string cookieValue = response.DynamicBody.Value;

            Assert.AreEqual("test cookie", cookieValue);
        }

        [Test, Category("HttpClient")]
        public void when_making_requests_and_not_persisting_cookies_it_should_not_send_returned_cookies()
        {
            var httpClient = new HttpClient();
            httpClient.Request.PersistCookies = false;
            httpClient.Request.Accept = HttpContentTypes.ApplicationJson;

            httpClient.Put("http://localhost:16000/cookie", new CookieInfo { Name = "test", Value = "test cookie" },
                HttpContentTypes.ApplicationJson);
            var response = httpClient.Get("http://localhost:16000/cookie/test");

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}