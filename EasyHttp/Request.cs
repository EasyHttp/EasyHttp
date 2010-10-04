using System.Collections.Generic;
using System.Net;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class Request: HttpBaseData
    {
        HttpWebRequest _internalRequest;

   
        readonly DataWriterProvider _writerProvider;

        public Request()
        {
            var writerSettings = new DataWriterSettings();

            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            _writerProvider = new DataWriterProvider(new List<IDataWriter>() { jsonWriter, xmlWriter });
           
        }

        void CreateRequestData(object data)
        {
            if (data == null)
            {
                return;
            }

            var serializer = _writerProvider.Find(Header.Accept, Header.ContentType);


            if (serializer == null)
            {
                throw new SerializationException("Cannot Serialize Data");
            }

            var requestStream = _internalRequest.GetRequestStream();

            var serialized = serializer.Write(data);

            var bytes = System.Text.Encoding.UTF8.GetBytes(serialized);

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        public Response MakeRequest(string uri, HttpMethod method, object data = null)
        {

            _internalRequest = (HttpWebRequest)WebRequest.Create(uri);

            _internalRequest.ContentType = Header.ContentType;
            _internalRequest.Accept = Header.Accept;
            _internalRequest.Method = method.ToString();

            CreateRequestData(data);

            var response = new Response();

            response.GetResponse(_internalRequest);

            return response;
        }

    }
}