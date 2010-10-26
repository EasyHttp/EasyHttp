using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using EasyHttp;

namespace YouTrackClient
{
    public class YouTrack
    {
        readonly string protocol;
        readonly string host;
        readonly int port;
        CookieCollection authenticationCookie;


        public YouTrack(string host, int port = 80, bool useSsl = false)
        {
            this.host = host;
            this.port = port;

            if (useSsl)
            {
                protocol = "https";
            }
            else
            {
                protocol = "http";
            }
        }

        public bool IsAuthenticated
        {
            get { return authenticationCookie["JSESSIONID"] != null;  }
        }

        public IList<Issue> GetIssues(string projectIdentifier)
        {
            var httpClient = new HttpClient();

            var response = httpClient
                .WithAccept("application/xml")
                .Get(ConstructUri("rest/project/issues/DCVR"));
                

            var list = new List<Issue>();

            list.Add(new Issue());

            return list;
        }

        string ConstructUri(string request)
        {
            return String.Format("{0}://{1}:{2}/{3}", protocol, host, port, request);
        }

        public void Login(string username, string password)
        {
            var httpClient = new HttpClient();

            var credentials = new Credentials() { Username = username, Password = password};

            httpClient
                .WithAccept("application/json")
                .Post(ConstructUri("rest/user/login"), credentials, "application/x-www-form-encoded");

            dynamic result = httpClient.Response.Body; 

            if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
            {
                throw new AuthenticationException("Authentication Failed");
            }

            authenticationCookie = httpClient.Response.Cookie;

        }
    }
}