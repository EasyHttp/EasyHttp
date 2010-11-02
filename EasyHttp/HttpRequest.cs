using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Text;
using EasyHttp.JsonFXExtensions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using SerializationException = System.Runtime.Serialization.SerializationException;

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
        
        public string UserAgent { get; set; }

        public HttpMethod Method { get; set; }

        public object Data { get; set; }

        public string Uri { get; set; }

        HttpWebRequest httpWebRequest;

        public HttpRequest()
        {
            //Accept = new List<string>();
            RawHeaders = new Dictionary<string, string>();

        }


        public void SetBasicAuthentication(string username, string password)
        {
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var networkCredential = new NetworkCredential(username, password);

                httpWebRequest.Credentials = networkCredential;
            }
        }

        void SetupRequestHeader()
        {
            httpWebRequest.ContentType = ContentType;
            httpWebRequest.Accept = Accept;
            httpWebRequest.Method = Method.ToString();

            foreach (var header in RawHeaders)
            {
                httpWebRequest.Headers.Add(header.Key, header.Value);
            }
        }

        void SetupRequestBody()
        {
            if (Data == null)
            {
                return;
            }

            var serializer = GetSerializer();

            var requestStream = httpWebRequest.GetRequestStream();

            var serialized = serializer.Write(Data);

            var bytes = Encoding.UTF8.GetBytes(serialized);

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        IDataWriter GetSerializer()
        {
            var writerSettings = new DataWriterSettings();

            
            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            var urlEncoderWriter = new UrlEncoderWriter(writerSettings);

            var writerProvider = new DataWriterProvider(new List<IDataWriter>() { jsonWriter, xmlWriter, urlEncoderWriter });

            var serializer = writerProvider.Find(httpWebRequest.ContentType, httpWebRequest.ContentType);

            if (serializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding serializer");
            }

            return serializer;
        }

        public HttpResponse MakeRequest()
        {
            httpWebRequest = (HttpWebRequest) WebRequest.Create(Uri);

            SetupRequestHeader();
            SetupRequestBody();

            var response = new HttpResponse();

            response.GetResponse(httpWebRequest);

            return response;
        }

       
    }
}