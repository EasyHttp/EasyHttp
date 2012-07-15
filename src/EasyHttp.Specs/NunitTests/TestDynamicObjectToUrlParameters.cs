using System;
using System.Dynamic;
using System.Linq;
using EasyHttp.Infrastructure;
using NUnit.Framework;

namespace EasyHttp.Specs.NunitTests
{
    public class TestDynamicObjectToUrlParameters
    {
        [Test]
        public void If_Adding_One_Parameter_Will_Create_The_Correct_Url_With_Anonymous_Object()
        {
            Assert.AreEqual("?Name=test",ObjectToUrlParameters.ParametersToUrl(new {Name = "test"}));
        }

        [Test]
        public void If_Adding_Two_Parameters_Will_Create_The_Correct_Url_With_Anonymous_Object()
        {
            Assert.AreEqual("?Name=test&Id=1", ObjectToUrlParameters.ParametersToUrl(new { Name = "test", Id = 1 }));
        }

        [Test]
        public void If_It_Will_UrlEncode_The_Value_With_Anonymous_Object()
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
            Assert.AreEqual("?Name=test&Id=1", ObjectToUrlParameters.ParametersToUrl(new StaticObjectWithNameAndId { Name = "test", Id=1 }));
        }

        [Test]
        public void If_Adding_One_Parameter_Will_Create_The_Correct_Url_With_Dynamic_Object()
        {
            dynamic x = new ExpandoObject();
            x.Name = "test";
            object o = x;
            string[] propertyNames = o.GetType().GetProperties().Select(p => p.Name).ToArray();
            foreach (var prop in propertyNames)
            {
                object propValue = o.GetType().GetProperty(prop).GetValue(o, null);
            }
            Assert.AreEqual("?Name=test", ObjectToUrlParameters.ParametersToUrl(x));
        }

        [Test]
        public void If_Adding_Two_Parameters_Will_Create_The_Correct_Url_With_Dynamic_Object()
        {
            dynamic x = new ExpandoObject();
            x.Name = "test";
            x.Id = 1;
            Assert.AreEqual("?Name=test&Id=1", ObjectToUrlParameters.ParametersToUrl(x));
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