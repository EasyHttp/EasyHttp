using System;
using System.Web.Script.Serialization;

namespace EasyHttp
{
    public class JsonCodec
    {
        readonly JavaScriptSerializer javaScriptSerializer;

        public JsonCodec()
        {
            javaScriptSerializer = new JavaScriptSerializer();

            javaScriptSerializer.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });

        }

        public string Encode(object obj)
        {
            return javaScriptSerializer.Serialize(obj);
        }

        public T Decode<T>(string input)
        {
            return javaScriptSerializer.Deserialize<T>(input);
        }

    }
}