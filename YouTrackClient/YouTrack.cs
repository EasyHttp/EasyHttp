using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public bool IsAuthenticated { get; private set; }

        public IList<Issue> GetIssues(string projectIdentifier)
        {
            var httpClient = new HttpClient();

            dynamic response = httpClient
                .WithAccept("application/json")
                .Get(ConstructUri("rest/project/issues/DCVR"));

            
            dynamic issues = response.Body.issue;

            var list = new List<Issue>();

            foreach (dynamic entry in issues)
            {
                var issue = new Issue();

                issue.Summary = entry.summary;

                list.Add(issue);
            }

            return list;
        }

        string ConstructUri(string request)
        {
            return String.Format("{0}://{1}:{2}/{3}", protocol, host, port, request);
        }

        public void Login(string username, string password)
        {
            var httpClient = new HttpClient();

            dynamic credentials = new ExpandoObject();

            credentials.login = "youtrackapi";
            credentials.password = "youtrackapi";

            try
            {
                httpClient
                    .WithAccept("application/xml")
                    .Post(ConstructUri("rest/user/login"), credentials, "application/x-www-form-urlencoded");

                dynamic result = httpClient.Response.Body;

                if (String.Compare(result.login, "ok", StringComparison.CurrentCultureIgnoreCase) != 0)
                {
                    throw new AuthenticationException("Authentication Failed");
                }
                IsAuthenticated = true;
                authenticationCookie = httpClient.Response.Cookie;
            }
            catch (HttpException)
            {
                IsAuthenticated = false;
            }


        }
    }
}