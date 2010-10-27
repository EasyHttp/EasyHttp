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
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public CookieCollection Cookie { get; set; }
        
        public Body Body { get; set; }


        readonly DataReaderProvider readerProvider;

        public HttpResponse()
        {
            var readerSettings = new DataReaderSettings();

            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            readerProvider = new DataReaderProvider(new List<IDataReader>() { jsonReader, xmlReader });

            Body = new Body();
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
                            var deserializer = readerProvider.Find(ContentType);

                            if (deserializer == null)
                            {
                                Body.RawText = reader.ReadToEnd();
                            }
                            else
                            {
                                // TODO HACK: Dynamic objects don't support @ in property names. Stripping this out for now here for YouTrack support 
                                // Fork JsonFX and do this in the JsonReader (JsonFormatter)
                                string readToEnd = reader.ReadToEnd();
                                var newstr = readToEnd.Replace("\"@", "\"");
                                Body = deserializer.Read<Body>(newstr);
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

                throw new HttpException(StatusCode, StatusDescription);
            }
        }


       
    }
}