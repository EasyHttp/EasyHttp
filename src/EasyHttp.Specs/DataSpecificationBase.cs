using EasyHttp.Specs.Helpers;
using Machine.Specifications;

namespace EasyHttp.Specs
{
    public class DataSpecificationBase : IAssemblyContext
    {
        private ServiceStackHost _appHost;
        private int _port;

        void IAssemblyContext.OnAssemblyComplete()
        {
            _appHost.Dispose();    
        }

        void IAssemblyContext.OnAssemblyStart()
        {
            _port = 16000;
            var listeningOn = "http://localhost:" + _port + "/";
            _appHost = new ServiceStackHost();
            _appHost.Init();
            _appHost.Start(listeningOn); 
        }

    }
}