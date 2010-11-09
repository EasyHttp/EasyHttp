using System;
using System.Collections.Generic;
using System.Text;
using EasyHttp.JsonFXExtensions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class DefaultCodec : ICodec
    {
        public byte[] Encode(object data, string contentType)
        {
            var writerSettings = new DataWriterSettings();

            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            var urlEncoderWriter = new UrlEncoderWriter(writerSettings);

            IDataWriterProvider writerProvider = new DataWriterProvider(new List<IDataWriter> { jsonWriter, xmlWriter, urlEncoderWriter });
            
            var serializer = writerProvider.Find(contentType, contentType);

            if (serializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding encoder");
            }

            var serialized = serializer.Write(data);

            return Encoding.UTF8.GetBytes(serialized);
        }

       
        public T DecodeToStatic<T>(string input, string contentType)
        {
            var readerSettings = new DataReaderSettings(new RemoveAmerpsandFromNameJsonResolverStrategy());

            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            IDataReaderProvider readerProvider = new DataReaderProvider(new List<IDataReader> { jsonReader, xmlReader });

            // TODO: This is a hack...
            var parsedText = input.Replace("\"@", "\"");
                            
            var deserializer = readerProvider.Find(contentType);

                            
            if (deserializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding decoder");
            }

            
            return deserializer.Read<T>(parsedText);
 
        }

        public dynamic DecodeToDynamic(string rawText, string contentType)
        {
            return DecodeToStatic<DynamicType>(rawText, contentType);
        }
    }
}