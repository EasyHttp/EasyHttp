using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class HttpRequest
    {
        readonly DataWriterProvider writerProvider;

        public string ContentType { get; protected set; }
        public string Accept { get; set; }
        public string UserAgent { get; protected set; }
        public IDictionary<string, string> ExtraHeaders { get; private set; }

        HttpWebRequest httpWebRequest;

        public HttpRequest()
        {
            //Accept = new List<string>();
            ExtraHeaders = new Dictionary<string, string>();

            var writerSettings = new DataWriterSettings();

            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            writerProvider = new DataWriterProvider(new List<IDataWriter>() { jsonWriter, xmlWriter });
        }


        public void SetBasicAuthentication(string username, string password)
        {
            var authData = String.Format("{0}:{1}", username, password);

            var bytes = Encoding.UTF8.GetBytes(authData);

            var base64Encoded = Convert.ToBase64String(bytes);

            ExtraHeaders.Add("Authorization",base64Encoded);   
        }

        void SetupRequestHeader(string uri, HttpMethod method)
        {
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = method.ToString();

            foreach (var header in ExtraHeaders)
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

            var serializer = writerProvider.Find(httpWebRequest.Accept, httpWebRequest.ContentType);


            if (serializer == null)
            {
                throw new SerializationException("Cannot Serialize Data");
            }

            var requestStream = httpWebRequest.GetRequestStream();

            var serialized = serializer.Write(data);

            var bytes = Encoding.UTF8.GetBytes(serialized);

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        public HttpResponse MakeRequest(string uri, HttpMethod method, object data = null)
        {
            httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);

            SetupRequestHeader(uri, method);
            SetupRequestBody(data);

            var response = new HttpResponse();

            response.GetResponse(httpWebRequest);

            return response;
        }

       
    }
}