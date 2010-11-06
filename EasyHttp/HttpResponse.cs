using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using EasyHttp.JsonFXExtensions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using JsonFx.Serialization.Resolvers;

namespace EasyHttp
{
    public class HttpResponse
    {
        IDataReader _deserializer;
        string _parsedRawText;

        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public CookieCollection Cookie { get; set; }
        
        public Body DynamicBody { get; set; }
        public string RawText { get; set; }
        
        public T StaticBody<T>()
        {
            if (_deserializer != null)
            {
                return _deserializer.Read<T>(_parsedRawText);
            }

            throw new SerializationException("The encoding requested does not have a corresponding serializer");
        }


        readonly DataReaderProvider readerProvider;

        public HttpResponse()
        {
            var readerSettings = new DataReaderSettings(new RemoveAmerpsandFromNameJsonResolverStrategy());

            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            readerProvider = new DataReaderProvider(new List<IDataReader>() { jsonReader, xmlReader });

            DynamicBody = new Body();
        }

        public void GetResponse(HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                ContentType = response.ContentType;
                StatusCode = response.StatusCode;
                StatusDescription = response.StatusDescription;
                Cookie = response.Cookies;

                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            RawText = reader.ReadToEnd();

                            _parsedRawText = RawText.Replace("\"@", "\"");
                            
                            _deserializer = readerProvider.Find(ContentType);

                            
                            if (_deserializer != null)
                            {
                                DynamicBody = _deserializer.Read<Body>(_parsedRawText);
                            }
                        }

                    }
                }

            }
            catch (WebException webException)
            {
                var respone = (HttpWebResponse)webException.Response;

                StatusCode = respone.StatusCode;
                StatusDescription = respone.StatusDescription;
            }
        }

    }
}