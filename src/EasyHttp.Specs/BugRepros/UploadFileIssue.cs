using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.BugRepros
{
    public class UploadFileIssues
    {
        [Test]
        public void file_upload_was_failing_because_fieldname_for_file_field_was_not_being_set()
        {
            var httpClient = new HttpClient();

            var filename = Path.Combine(TestRunContext.WorkingDirectory.FullName, "Helpers", "test.xml");

            IDictionary<string, object> data = new Dictionary<string, object>();

            data.Add("runTest", " Run Test ");

            IList<MultiPartFileDataAbstraction> files = new List<MultiPartFileDataAbstraction>();

            files.Add(new FileData() {FieldName = "file", ContentType = "text/xml", Filename = filename});

            httpClient.Post("https://loandelivery.intgfanniemae.com/mismoxtt/mismoValidator.do", data, files);

            StringAssert.DoesNotContain("Please select a file to test.", httpClient.Response.RawText);
        }
    }
}