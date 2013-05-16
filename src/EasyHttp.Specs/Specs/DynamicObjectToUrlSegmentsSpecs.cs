﻿using System.Dynamic;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_expando_object
    {
        Establish context = () =>
        {
            objectToUrlSegments = new ObjectToUrlSegments();
            parameters = new ExpandoObject();
            parameters.Name = "test";
        };

        Because of = () => url = objectToUrlSegments.ParametersToUrl(parameters);

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("/test");

        static ObjectToUrlSegments objectToUrlSegments;
        private static dynamic parameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_expando_object
    {
        Establish context = () =>
        {
            objectToUrlSegments = new ObjectToUrlSegments();
            parameters = new ExpandoObject();
            parameters.Name = "test";
            parameters.Id = 1;
        };

        Because of = () => url = objectToUrlSegments.ParametersToUrl(parameters);

        It should_have_the_correct_url_parameters = () => url.ShouldEqual("/test/1");

        static ObjectToUrlSegments objectToUrlSegments;
        private static dynamic parameters;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_anonymous_object
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(new {Name = "test"});

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/test");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_anonymous_object
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(new { Name = "test", Id=1 });

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/test/1");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_it_should_encode_value
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(new { Name = "test<>&;"});

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/test%3c%3e%26%3b");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_it_should_be_empty_when_passing_null
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(null);

        It should_have_the_correct_url_segments = () => url.ShouldBeEmpty();

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_static_object
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
            parameter = new StaticObjectWithName() {Name="test"};
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/test");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
        static StaticObjectWithName parameter;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_static_object
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
            parameter = new StaticObjectWithNameAndId() { Name = "test", Id=1 };
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/test/1");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
        static StaticObjectWithNameAndId parameter;
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_static_object_in_different_order
    {
        Establish context = () =>
        {
            _objectToUrlSegments = new ObjectToUrlSegments();
            parameter = new StaticObjectWithNameAndIdInDifferentOrder() { Name = "test", Id = 1 };
        };

        Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        It should_have_the_correct_url_segments = () => url.ShouldEqual("/1/test");

        static ObjectToUrlSegments _objectToUrlSegments;
        static string url;
        static StaticObjectWithNameAndIdInDifferentOrder parameter;
    }

    public class StaticObjectWithNameAndIdInDifferentOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
