using System.Collections.Generic;
using System.Dynamic;
using EasyHttp.Infrastructure;

namespace EasyHttp.Codecs
{
    public class DynamicType: DynamicObject
    {
        //readonly IMemberBehavior _memberBehavior;

        //protected DynamicType(IMemberBehavior memberBehavior)
        //{
        //    _memberBehavior = memberBehavior;
        //}

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