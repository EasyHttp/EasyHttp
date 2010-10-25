using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class HttpResponse
    {
        public string ContentType { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; }
        
        public Body Body { get; set; }
        
        readonly DataReaderProvider readerProvider;

        public HttpResponse()
        {
            var readerSettings = new DataReaderSettings();

            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            readerProvider = new DataReaderProvider(new List<IDataReader>() { jsonReader, xmlReader });
        }

        public void GetResponse(HttpWebRequest request)
        {
            var webResponse = (HttpWebResponse)request.GetResponse();

            ContentType = webResponse.ContentType;
            StatusDescription = webResponse.StatusDescription;

            using (var stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var deserializer = readerProvider.Find(ContentType);

                        if (deserializer == null)
                        {
                            Body.RawText = reader.ReadToEnd();
                        }
                        else
                        {
                            Body = deserializer.Read<Body>(reader);
                        }
                    }

                }
            }
        }


       
    }
}