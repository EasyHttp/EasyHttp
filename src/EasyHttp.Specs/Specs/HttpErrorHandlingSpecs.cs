using System;
using System.Net;
using EasyHttp.Http;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(TestOf = typeof (HttpClient))]
    public class HttpErrorHandlingSpecs
    {
        [Test, Ignore("This site is down. Need to find alternative")]
        public void when_making_a_request_that_contains_a_response_with_error_information()
        {
            var client = new HttpClient("https://cloudapi.ritterim.com");
            client.Request.AddExtraHeader("X-Requested-With", "XMLHttpRequest");
            client.Request.AddExtraHeader("Token-Authorization", "badpassword");
            client.Request.Accept = HttpContentTypes.ApplicationJson;
            client.Request.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds);

            var response = client.Post("Users", new
            {
                Password = "foo",
                PasswordExpiration = DateTime.MinValue,
                Email = "new@mail.com",
                FirstName = "test",
                LastName = "fail",
                Information = new { IpAddress = "1.2.3.4" }
            }, HttpContentTypes.ApplicationJson);

            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.NotNull(response.StatusDescription);
            Assert.Greater(0, response.RawHeaders.Keys.Count);
            Assert.AreEqual("3.0", response.RawHeaders["X-AspNetMvc-Version"]);
            Assert.AreEqual("NoCache", response.CacheControl.ToString());
            Assert.AreEqual("Microsoft-IIS/8.0", response.Server);
        }
    }
}
