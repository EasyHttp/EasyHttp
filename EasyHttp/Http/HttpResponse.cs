using System;
using System.IO;
using System.Net;
using EasyHttp.Codecs;

namespace EasyHttp.Http
{
    public class HttpResponse
    {
        readonly ICodec _codec;
        HttpWebResponse _response;

        public string ContentType { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public string StatusDescription { get; private set; }
        public CookieCollection Cookie { get; private set; }
        public int Age { get; private set; }
        public HttpMethod[] Allow { get; private set; }
        public CacheControl CacheControl { get; private set; }
        public string ContentEncoding { get; private set; }
        public string ContentLanguage { get; private set; }
        public long ContentLength { get; private set; }
        public string ContentLocation { get; private set; }
        
        // TODO :This should be files
        public string ContentDisposition { get; private set; }
        
        public DateTime Date { get; private set; }
        public string ETag { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime LastModified { get; private set; }
        public string Location { get; private set; }
        public CacheControl Pragma { get; private set; }
        public string Server { get; private set; }
        public WebHeaderCollection RawHeaders { get; private set; }

        

        
        public dynamic DynamicBody
        {
            get { return _codec.DecodeToDynamic(RawText, ContentType); }
        }

        public string RawText { get; set; }
        
        public T StaticBody<T>()
        {
            return _codec.DecodeToStatic<T>(RawText, ContentType);
        }



        public HttpResponse(ICodec codec)
        {
            _codec = codec;
     
        }

        public void GetResponse(HttpWebRequest request)
        {
            try
            {
                _response = (HttpWebResponse)request.GetResponse();

                ContentType = _response.ContentType;
                StatusCode = _response.StatusCode;
                StatusDescription = _response.StatusDescription;
                Cookie = _response.Cookies;
                ContentEncoding = _response.ContentEncoding;
                ContentLength = _response.ContentLength;
                Date = DateTime.Now;
                LastModified = _response.LastModified;
                Server = _response.Server;

                if (!String.IsNullOrEmpty(_response.GetResponseHeader("Age")))
                {
                    Age = Convert.ToInt32(_response.GetResponseHeader("Age"));
                }

                ContentLanguage = _response.GetResponseHeader("Content-Language");
                ContentLocation = _response.GetResponseHeader("Content-Location");
                ContentDisposition = _response.GetResponseHeader("Content-Disposition");
                ETag = _response.GetResponseHeader("ETag");
                Location = _response.GetResponseHeader("Location");
                
                if (!String.IsNullOrEmpty(_response.GetResponseHeader("Expires")))
                {
                    Expires = Convert.ToDateTime(_response.GetResponseHeader("Expires"));
                }

                // TODO: Finish this.
                 //   public HttpMethod Allow { get; private set; }
                 //   public CacheControl CacheControl { get; private set; }
                 //   public CacheControl Pragma { get; private set; }


                RawHeaders = _response.Headers;

                using (var stream = _response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            RawText = reader.ReadToEnd();
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