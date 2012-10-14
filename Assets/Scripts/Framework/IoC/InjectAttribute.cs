using System;

namespace IoC
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class InjectAttribute: Attribute
	{
	}
}
