using EasyHttp.Http;
using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_one_parameter_using_anonymous_object
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(new {Name = "test"});

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test");

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_two_parameters_using_anonymous_object
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(new { Name = "test", Id=1 });

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test&Id=1");

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_it_should_encode_value
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(new { Name = "test<>&;"});

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test%3c%3e%26%3b");

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_it_should_be_empty_when_passing_null
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(null);

        It should_have_the_correct_url_parameters = () => url.ShouldBeEmpty();

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_one_parameter_using_static_object
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
            parameter = new StaticObjectWithName() {Name="test"};
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(parameter);

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test");

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
        static StaticObjectWithName parameter;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_two_parameters_using_static_object
    {
        Establish context = () =>
        {
            objectToUrlParameters = new ObjectToUrlParameters();
            parameter = new StaticObjectWithNameAndId() { Name = "test", Id=1 };
        };

        Because of = () => url = objectToUrlParameters.ParametersToUrl(parameter);

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test&Id=1");

        static ObjectToUrlParameters objectToUrlParameters;
        static string url;
        static StaticObjectWithNameAndId parameter;
    }

    public class StaticObjectWithName
    {
        public string Name { get; set; }
    }

    public class StaticObjectWithNameAndId
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}
