using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Text;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp
{
    public class Body: DynamicObject
    {
        readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

     
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_properties.ContainsKey(binder.Name.ToLower()))
            {
                result = _properties[binder.Name.ToLower()];
                return true;
            }
            throw new PropertyNotFoundException(binder.Name);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name.ToLower()] = value;
            return true;
        }

      
    }
}