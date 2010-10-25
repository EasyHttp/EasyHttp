using System;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Working with DELETE")]
    public class when_deleting_a_resource
    {
        Establish context = () =>
        {
            _easyHttp = new EasyHttp()
                .WithAccept("application/json");

            // First create customer in order to then delete it
            guid = Guid.NewGuid();
            _easyHttp.Put(string.Format("{0}/{1}", "http://127.0.0.1:5984/customers", guid),
                          new Customer() {Name = "ToDelete", Email = "test@test.com"});

            response = _easyHttp.Response;

            rev = response.Body.rev;
        };

        Because of = () =>
        {

            _easyHttp.Delete(String.Format("http://127.0.0.1:5984/customers/{0}?rev={1}", guid, rev));
            response = _easyHttp.Response;
        };
        It should_delete_the_specified_resource = () =>
        {
            bool ok = response.Body.ok;

            string id = response.Body.id;

            ok.ShouldBeTrue();

            id.ShouldNotBeEmpty();

        };


        static EasyHttp _easyHttp;
        static dynamic response;
        static string rev;
        static Guid guid;
    }
}