using System.Collections.Generic;
using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public interface IConfiguration
    {
        IList<Registry> Registries { get; }
    }
}