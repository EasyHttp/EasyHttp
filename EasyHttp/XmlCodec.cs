using System;
using System.IO;
using System.Xml.Serialization;

namespace EasyHttp
{
    public class XmlCodec
    {
        public T Decode<T>(string input)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            var stringReader = new StringReader(input);

            return (T) xmlSerializer.Deserialize(stringReader);

        }

        public string Encode(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            var stringWriter = new StringWriter();

            var xmlnsEmpty = new XmlSerializerNamespaces();

            xmlnsEmpty.Add("", "");

            serializer.Serialize(stringWriter, obj, xmlnsEmpty);

            return stringWriter.ToString();
        }
    }
}