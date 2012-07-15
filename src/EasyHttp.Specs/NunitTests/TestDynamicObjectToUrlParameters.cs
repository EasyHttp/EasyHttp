using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.NunitTests
{
    public class TestDynamicObjectToUrlParameters
    {
        [Test]
        public void If_Adding_One_Parameter_Will_Create_The_Correct_Url()
        {
            Assert.AreEqual("?Name=test",ObjectToUrlParameters.ParametersToUrl(new {Name = "test"}));
        }

        [Test]
        public void If_Adding_Two_Parameters_Will_Create_The_Correct_Url()
        {
            Assert.AreEqual("?Name=test&Id=1", ObjectToUrlParameters.ParametersToUrl(new { Name = "test", Id = 1 }));
        }

        [Test]
        public void If_It_Will_UrlEncode_The_Value()
        {
            Assert.AreEqual("?Name=test%3c%3e%26%3b", ObjectToUrlParameters.ParametersToUrl(new { Name = "test<>&;"}));
        }

        [Test]
        public void Should_Return_An_Empty_String_When_Parameters_Are_Null()
        {
            Assert.IsEmpty(ObjectToUrlParameters.ParametersToUrl(null)); 
        }

        [Test]
        public void If_Adding_One_Parameter_Will_Create_The_Correct_Url_With_Static_Object()
        {
            Assert.AreEqual("?Name=test", ObjectToUrlParameters.ParametersToUrl(new StaticObjectWithName { Name = "test" }));
        }

        [Test]
        public void If_Adding_Two_Parameters_Will_Create_The_Correct_Url_With_Static_Object()
        {
            Assert.AreEqual("?Name=test", ObjectToUrlParameters.ParametersToUrl(new StaticObjectWithName { Name = "test" }));
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