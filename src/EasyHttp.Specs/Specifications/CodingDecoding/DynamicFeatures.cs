using System;
using EasyHttp.Codecs;
using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.Specifications.CodingDecoding
{
    [Subject("DynamicType")]
    public class when_accessing_a_property_that_is_defined_with_no_member_behavior
    {
        Establish context = () =>
        {
            dynamicObject = new DynamicType();
            
            dynamicObject.Name = "Joe";
        };

        Because of = () =>
        {

            value = dynamicObject.Name;

        };

        It should_return_the_value = () =>
        {
            value.ShouldEqual("Joe");
        };

        static dynamic dynamicObject;
        static string value;
    }

    [Subject("DynamicType")]
    public class when_accessing_a_property_that_is_not_defined_with_no_member_behavior
    {
        Establish context = () =>
        {
            dynamicObject = new DynamicType();

        };

        Because of = () =>
        {
            string value;
            exception = Catch.Exception( () => value = dynamicObject.Name );
        };

        It should_throw_property_not_found_exception = () =>
        {
            exception.ShouldBeOfType(typeof(PropertyNotFoundException));
            
            ((PropertyNotFoundException)exception).PropertyName.ShouldEqual("Name");
        };

        static dynamic dynamicObject;
        static Exception exception;
    }


    [Subject("DynamicType")]
    public class when_accessing_a_property_of_a_child_property_that_is_defined_with_no_member_behavior
    {
        Establish context = () =>
        {
            childObject = new DynamicType();

            childObject.Name = "Child";

            parentObject = new DynamicType();

            parentObject.Child = childObject;

        };

        Because of = () =>
        {
            value = parentObject.Child.Name;
        };

        It should_return_the_value = () =>
        {
            value.ShouldEqual("Child");
        };

        static dynamic childObject;
        static dynamic parentObject;
        static string value;
    }

    [Subject("DynamicType")]
    public class when_accessing_a_property_of_a_child_property_that_is_not_defined_with_no_member_behavior
    {
        Establish context = () =>
        {
            childObject = new DynamicType();

            parentObject = new DynamicType();

            parentObject.Child = childObject;
        };

        Because of = () =>
        {
            string value;
            exception = Catch.Exception(() => value = parentObject.Child.Name);
        };

        It should_throw_property_not_found_exception = () =>
        {
            exception.ShouldBeOfType(typeof(PropertyNotFoundException));

            ((PropertyNotFoundException)exception).PropertyName.ShouldEqual("Name");
        };

        static dynamic childObject;
        static dynamic parentObject;
        static string value;
        static Exception exception;
    }

   
}