using System;
using Newtonsoft.Json.Serialization;

namespace GraphDiffClient
{
	internal class EmptyListValueProvider : IValueProvider
	{
		private readonly IValueProvider _innerProvider;
		private readonly object _defaultValue;

		public EmptyListValueProvider(IValueProvider innerProvider, Type listType)
		{
			_innerProvider = innerProvider;
			_defaultValue = Activator.CreateInstance(listType);
		}

		public void SetValue(object target, object value)
		{
			_innerProvider.SetValue(target, value ?? _defaultValue);
		}

		public object GetValue(object target)
		{
			return _innerProvider.GetValue(target) ?? _defaultValue;
		}
	}
}