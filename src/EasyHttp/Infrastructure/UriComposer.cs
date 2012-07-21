using System;

namespace EasyHttp.Infrastructure
{
    public class UriComposer
    {
        readonly ObjectToUrlParameters _objectToUrlParameters;

        public UriComposer()
        {
            _objectToUrlParameters = new ObjectToUrlParameters();
        }

        public string Compose(string baseuri, string uri, object query)
        {
            var returnUri = uri;
            if(!String.IsNullOrEmpty(baseuri))
            {
                returnUri = baseuri.EndsWith("/") ? baseuri : String.Concat(baseuri,"/");
                returnUri += uri.StartsWith("/") ? uri.Substring(1) : uri;
            }
            returnUri = query != null ? String.Concat(returnUri, _objectToUrlParameters.ParametersToUrl(query)) : returnUri;
            return returnUri;
        }
    }
}