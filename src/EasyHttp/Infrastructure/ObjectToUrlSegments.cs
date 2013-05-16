namespace EasyHttp.Infrastructure
{
    public class ObjectToUrlSegments : ObjectToUrl
    {

        protected override string PathStartCharacter
        {
            get { return "/"; }
        }

        protected override string PathSeparatorCharacter
        {
            get { return "/"; }
        }

        protected override string BuildParam(PropertyValue propertyValue)
        {
            return System.Web.HttpUtility.UrlEncode(propertyValue.Value);
        }
    }
}