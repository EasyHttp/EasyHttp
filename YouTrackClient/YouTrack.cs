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

        /// <summary>
        /// Creates a new instance of YouTrack setting the appropriate host and port for successive calls. 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
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

        /// <summary>
        /// Indicates whether a successful login has already taken place
        /// <seealso cref="Login"/>
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Retrieves a list of issues 
        /// </summary>
        /// <param name="projectIdentifier">Project Identifier</param>
        /// <returns>List of Issues</returns>
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





        /// <summary>
        /// Create base Uri for YouTrack containing host, port and specific request
        /// </summary>
        /// <param name="request">Specific request</param>
        /// <returns>Uri</returns>
        string ConstructUri(string request)
        {
            return String.Format("{0}://{1}:{2}/{3}", protocol, host, port, request);
        }

        /// <summary>
        /// Logs in to YouTrack provided the correct username and password. If successful, <see cref="IsAuthenticated"/>will be true
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Passowrd</param>
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