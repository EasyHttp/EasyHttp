using System;
using System.IO;
using System.Reflection;
using EasyHttp.Specs.Helpers;
using NUnit.Framework;

namespace EasyHttp.Specs
{
    [SetUpFixture]
    public class TestRunContext
    {
        public static DirectoryInfo WorkingDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        private ServiceStackHost _appHost;
        private int _port;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _port = 16000;
            var listeningOn = String.Format("http://localhost:{0}/", _port);
            _appHost = new ServiceStackHost();
            _appHost.Init();
            _appHost.Start(listeningOn);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _appHost.Stop();
            _appHost.Dispose();
        }
    }
}