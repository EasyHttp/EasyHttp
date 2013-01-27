using System.Collections.Generic;
using System.IO;
using System.Net;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{

    public class file_upload_was_failing_because_fieldname_for_file_field_was_not_being_set
    {
        Establish context = () =>
            {
                httpClient = new HttpClient();
            };

        Because of = () =>
            {

                var filename = Path.Combine("Helpers", "test.xml"); 
                

                IDictionary<string, object> data = new Dictionary<string, object>();


                data.Add("runTest", " Run Test ");

                IList<FileData> files = new List<FileData>();

                files.Add(new FileData() { FieldName = "file", ContentType = "text/xml", Filename = filename });

                httpClient.Post("https://loandelivery.intgfanniemae.com/mismoxtt/mismoValidator.do", data, files);

                response = httpClient.Response;
            };


        It should_say_that_operation_was_successful = () => response.RawText.ShouldNotContain("Please select a file to test.");
        
        static HttpClient httpClient;
        static HttpResponse response;
    }
}