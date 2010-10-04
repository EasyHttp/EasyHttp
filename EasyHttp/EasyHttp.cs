using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace EasyHttp
{
    public class EasyHttp
    {
        // TODO: Move all encoding and request/response stuff to the classes now created
        readonly DataReaderProvider _readerProvider;
        readonly DataWriterProvider _writerProvider;
        readonly Response _response;
        readonly Request _request;

        HttpWebRequest _internalRequest;

        
        public Response Response { get { return _response;  } }
        public Request Request { get { return _request; } }

        public EasyHttp()
        {
            var readerSettings = new DataReaderSettings();
          
            var jsonReader = new JsonFx.Json.JsonReader(readerSettings);
          
            var xmlReader = new JsonFx.Xml.XmlReader(readerSettings);

            _readerProvider = new DataReaderProvider(new List<IDataReader>() {jsonReader, xmlReader});


            var writerSettings = new DataWriterSettings();

            var jsonWriter = new JsonFx.Json.JsonWriter(writerSettings);

            var xmlWriter = new JsonFx.Xml.XmlWriter(writerSettings);

            _writerProvider = new DataWriterProvider(new List<IDataWriter>() {jsonWriter, xmlWriter});
            
            _response = new Response();
            _request = new Request();
        }

        public void Get(string uri)
        {
            CreateRequest(uri, HttpMethod.GET);

            GetResponse();
        }

        public void Post(string uri, object data)
        {
            CreateRequest(uri, HttpMethod.POST);
            CreateRequestData(data);
            GetResponse();
        }

        void CreateRequestData(object data)
        {
            var serializer = _writerProvider.Find(_request.Header.Accept, _request.Header.ContentType);
          

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

        void GetResponse()
        {
            var webResponse = (HttpWebResponse)_internalRequest.GetResponse();

            Response.Header.ContentType = webResponse.ContentType;
            Response.Header.StatusDescription = webResponse.StatusDescription;

            using (var stream = webResponse.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var deserializer = _readerProvider.Find(Response.Header.ContentType);

                        if (deserializer == null)
                        {
                            _response.Body.RawText = reader.ReadToEnd();
                        }
                        else
                        {
                            _response.Body = deserializer.Read<Body>(reader);
                        }
                    }
                        
                }
            }
        }

        HttpWebRequest CreateRequest(string uri, HttpMethod method)
        {
            _internalRequest = (HttpWebRequest) WebRequest.Create(uri);

            _internalRequest.ContentType = _request.Header.ContentType;
            _internalRequest.Accept = _request.Header.Accept;
            _internalRequest.Method = method.ToString();

            return _internalRequest;
        }


        public void Put(string uri, object data)
        {
            CreateRequest(uri, HttpMethod.PUT);

            CreateRequestData(data);

            GetResponse();

        }

        public void Delete(string uri)
        {
            CreateRequest(uri, HttpMethod.DELETE);
            GetResponse();
        }

        public void SetContentType(string contentType)
        {
            _request.Header.ContentType = contentType;
        }

        public void SetAccept(string accept)
        {
            _request.Header.Accept = accept;
        }

        public void Head(string uri)
        {
            CreateRequest(uri, HttpMethod.HEAD);
            GetResponse();
        }
    }
}