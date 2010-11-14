using JsonFx.Serialization.Providers;
using StructureMap;

namespace EasyHttp.Configuration
{
    public static class BootStrapper
    {
        public static void InitStructureMap()
        {
            ObjectFactory.Initialize(
                x =>
                {
                    x.AddRegistry<CodecRegistry>();
                });

        }
    }
}