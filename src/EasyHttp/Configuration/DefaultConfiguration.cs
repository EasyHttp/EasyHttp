using System.Collections.Generic;
using StructureMap.Configuration.DSL;

namespace EasyHttp.Configuration
{
    public class DefaultConfiguration : IConfiguration
    {
        public IList<Registry> Registries { get; private set; }

        public DefaultConfiguration()
        {
            Registries = new List<Registry>();

            Registries.Add(new CodecRegistry());
        }
    }
}