using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EasyHttp.Specs.Helpers
{
    public static class TestSettings
    {
        public static string CouchDbRootUrl
        {
            get
            {

                return  ConfigurationManager.AppSettings["CouchDbRootUrl"];

            }
        }

        public static string CouchDbDatabaseUrl
        {
            get { 
                
                return ConfigurationManager.AppSettings["CouchDbDatabaseUrl"];
              
            }
        } 
    }
}
