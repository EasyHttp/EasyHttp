using System;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Working with PUT")]
    public class when_putting_a_request_in_json_format
    {
        Establish context = () =>
        {
            _easyHttp = new EasyHttp()
                .WithAccept("application/json")
                .WithContentType("application/json");
        };

        Because of = () =>
        {
            Guid guid = Guid.NewGuid();
            _easyHttp.Put(string.Format("{0}/{1}", "http://127.0.0.1:5984/customers", guid),
                          new Customer() {Name = "Put", Email = "test@test.com"});

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