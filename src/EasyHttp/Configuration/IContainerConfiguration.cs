using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public interface IContainerConfiguration
    {
        Registry InitializeContainer();
    }
}