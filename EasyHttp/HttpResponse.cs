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
        readonly ICoDec _codec;

        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public CookieCollection Cookie { get; set; }
        
        public Body DynamicBody
        {
            get { return _codec.Decode<Body>(RawText, ContentType); }
        }

        public string RawText { get; set; }
        
        public T StaticBody<T>()
        {
            return _codec.Decode<T>(RawText, ContentType);
        }



        public HttpResponse(ICoDec codec)
        {
            _codec = codec;
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