using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Web.Script.Serialization;

namespace EasyHttp
{
    public class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            return new DynamicJsonObject(dictionary);

        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return new DynamicJsonArray(obj);
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(DynamicObject), typeof(ExpandoObject) })); }
        }
    }

    public class DynamicJsonArray : Dictionary<string, object>
    {
        
        public DynamicJsonArray(object obj)
        {
            throw new NotImplementedException();
        }
    }
}