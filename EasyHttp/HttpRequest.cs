using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Text;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class HttpRequest
    {

        public string Accept { get ; set; }
        public string CachePolicy { get; set; }
        public string Connection { get; set; }
        public string ContentLength { get; set; }
        public string ContentType { get; set; }


        public IDictionary<string, string> RawHeaders { get; private set; }
        public string UserAgent { get; protected set; }

        HttpWebRequest httpWebRequest;

        public HttpRequest()
        {
            //Accept = new List<string>();
            RawHeaders = new Dictionary<string, string>();

        }


        public void SetBasicAuthentication(string username, string password)
        {
            var networkCredential = new NetworkCredential(username, password);

            httpWebRequest.Credentials = networkCredential;
        }

        void SetupRequestHeader(HttpMethod method)
        {
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = method.ToString();

            foreach (var header in RawHeaders)
            {
                httpWebRequest.Headers.Add(header.Key, header.Value);
            }
        }

        void SetupRequestBody(object data)
        {
            if (data == null)
            {
                return;
            }

            var serializer = GetSerializer();

            var requestStream = httpWebRequest.GetRequestStream();

            var serialized = serializer.Write(data);

            var bytes = Encoding.UTF8.GetBytes(serialized);

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        IDataWriter GetSerializer()
        {
            var writerSettings = new DataWriterSettings();

            
            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            var urlencoderWriter = new UrlEncoderWriter(writerSettings);

            var writerProvider = new DataWriterProvider(new List<IDataWriter>() { jsonWriter, xmlWriter, urlencoderWriter });

            var serializer = writerProvider.Find(httpWebRequest.ContentType, httpWebRequest.ContentType);

          //  Have to add support for xxx-form-encoding

            if (serializer == null)
            {
                throw new EncoderFallbackException("Serializer not located");
            }

            return serializer;
        }

        public HttpResponse MakeRequest(string uri, HttpMethod method, object data = null)
        {
            httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);

            SetupRequestHeader(method);
            SetupRequestBody(data);

            var response = new HttpResponse();

            response.GetResponse(httpWebRequest);

            return response;
        }

       
    }
}