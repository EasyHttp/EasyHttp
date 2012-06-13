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

using System;
using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Configuration;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Model.Filters;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using Machine.Specifications;

namespace EasyHttp.Specs.BugRepros
{
    public class when_decoding_date_in_iso8601_format
    {
        Establish context = () =>
        {
            input = "{\"LockedOutUntil\":\"/Date(1289073014137)/\"}";


            IEnumerable<IDataReader> readers = new List<IDataReader> { new 
                JsonReader(new DataReaderSettings(DefaultEncoderDecoderConfiguration.CombinedResolverStrategy(), 
                    new MSAjaxDateFilter(),
                    new Iso8601DateFilter()
                ), "application/.*json") };

            decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));



        };

        Because of = () =>
        {
            outputStatic = decoder.DecodeToStatic<User>(input, HttpContentTypes.ApplicationJson);
        };

        It should_decode_correctly_to_dynamic_body = () =>
        {
            outputStatic.LockedOutUntil.ShouldEqual(new DateTime(2010, 11, 06));
        };

        static DefaultDecoder decoder;
        static User outputStatic;
        static string input;

        public class User
        {
            public DateTime? LockedOutUntil { get; set; }
        }

    }

    public class when_decoding_an_object_that_is_an_array_to_dynamic
    {
        Establish context = () => 
        {
            input = "[{\"intAuthorID\":\"8\",\"strText\":\"test1\"},{\"intAuthorID\":\"5\",\"strText\":\"This message\"}]";

            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(DefaultEncoderDecoderConfiguration.CombinedResolverStrategy()), "application/.*json") };

            decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));
        };


        Because of = () =>
        {

            output = decoder.DecodeToDynamic(input, HttpContentTypes.ApplicationJson);
        };


        It should_decode_correctly = () =>
        {
            string authorId = output[0].intAuthorID;
            authorId.ShouldEqual("8");
        };

        static string input;
        static DefaultDecoder decoder;
        static dynamic output;
    }

    public class when_decoding_fields_with_underscores
    {
        Establish context = () =>
        {
            input =
                "{\"html_attributions\": [],\"result\": {\"address_components\": [{\"long_name\": \"Church Street\",\"short_name\": \"Church Street\",\"types\": [\"route\"]},{\"long_name\": \"Wilmslow\",\"short_name\": \"Wilmslow\",\"types\": [\"locality\",\"political\"]},{\"long_name\": \"GB\",\"short_name\": \"GB\",\"types\": [\"country\",\"political\"]},{\"long_name\": \"SK9 1\",\"short_name\": \"SK9 1\",\"types\": [\"postal_code\"]}],\"formatted_address\": \"Church Street, Wilmslow, SK9 1, United Kingdom\",\"formatted_phone_number\": \"01625 538831\",\"geometry\": {\"location\": {\"lat\": 53.328908,\"lng\": -2.229191}},\"icon\": \"http://maps.gstatic.com/mapfiles/place_api/icons/generic_business-71.png\",\"id\": \"51155a69bc03041b926e44f03a5bbe9feafb5035\",\"international_phone_number\": \"+44 1625 538831\",\"name\": \"Waitrose\",\"reference\": \"CmRfAAAAUZ4dYk9VpNJd1mFxa970TxVGgp9QTGeEa1BaU_wTWdTHNLCcB-9YyNu5LjgIewxo_oOna0KI9f_Z-Xff4CxvTf9wFHTHgE1wRGyCLLJo2BPjkGHo5Qem-Z-2_FKiY3gmEhA_Qs0jcQyFgVEs1BZAt_bdGhRerV30JziD2x7ZOMgxQTKlnH0yAQ\",\"types\": [\"grocery_or_supermarket\",\"food\",\"store\",\"establishment\"],\"url\": \"http://maps.google.com/maps/place?cid=14979720525476796445\",\"vicinity\": \"Church Street, Wilmslow\",\"website\": \"http://www.waitrose.com/wilmslow\"},\"status\": \"OK\"}";

            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(DefaultEncoderDecoderConfiguration.CombinedResolverStrategy()), "application/.*json") };

            decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));

       
      
        };

        Because of = () =>
        {
            outputDynamic = decoder.DecodeToDynamic(input, HttpContentTypes.ApplicationJson);
            outputStatic = decoder.DecodeToStatic<PlaceResponse<PlaceDetail>>(input, HttpContentTypes.ApplicationJson);
            
        }; 
        
        It should_decode_correctly_to_dynamic_body = () =>
        {
            string formatted_address = outputDynamic.result.formatted_address;
            formatted_address.ShouldEqual("Church Street, Wilmslow, SK9 1, United Kingdom");
        };

        It should_drecode_correctly_to_static_body = () =>
        {

            outputStatic.result.formatted_address.ShouldEqual("Church Street, Wilmslow, SK9 1, United Kingdom");
        };     
        
        static DefaultDecoder decoder;
        static dynamic outputDynamic;
        static string input;
        static PlaceResponse<PlaceDetail> outputStatic;

    }

    public class response_that_contains_umlats
    {
        Because of = () =>
        {
            var http = new HttpClient();

            http.Request.Accept = HttpContentTypes.ApplicationJson;
            
            response = http.Get("https://api.github.com/users/thecodejunkie");


        };

        It should_correctly_decode_umlats = () =>
        {
            var user = response.DynamicBody;
            string username = user.name;
            username.ShouldEqual("Andreas Håkansson");
        };

        static HttpResponse response;
    }








    public class PlaceResponse<T>
    {
        public string status { get; set; }
        public T result { get; set; }
        public string[] html_attributions { get; set; }
    }

    public class PlaceLocation
    {
        public double lat { get; set; }
        public double lng { get; set; }

    }

    public class PlaceGeometry
    {
        public PlaceLocation location { get; set; }
    }

    public class PlaceDetail
    {
        public string name { get; set; }

        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }

        public string icon { get; set; }
        public string id { get; set; }
        public PlaceAddressComponent[] address_components { get; set; }

        public PlaceGeometry geometry { get; set; }
        public string reference { get; set; }
        public string[] types { get; set; }
        public string url { get; set; }
        public string vicinity { get; set; }
        public string website { get; set; }
    }

    public class PlaceAddressComponent
    {
        public string[] types { get; set; }
        public string long_name { get; set; }
        public string short_name { get; set; }
    }

}