using System.Net;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;

namespace EasyHttp.Specs.Helpers
{
    public class Hello
    {
        public string Name { get; set; }
    }

    public class HelloResponse
    {
        public string Result { get; set; }
    }

    public class Redirect
    {
        public string Name { get; set; }
    }

    public class Files
    {
        public string Name { get; set; }
    }

    public class CookieInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class SomeData
    {
        public string Id { get; set; }
    }

    public class SomeDataResponse
    {
        public string SomeValue { get; set; }
    }

    public class HelloService : RestServiceBase<Hello>
    {
        public override object OnGet(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnPut(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnPost(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }

        public override object OnDelete(Hello request)
        {
            return new HelloResponse { Result = "Hello, " + request.Name };
        }
    }

    public class FilesService : RestServiceBase<Files>
    {
        public override object OnPut(Files request)
        {
            if(base.Request.ContentType == "image/jpeg" ) return new HttpResult() { StatusCode = HttpStatusCode.Created };
            return new HttpResult() { StatusCode = HttpStatusCode.NoContent };
        }

        public override object OnPost(Files request)
        {
            if (base.Request.Files.Length == 2) return new HttpResult() { StatusCode = HttpStatusCode.OK };
            return new HttpResult() { StatusCode = HttpStatusCode.NoContent };
        }
    }

    public class CookieService : RestServiceBase<CookieInfo>
    {
        public override object OnGet(CookieInfo request)
        {
            if (!Request.Cookies.ContainsKey(request.Name))
                return new HttpResult {StatusCode = HttpStatusCode.NotFound};
            return new CookieInfo() { Name = request.Name, Value = Request.Cookies[request.Name].Value };
        }

        public override object OnPut(CookieInfo request)
        {
            Response.Cookies.AddCookie(new Cookie(request.Name, request.Value));
            return new HttpResult() { StatusCode = HttpStatusCode.OK };
        }
    }

    public class RedirectorService : RestServiceBase<Redirect>
    {
        public override object OnGet(Redirect request)
        {
            if (this.Request.AbsoluteUri.EndsWith("redirected"))
                return new HttpResult() { StatusCode = HttpStatusCode.OK, };

            return new HttpResult()
                   {
                       StatusCode = HttpStatusCode.Redirect,
                       Location = this.Request.AbsoluteUri+"/redirected"
                   };
        }
    }

    public class SomeDataService : RestServiceBase<SomeData>
    {
        public override object OnGet(SomeData request)
        {
            return new SomeDataResponse() { SomeValue = @"@bormod how are you?" };
        }
    }

    //Define the Web Services AppHost
    public class ServiceStackHost : AppHostHttpListenerBase
    {
        public ServiceStackHost() : base("StarterTemplate HttpListener", typeof(HelloService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            Routes.Add<Hello>("/hello")
                  .Add<Hello>("/hello/{Name}");
            Routes.Add<Files>("/fileupload/{Name}")
                  .Add<Files>("/fileupload");
            Routes.Add<CookieInfo>("/cookie")
                  .Add<CookieInfo>("/cookie/{Name}");
            Routes.Add<Redirect>("/redirector")
                  .Add<Redirect>("/redirector/redirected");
            Routes.Add<SomeData>("/data")
                .Add<SomeData>("/data/{Id}");
        }
    }
}