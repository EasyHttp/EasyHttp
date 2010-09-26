using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Working with POST")]
    public class when_posting_a_request_in_json_format
    {
        Establish context = () =>
        {
            _easyHttp = new EasyHttp();
            _easyHttp.SetContentType("application/json");
            _easyHttp.SetAccept("application/json");
        };

        Because of = () =>
        {

            _easyHttp.Post("http://127.0.0.1:5984/customers", new Customer() { Name = "Hadi", Email = "test@test.com" });
            response = _easyHttp.Response;
        };


        It should_succeed = () =>
        {
            bool ok = response.Body.ok;

            string id = response.Body.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();
        };



        static EasyHttp _easyHttp;
        static dynamic response;
    }
}