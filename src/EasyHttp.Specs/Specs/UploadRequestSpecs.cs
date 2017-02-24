using System.Collections.Generic;
using System.IO;
using System.Net;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(TestOf = typeof(HttpClient))]
    public class UploadRequestSpecs
    {
        [Test]
        public void when_sending_binary_data_as_put_it_should_upload_it_succesfully()
        {
            var httpClient = new HttpClient();

            var imageFile = Path.Combine(TestRunContext.WorkingDirectory.FullName, "Helpers", "test.jpg");

            httpClient.PutFile(string.Format("{0}/fileupload/test.jpg", "http://localhost:16000"),
                imageFile,
                "image/jpeg");
            Assert.AreEqual(HttpStatusCode.Created, httpClient.Response.StatusCode);
        }

        [Test] public void when_sending_binary_data_as_multipart_post_it_should_upload_it_succesfully()
        {
            var httpClient = new HttpClient();

            var imageFile = Path.Combine(TestRunContext.WorkingDirectory.FullName, "Helpers", "test.jpg");

            IDictionary<string, object> data = new Dictionary<string, object>();

            data.Add("email", "hadi@hadi.com");
            data.Add("name", "hadi");

            IList<FileData> files = new List<FileData>();

            files.Add(new FileData() {FieldName = "image1", ContentType = "image/jpeg", Filename = imageFile});
            files.Add(new FileData() {FieldName = "image2", ContentType = "image/jpeg", Filename = imageFile});
            httpClient.Post(string.Format("{0}/fileupload", "http://localhost:16000"), data, files);

            Assert.AreEqual(HttpStatusCode.OK, httpClient.Response.StatusCode);
        }
    }
}