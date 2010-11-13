using StructureMap;

namespace EasyHttp
{
    public static class BootStrapper
    {
        public static void InitStructureMap()
        {
                
               
            ObjectFactory.Initialize(
                x => {
                         x.For<ICodec>().Use<DefaultCodec>();
                });
        }
    }
}