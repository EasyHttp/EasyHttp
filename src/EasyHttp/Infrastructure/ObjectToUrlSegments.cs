using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EasyHttp.Infrastructure
{
    public class ObjectToUrlSegments
    {
        public string ParametersToUrl(object parameters)
        {
            var returnuri = "";
            var properties = GetProperties((parameters));
            if (parameters != null)
            {
                var paramsList = properties.Select(prop => System.Web.HttpUtility.UrlEncode(prop.Value)).ToList();
                if (paramsList.Count > 0)  returnuri = String.Format("/", String.Join("/", paramsList));
            }
            return returnuri;
        }

        private IEnumerable<PropertyValue> GetProperties(object parameters)
        {
            if (parameters != null)
            {
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
        }

        private class PropertyValue
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}