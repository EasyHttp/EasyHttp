using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace RestClient
{
    public class RestClient: DynamicObject
    {

        DataReaderProvider _readerProvider;
        public IList<string> Accept { get; set; }
        public string ContentType { get; private set; }

        public RestClient()
        {
            var readerSettings = new DataReaderSettings();
          
            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            _readerProvider = new DataReaderProvider(new List<IDataReader>() {jsonReader, xmlReader});

        }

        public HttpResponse Get(string uri)
        {
            
            var request = WebRequest.Create(uri);

            var response = request.GetResponse();


            if (response != null)
            {
                ContentType = response.ContentType;

                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return ParseResponse(reader);
                        }
                        
                    }
                }
            }
            return null;
        }

        HttpResponse ParseResponse(StreamReader reader)
        {
            var deserializer = _readerProvider.Find(ContentType);

            var httpResponse = new HttpResponse();

            httpResponse.Body = deserializer.Read<Body>(reader);

            return httpResponse;
        }
    }
}