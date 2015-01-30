using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace GraphDiffClient
{
	internal class NullToEmptyListResolver : DefaultContractResolver
	{
		protected override IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			var provider = base.CreateMemberValueProvider(member);

			if (member.MemberType != MemberTypes.Property) return provider;

			var propType = ((PropertyInfo) member).PropertyType;
			if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof (List<string>))
				return new EmptyListValueProvider(provider, propType);

			return provider;
		}
	}
}