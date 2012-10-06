namespace EasyHttp.Specs.Helpers
{
    public class InitAndTearDownServiceStackHost
    {
        private ServiceStackHost _appHost;
        private readonly int _port;

        public InitAndTearDownServiceStackHost(int port)
        {
            _port = port;
        }

        public void Init()
        {
            var listeningOn = "http://localhost:" + _port + "/";
            _appHost = new ServiceStackHost();
            _appHost.Init();
            _appHost.Start(listeningOn); 
        }

        public void TearDown()
        {
            _appHost.Dispose();
        }
    }
}