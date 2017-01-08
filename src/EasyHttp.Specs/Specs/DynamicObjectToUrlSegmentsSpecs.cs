using System.Dynamic;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(TestOf = typeof(HttpClient))]
    public class DynamicObjectToUrlSegmentSpecs
    {
        [Test]
        public void when_making_url_segments_with_one_parameter_using_expando_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlSegments = new ObjectToUrlSegments();
            dynamic parameters = new ExpandoObject();
            parameters.Name = "test";

            var url = objectToUrlSegments.ParametersToUrl(parameters);

            Assert.AreEqual("/test", url);
        }

        [Test]
        public void when_making_url_segments_with_two_parameters_using_expando_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlSegments = new ObjectToUrlSegments();
            dynamic parameters = new ExpandoObject();
            parameters.Name = "test";
            parameters.Id = 1;

            var url = objectToUrlSegments.ParametersToUrl(parameters);

            Assert.AreEqual("/test/1", url);
        }

        [Test]
        public void when_making_url_segments_with_one_parameter_using_anonymous_object_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();

            var url = _objectToUrlSegments.ParametersToUrl(new {Name = "test"});

            Assert.AreEqual("/test", url);
        }

        [Test]
        public void when_making_url_segments_with_two_parameters_using_anonymous_object_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();

            var url = _objectToUrlSegments.ParametersToUrl(new {Name = "test", Id = 1});

            Assert.AreEqual("/test/1", url);
        }

        [Test]
        public void when_making_url_segments_it_should_encode_value_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();

            var url = _objectToUrlSegments.ParametersToUrl(new {Name = "test<>&;"});

            Assert.AreEqual("/test%3c%3e%26%3b", url);
        }

        [Test]
        public void when_making_url_segments_it_should_be_empty_when_passing_null_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();

            var url = _objectToUrlSegments.ParametersToUrl(null);

            Assert.IsEmpty(url);
        }

        [Test]
        public void when_making_url_segments_with_one_parameter_using_static_object_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();
            dynamic parameter = new DynamicObjectToUrlSpecs.StaticObjectWithName() {Name = "test"};

            var url = _objectToUrlSegments.ParametersToUrl(parameter);

            Assert.AreEqual("/test", url);
        }

        [Test]
        public void when_making_url_segments_with_two_parameters_using_static_object_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();
            dynamic parameter = new DynamicObjectToUrlSpecs.StaticObjectWithNameAndId() {Name = "test", Id = 1};

            var url = _objectToUrlSegments.ParametersToUrl(parameter);

            Assert.AreEqual("/test/1", url);
        }

        [Test]
        public void when_making_url_segments_with_two_parameters_using_static_object_in_different_order_it_should_have_the_correct_url_segments()
        {
            var _objectToUrlSegments = new ObjectToUrlSegments();
            dynamic parameter = new StaticObjectWithNameAndIdInDifferentOrder() {Name = "test", Id = 1};

            var url = _objectToUrlSegments.ParametersToUrl(parameter);

            Assert.AreEqual("/1/test", url);
        }

        public class StaticObjectWithNameAndIdInDifferentOrder
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
