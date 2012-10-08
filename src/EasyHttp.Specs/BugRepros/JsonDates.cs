using System;
using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Configuration;
using EasyHttp.Http;
using JsonFx.Json;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using Machine.Specifications;


namespace EasyHttp.Specs.BugRepros
{
    public class when_json_dates
    {
        Establish context = () =>
        {
            input = @"{""Users"":[{""Id"":""90c68332-a5e2-42ca-bde7-9e0701412110"",""IsActive"":true,""LockedOutUntil"":""\/Date(1289073014137)\/""}],""Count"":1}";


            IEnumerable<IDataReader> readers = new List<IDataReader> { new JsonReader(new DataReaderSettings(DefaultEncoderDecoderConfiguration.CombinedResolverStrategy()), "application/.*json") };

            decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));



        };

        Because of = () =>
        {
            outputDynamic = decoder.DecodeToDynamic(input, HttpContentTypes.ApplicationJson);
            outputStatic = decoder.DecodeToStatic<UserResult>(input, HttpContentTypes.ApplicationJson);

        };

        It should_decode_correctly_to_dynamic_body = () =>
        {
            outputDynamic.Users[0].LockedOutUntil.Date.ShouldEqual(new DateTime(2010, 11, 06));
        };

        It should_decode_correctly_to_static_body = () =>
        {
            outputStatic.Users[0].LockedOutUntil.GetValueOrDefault().Date.ShouldEqual(new DateTime(2010, 11, 06));
        };

        static DefaultDecoder decoder;
        static dynamic outputDynamic;
        static string input;
        static UserResult outputStatic;

        public class UserResult
        {
            public List<User> Users { get; set; }

            public int Count { get; set; }

            public UserResult()
            {
                this.Users = new List<User>();
            }
        }

        public class User
        {
            public bool? IsDisabled { get; set; }

            public string Id { get; set; }

            public bool? IsActive { get; set; }

            public DateTime? LockedOutUntil { get; set; }
        }

    }
}
