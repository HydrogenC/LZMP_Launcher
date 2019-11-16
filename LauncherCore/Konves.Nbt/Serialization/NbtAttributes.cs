using System;

namespace Konves.Nbt.Serialization
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtByteAttribute : NbtTagAttribute
	{
		public NbtByteAttribute()
			: base(NbtTagType.Byte) { }

		public NbtByteAttribute(string name)
			: base(NbtTagType.Byte, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtShortAttribute : NbtTagAttribute
	{
		public NbtShortAttribute()
			: base(NbtTagType.Short) { }

		public NbtShortAttribute(string name)
			: base(NbtTagType.Short, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtIntAttribute : NbtTagAttribute
	{
		public NbtIntAttribute()
			: base(NbtTagType.Int) { }

		public NbtIntAttribute(string name)
			: base(NbtTagType.Int, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtLongAttribute : NbtTagAttribute
	{
		public NbtLongAttribute()
			: base(NbtTagType.Long) { }

		public NbtLongAttribute(string name)
			: base(NbtTagType.Long, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtFloatAttribute : NbtTagAttribute
	{
		public NbtFloatAttribute()
			: base(NbtTagType.Float) { }

		public NbtFloatAttribute(string name)
			: base(NbtTagType.Float, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtDoubleAttribute : NbtTagAttribute
	{
		public NbtDoubleAttribute()
			: base(NbtTagType.Double) { }

		public NbtDoubleAttribute(string name)
			: base(NbtTagType.Double, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtByteArrayAttribute : NbtTagAttribute
	{
		public NbtByteArrayAttribute()
			: base(NbtTagType.ByteArray) { }

		public NbtByteArrayAttribute(string name)
			: base(NbtTagType.ByteArray, name) { }
	}
	
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtStringAttribute : NbtTagAttribute
	{
		public NbtStringAttribute()
			: base(NbtTagType.String) { }

		public NbtStringAttribute(string name)
			: base(NbtTagType.String, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtListAttribute : NbtTagAttribute
	{
		public NbtListAttribute(NbtTagType elementType)
			: base(NbtTagType.List)
		{
			m_elementType = elementType;
		}

		public NbtListAttribute(string name, NbtTagType elementType)
			: base(NbtTagType.List, name)
		{
			m_elementType = elementType;
		}

		public NbtTagType ElementType { get { return m_elementType; } }

		readonly NbtTagType m_elementType;
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtCompoundAttribute : NbtTagAttribute
	{
		public NbtCompoundAttribute()
			: base(NbtTagType.Compound) { }

		public NbtCompoundAttribute(string name)
			: base(NbtTagType.Compound, name) { }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class NbtIntArrayAttribute : NbtTagAttribute
	{
		public NbtIntArrayAttribute()
			: base(NbtTagType.IntArray) { }

		public NbtIntArrayAttribute(string name)
			: base(NbtTagType.IntArray, name) { }
	}
}
