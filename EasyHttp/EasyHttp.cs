using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using EasyHttp;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;
using SerializationException = System.Runtime.Serialization.SerializationException;

namespace EasyHttp
{
    public class EasyHttp
    {
        

        public HttpResponse Response { get; private set; }
        public HttpRequest Request { get; private set;  }

        public EasyHttp()
        {
            Request = new HttpRequest();
        }
        
        
        public EasyHttp WithBasicAuthentication(string username, string password)
        {
            Request.SetBasicAuthentication(username, password);    
            return this;
        }

        

        public HttpResponse Get(string uri)
        {
            Response = Request.MakeRequest(uri, HttpMethod.GET);
            return Response;
        }

        public void Post(string uri, object data)
        {
            Response = Request.MakeRequest(uri, HttpMethod.POST, data);
        }

      

        public void Put(string uri, object data)
        {
            Response = Request.MakeRequest(uri, HttpMethod.PUT, data);
        }

        public void Delete(string uri)
        {
            Response = Request.MakeRequest(uri, HttpMethod.DELETE);
        }

      
        public EasyHttp WithAccept(string accept)
        {
            Request.Accept = accept;
            return this;
        }

  
        public void Head(string uri)
        {
            Response = Request.MakeRequest(uri, HttpMethod.HEAD);
        }
    }

   
}