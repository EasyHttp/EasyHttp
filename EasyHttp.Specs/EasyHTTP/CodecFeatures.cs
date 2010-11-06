using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("Codec")]
    public class when_excluding_properties_from_serialization_on_post
    {
        Establish context = () =>
        {
            http = new HttpClient();

            var customer = new Customer {Name = "test", Email = "some@email.com", Phone = "12346", Fax = "5678"};

            http.Put(String.Format(" {0}/{1}", TestSettings.CouchDbDatabaseUrl, "codec_exclude_post_customer"), customer, "application/json");
        };

        Because of = () =>
        {
            var response = http.Get(String.Format("{0}/{1}", TestSettings.CouchDbRootUrl, "codec_exclude_post_customer"));

            var customer = response.StaticBody<CustomCodecClass>();

        };

        It should_not_serialize_them_as_part_of_request = () =>
        {
            
        };

        static HttpClient http;
    }
}
