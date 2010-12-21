using System;
using System.Collections.Generic;
using EasyHttp.Http;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject("HttpClient Init")]
    public class when_creating_a_new_instance
    {
        Because of = () =>
        {
            httpClient = new HttpClient(); 
        };

        It should_return_new_instance_using_default_configuration = () => httpClient.ShouldNotBeNull();

        static HttpClient httpClient;
    }

}