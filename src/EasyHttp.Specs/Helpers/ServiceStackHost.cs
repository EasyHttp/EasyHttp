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

    public class Files
    {
        public string Name { get; set; }
    }

    public class FilesResponse
    {
        public string Result { get; set; }
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
    }

    //Define the Web Services AppHost
    public class ServiceStackHost : AppHostHttpListenerBase
    {
        public ServiceStackHost() : base("StarterTemplate HttpListener", typeof(HelloService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            Routes
                            .Add<Hello>("/hello")
                            .Add<Hello>("/hello/{Name}");
            Routes.Add<Files>("/fileupload/{Name}");
        }
    }
}