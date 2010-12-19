using StructureMap;
using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public interface IConfigurationStep
    {
        void Execute(Registry registry);
    }

}