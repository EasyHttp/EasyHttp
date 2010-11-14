using System.Collections.Generic;
using System.Reflection;
using JsonFx.Json.Resolvers;
using JsonFx.Serialization;

namespace EasyHttp.Codecs.JsonFXExtensions
{
    public class RemoveAmerpsandFromNameJsonResolverStrategy: JsonResolverStrategy
    {
        public override IEnumerable<DataName> GetName(MemberInfo member)
        {
            if (member.Name.StartsWith("@"))
            {
                string nameWithoutAmpersand = member.Name.Remove(0);

                return new List<DataName> {new DataName(nameWithoutAmpersand)};
            }
            return base.GetName(member);
        }
    }
}