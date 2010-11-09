using System.IO;
using System.Net;

namespace EasyHttp
{
    public class HttpResponse
    {
        readonly ICodec _codec;

        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public CookieCollection Cookie { get; set; }
        
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