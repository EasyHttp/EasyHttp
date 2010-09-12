using System.Collections.Generic;
using System.Dynamic;

namespace EasyHttp
{
    public class Body: DynamicObject
    {
        readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public string RawText { get; set; }

       
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _properties[binder.Name.ToLower()];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name.ToLower()] = value;
            return true;
        }
    }
}