using System;

namespace EasyHttp
{
    public class CoDecOptions : Attribute
    {
        public bool IncludeHttpPost { get; set; }
        public bool IncludeHttpPut { get; set; }
        public object DefaultValue { get; set; }
    }
}