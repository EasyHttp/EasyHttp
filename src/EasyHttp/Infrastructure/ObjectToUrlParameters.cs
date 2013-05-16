using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace EasyHttp.Infrastructure
{
    public class ObjectToUrlParameters
    {
        public string ParametersToUrl(object parameters)
        {
            var returnuri = "";
            var properties = GetProperties((parameters));
            if (parameters != null)
            {
                var paramsList = properties.Select(prop => String.Join("=", prop.Name, System.Web.HttpUtility.UrlEncode(prop.Value))).ToList();
                if (paramsList.Count > 0)  returnuri = String.Format("?{0}", String.Join("&", paramsList));
            }
            return returnuri;
        }

        private IEnumerable<PropertyValue> GetProperties(object parameters)
        {
            if (parameters == null) yield break;
            if (parameters is ExpandoObject)
            {
                var dictionary = parameters as IDictionary<string, object>;
                foreach (var property in dictionary)
                {
                    yield return new PropertyValue { Name = property.Key, Value = property.Value.ToString() };
                }
            }

            var properties = TypeDescriptor.GetProperties(parameters);
            foreach (PropertyDescriptor propertyDescriptor in properties)
            {
                var val = propertyDescriptor.GetValue(parameters);
                if (val != null)
                {
                    yield return new PropertyValue { Name = propertyDescriptor.Name, Value = val.ToString() };
                }
            }
        }

        private class PropertyValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}