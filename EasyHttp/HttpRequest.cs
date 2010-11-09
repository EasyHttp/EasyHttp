using System;
using System.Collections.Generic;
using System.Net;

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

        readonly ICodec _codec;

        public HttpRequest(ICodec codec)
        {
            RawHeaders = new Dictionary<string, string>();
            _codec = codec;
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

            var bytes = _codec.Encode(Data, ContentType);

            var requestStream = httpWebRequest.GetRequestStream();
            
            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }


        public HttpResponse MakeRequest()
        {
            httpWebRequest = (HttpWebRequest) WebRequest.Create(Uri);

            SetupRequestHeader();

            SetupRequestBody();

            var response = new HttpResponse(_codec);

            response.GetResponse(httpWebRequest);

            return response;
        }

       
    }
}