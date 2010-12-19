using System.Collections.Generic;
using System.Text;
using EasyHttp.Codecs.JsonFXExtensions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using StructureMap;

namespace EasyHttp.Codecs
{
    public class DefaultCodec : ICodec
    {
        public byte[] Encode(object data, string contentType)
        {
      
            var dataWriters = ObjectFactory.GetAllInstances<IDataWriter>();

            var writerProvider = new DataWriterProvider(dataWriters);

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
            var dataReaders = ObjectFactory.GetAllInstances<IDataReader>();

            
            var readerProvider = new DataReaderProvider(dataReaders);
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