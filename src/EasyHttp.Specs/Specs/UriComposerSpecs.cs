using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.Specs
{
    public class UriComposerSpecs
    {
        [Test]
        public void When_baseuri_is_null_and_query_is_null_it_should_return_the_uri()
        {
            var uriComposer = new UriComposer();
            var uri = "uri";

            var url = uriComposer.Compose(null, uri, null, false);

            Assert.AreEqual("uri", url);
        }

        [Test]
        public void When_baseuri_is_empty_and_query_is_null_should_return_the_uri_it_should_return_the_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "";
            var uri = "uri";

            var url = uriComposer.Compose(baseuri, uri, null, false);

            Assert.AreEqual("uri", url);
        }

        [Test]
        public void When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_query_is_null_it_should_return_the_baseuri_plus_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri";
            var uri = "uri";

            var url = uriComposer.Compose(baseuri, uri, null, false);

            Assert.AreEqual("baseuri/uri", url);
        }

        [Test]
        public void When_baseuri_is_filled_and_ends_with_a_forwardslash_and_query_is_null_it_should_return_the_baseuri_plus_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri/";
            var uri = "uri";

            var url = uriComposer.Compose(baseuri, uri, null, false);

            Assert.AreEqual("baseuri/uri", url);
        }

        [Test]
        public void
            When_baseuri_is_filled_and_ends_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null_it_should_return_the_baseuri_plus_uri
            ()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri/";
            var uri = "/uri";

            var url = uriComposer.Compose(baseuri, uri, null, false);

            Assert.AreEqual("baseuri/uri", url);
        }

        [Test]
        public void When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null_it_should_return_the_baseuri_plus_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri";
            var uri = "/uri";

            var url = uriComposer.Compose(baseuri, uri, null, false);

            Assert.AreEqual("baseuri/uri", url);
        }

        [Test]
        public void When_baseuri_and_url_are_filled_and_query_is_not_null_it_should_return_the_baseuri_plus_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri";
            var uri = "/uri";
            var query = new {Name = "test"};

            var url = uriComposer.Compose(baseuri, uri, query, false);

            Assert.AreEqual("baseuri/uri?Name=test", url);
        }

        [Test]
        public void When_baseuri_and_url_are_filled_and_query_is_not_null_and_ParametersAsSegments_is_true_should_return_the_baseuri_plus_uri()
        {
            var uriComposer = new UriComposer();
            var baseuri = "baseuri";
            var uri = "/uri";
            var query = new {Name = "test"};

            var url = uriComposer.Compose(baseuri, uri, query, true);

            Assert.AreEqual("baseuri/uri/test", url);
        }
    }
}