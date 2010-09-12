using System;
using System.IO;
using JsonFx.Json;
using JsonFx.Serialization.Providers;

namespace RestClient
{
    public class HttpResponse
    {
        public Body Body { get; set; }

        public HttpResponse()
        {
            Body = new Body();

           

        }

    }
}