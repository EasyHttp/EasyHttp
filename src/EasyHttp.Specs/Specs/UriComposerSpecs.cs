using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    public class When_baseuri_is_null_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            uri = "uri";
        };

        Because of = () => url = uriComposer.Compose(null, uri, null);

        It should_return_the_uri = () => url.ShouldEqual("uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
    }

    public class When_baseuri_is_empty_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "";
            uri = "uri";
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, null);

        It should_return_the_uri = () => url.ShouldEqual("uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
    }

    public class When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "baseuri";
            uri = "uri";
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, null);

        It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
    }

    public class When_baseuri_is_filled_and_ends_with_a_forwardslash_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "baseuri/";
            uri = "uri";
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, null);

        It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
    }

    public class When_baseuri_is_filled_and_ends_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "baseuri/";
            uri = "/uri";
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, null);

        It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
    }

    public class When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "baseuri";
            uri = "/uri";
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, null);

        It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
    }

    public class When_baseuri_and_ur_are_filled_and_query_is_not_null
    {
        Establish context = () =>
        {
            uriComposer = new UriComposer();
            baseuri = "baseuri";
            uri = "/uri";
            query = new {Name = "test"};
        };

        Because of = () => url = uriComposer.Compose(baseuri, uri, query);

        It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri?Name=test");

        static UriComposer uriComposer;
        static string url;
        static string uri;
        static string baseuri;
        static object query;
    }
}