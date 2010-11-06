using EasyHttp;

namespace EasyHttp.Specs.Helpers
{
    public class CustomCodecClass
    {
        public string Name { get; set; }
        public string Email { get; set; }
        [CoDecOptions(IncludeHttpPost = true, IncludeHttpPut = true, DefaultValue = "")]
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}