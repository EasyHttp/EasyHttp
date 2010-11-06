using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using EasyHttp.JsonFXExtensions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class CoDec : ICoDec
    {
        readonly IDataReaderProvider _readerProvider;
        readonly IDataWriterProvider _writerProvider;
        
        public CoDec()
        {
            var writerSettings = new DataWriterSettings();


            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            var urlEncoderWriter = new UrlEncoderWriter(writerSettings);

            _writerProvider = new DataWriterProvider(new List<IDataWriter>() { jsonWriter, xmlWriter, urlEncoderWriter });


            var readerSettings = new DataReaderSettings(new RemoveAmerpsandFromNameJsonResolverStrategy());

            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);

            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            _readerProvider = new DataReaderProvider(new List<IDataReader>() { jsonReader, xmlReader });
        }


        public byte[] Encode(object data, string contentType)
        {

            var serializer = _writerProvider.Find(contentType, contentType);

            if (serializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding encoder");
            }

            var serialized = serializer.Write(data);

            return Encoding.UTF8.GetBytes(serialized);
        }

       
        public T Decode<T>(string input, string contentType)
        {
            var parsedText = input.Replace("\"@", "\"");
                            
            var deserializer = _readerProvider.Find(contentType);

                            
            if (deserializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding decoder");
            }

            return deserializer.Read<T>(parsedText);
 
        }


    }
}