using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Nbt.Serialization
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class NbtIgnoreAttribute : Attribute
	{
	}
}
