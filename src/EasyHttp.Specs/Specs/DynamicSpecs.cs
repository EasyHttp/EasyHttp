#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
// 
// 
// Parts of this Software use JsonFX Serialization Library which is distributed under the MIT License:
// 
// Distributed under the terms of an MIT-style license:
// 
// The MIT License
// 
// Copyright (c) 2006-2009 Stephen M. McKamey
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using System;
using EasyHttp.Codecs;
using EasyHttp.Infrastructure;
using Machine.Specifications;

namespace EasyHttp.Specs.Specs
{
    [Subject("DynamicType")]
    public class when_accessing_a_property_that_is_defined
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

        It should_return_the_value = () => value.ShouldEqual("Joe");

        static dynamic dynamicObject;
        static string value;
    }

    [Subject("DynamicType")]
    public class when_accessing_a_property_that_is_not_defined
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

        It should_throw_property_not_found_exception = () => exception.ShouldBeOfType<PropertyNotFoundException>();

        It should_set_property_name_to_name_of_property_not_found = () => ((PropertyNotFoundException)exception).PropertyName.ShouldEqual("Name");

        static dynamic dynamicObject;
        static Exception exception;
    }


    [Subject("DynamicType")]
    public class when_accessing_a_property_of_a_child_property_that_is_defined
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

        It should_return_the_value = () => value.ShouldEqual("Child");

        static dynamic childObject;
        static dynamic parentObject;
        static string value;
    }

    [Subject("Infrastructure")]
    public class when_accessing_a_property_of_a_child_property_that_is_not_defined
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

        It should_throw_property_not_found_exception = () => exception.ShouldBeOfType<PropertyNotFoundException>();

        It should_set_property_name_to_name_of_property_not_found = () => ((PropertyNotFoundException)exception).PropertyName.ShouldEqual("Name");

        static dynamic childObject;
        static dynamic parentObject;
        static string value;
        static Exception exception;
    }

   
}