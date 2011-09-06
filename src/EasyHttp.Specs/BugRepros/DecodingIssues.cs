#region License
// Distributed under the BSD License
//  
// EasyHttp Copyright (c) 2011-2011, Hadi Hariri and Contributors
// All rights reserved.
//  
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
//  
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//  TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//  <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//  LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  
#endregion

using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Serialization;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    public class when_decoding_an_object_that_is_an_array_to_dynamic
    {
        Establish context = () => 
        {
            input = "[{\"intAuthorID\":\"8\",\"strText\":\"test1\"},{\"intAuthorID\":\"5\",\"strText\":\"This message\"}]";

            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(), "application/.*json") };

            _decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));
        };


        Because of = () =>
        {

            output = _decoder.DecodeToDynamic(input, HttpContentTypes.ApplicationJson);
        };


        It should_decode_correctly = () =>
        {
            string authorId = output[0].intAuthorID;
            authorId.ShouldEqual("8");
        };

        static string input;
        static DefaultDecoder _decoder;
        static dynamic output;
    }
}