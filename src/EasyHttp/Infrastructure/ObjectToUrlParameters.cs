using System;

namespace EasyHttp.Infrastructure
{
    public class ObjectToUrlParameters : ObjectToUrl
    {
        protected override string PathStartCharacter
        {
            get { return "?"; }
        }

        protected override string PathSeparatorCharacter
        {
            get { return "&"; }
        }

        protected override string BuildParam(PropertyValue propertyValue)
        {
            return String.Join("=", propertyValue.Name, System.Web.HttpUtility.UrlEncode(propertyValue.Value));
        }

    }
}