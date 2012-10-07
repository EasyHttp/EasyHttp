using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof (HttpClient))]
    public class when_sending_binary_data_as_put
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
        };

        Because of = () =>
        {
        
            var imageFile = Path.Combine("Helpers", "test.jpg");

            httpClient.PutFile(string.Format("{0}/fileupload/test.jpg", "http://localhost:16000"),
                                               imageFile,
                                               "image/jpeg");

            
        };

        It should_upload_it_succesfully = () =>
        {
            httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.Created);
        };

        static HttpClient httpClient;
        static Guid guid;
        static HttpResponse response;
        static string rev;
    }

    [Subject(typeof (HttpClient))]
    public class when_sending_binary_data_as_multipart_post
    {
        Establish context = () =>
        {
            httpClient = new HttpClient();
        };

        Because of = () =>
        {
        
            var imageFile = Path.Combine("Helpers", "test.jpg");

            IDictionary<string, object> data = new Dictionary<string, object>();

            data.Add("email", "hadi@hadi.com");

            IList<FileData> files = new List<FileData>();

            files.Add(new FileData() { FieldName = imageFile, ContentType = "image/jpeg", Filename = imageFile});
            httpClient.Post(string.Format("{0}/fileupload", "http://localhost:16000"), data, files);
            
        };

        It should_upload_it_succesfully = () =>
        {
            httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        };

        static HttpClient httpClient;
        static Guid guid;
        static HttpResponse response;
        static string rev;
    }

}