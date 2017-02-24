using System.Dynamic;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    [TestFixture(TestOf = typeof(HttpClient))]
    public class DynamicObjectToUrlSpecs
    {
        [Test]
        public void when_making_url_parameters_with_one_parameter_using_expando_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();
            dynamic parameters = new ExpandoObject();
            parameters.Name = "test";

            var url = objectToUrlParameters.ParametersToUrl(parameters);

            Assert.AreEqual("?Name=test", url);
        }

        [Test]
        public void when_making_url_parameters_with_two_parameters_using_expando_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();
            dynamic parameters = new ExpandoObject();
            parameters.Name = "test";
            parameters.Id = 1;

            var url = objectToUrlParameters.ParametersToUrl(parameters);

            Assert.AreEqual("?Name=test&Id=1", url);
        }

        [Test]
        public void when_making_url_parameters_with_one_parameter_using_anonymous_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();

            var url = objectToUrlParameters.ParametersToUrl(new {Name = "test"});

            Assert.AreEqual("?Name=test", url);
        }

        [Test]
        public void when_making_url_parameters_with_two_parameters_using_anonymous_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();

            var url = objectToUrlParameters.ParametersToUrl(new {Name = "test", Id = 1});

            Assert.AreEqual("?Name=test&Id=1", url);
        }

        [Test]
        public void when_making_url_parameters_it_should_encode_value_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();

            var url = objectToUrlParameters.ParametersToUrl(new {Name = "test<>&;"});

            Assert.AreEqual("?Name=test%3c%3e%26%3b", url);
        }

        [Test]
        public void when_making_url_parameters_it_should_be_empty_when_passing_null_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();

            var url = objectToUrlParameters.ParametersToUrl(null);

            Assert.IsEmpty(url);
        }

        [Test]
        public void when_making_url_parameters_with_one_parameter_using_static_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();
            dynamic parameter = new StaticObjectWithName() {Name = "test"};

            var url = objectToUrlParameters.ParametersToUrl(parameter);

            Assert.AreEqual("?Name=test", url);
        }

        [Test]
        public void when_making_url_parameters_with_two_parameters_using_static_object_it_should_have_the_correct_url_parameters()
        {
            var objectToUrlParameters = new ObjectToUrlParameters();
            var parameter = new StaticObjectWithNameAndId() {Name = "test", Id = 1};

            var url = objectToUrlParameters.ParametersToUrl(parameter);

            Assert.AreEqual("?Name=test&Id=1", url);
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
}
