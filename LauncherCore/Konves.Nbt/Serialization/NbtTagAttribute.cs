using System;

namespace Konves.Nbt.Serialization
{
	public abstract class NbtTagAttribute : Attribute
	{
		protected NbtTagAttribute(NbtTagType type)
			: this(type, null) { }

		protected NbtTagAttribute(NbtTagType type, string name)
		{
			m_type = type;
			m_name = name;
			m_hasName = !string.IsNullOrEmpty(name);
		}

		public NbtTagType Type { get { return m_type; } }

		public string Name { get { return m_name; } }

		public bool HasName { get { return m_hasName; } }

		readonly NbtTagType m_type;
		readonly string m_name;
		readonly bool m_hasName;
	}
}