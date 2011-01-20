using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using EasyHttp.Http;
using EasyHttp.Specs.Helpers;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var http = new HttpClient();

            dynamic customer = new ExpandoObject();

            customer.Name = "Joe Smith";
            customer.Email = "Joe@Gmail.com";
            
         
          //  http.Post("http://domain.com/customer", customer, HttpContentTypes.);

         
        }
    }
}
