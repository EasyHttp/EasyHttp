using EasyHttp.Http;
using NUnit.Framework;

namespace EasyHttp.Specs.BugRepros
{
    public class HeaderIssues
    {
        [Test]
        public void when_adding_extra_header_to_request()
        {
            var http = new HttpClient();

            http.Request.AddExtraHeader("X-Header", "X-Value");

            Assert.True(http.Request.RawHeaders.ContainsKey("X-Header"));
        }
    }
}