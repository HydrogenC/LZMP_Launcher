using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Konves.Nbt.Serialization
{
	internal class SerializationInfo
	{
		internal SerializationInfo(NbtTagType tagType, string tagName, Action<object, object, object[]> setMethod, Func<object, object[], object> getMethod, NbtTagType? elementType)
		{
			m_tagType = tagType;
			m_tagName = tagName;

			m_setMethod = setMethod;
			m_getMethod = getMethod;

			m_elementType = elementType;
		}

		internal string TagName { get { return m_tagName; } }

		internal NbtTagType TagType { get { return m_tagType; } }

		internal NbtTagType? ElementType { get { return m_elementType; } }

		internal object GetValue(object obj)
		{
			return m_getMethod.Invoke(obj, null);
		}
		internal void SetValue(object obj, object value)
		{
			m_setMethod.Invoke(obj, value, null);
		}

		internal static SerializationInfo[] GetSerializationInfo(Type type)
		{
			return _getSerializationInfo(type).ToArray();
		}

		private static IEnumerable<SerializationInfo> _getSerializationInfo(Type type)
		{
			foreach (PropertyInfo pi in type.GetProperties())
			{
				object[] attributes = pi.GetCustomAttributes(true);

				if (!attributes.Any(a => typeof(NbtIgnoreAttribute).IsAssignableFrom(a.GetType())))
				{
					NbtTagAttribute tagAttribute = attributes.FirstOrDefault(a => typeof(NbtTagAttribute).IsAssignableFrom(a.GetType())) as NbtTagAttribute;

					if (tagAttribute == null)
					{
						NbtTagType tagType;
						NbtTagType? elementType;
						if (TryGetNbtTagType(pi.PropertyType, out tagType, out elementType))
							yield return new SerializationInfo(tagType, pi.Name, pi.SetValue, pi.GetValue, elementType);
					}
					else
					{
						NbtListAttribute listAttribute = tagAttribute as NbtListAttribute;
						yield return new SerializationInfo(tagAttribute.Type, tagAttribute.HasName ? tagAttribute.Name : pi.Name, pi.SetValue, pi.GetValue, listAttribute == null ? null : (NbtTagType?)listAttribute.ElementType);
					}
				}
			}
		}

		private static bool TryGetNbtTagType(Type type, out NbtTagType tagType, out NbtTagType? elementType)
		{
			elementType = null;

			if (type == typeof(byte))
				tagType = NbtTagType.Byte;
			else if (type == typeof(short))
				tagType = NbtTagType.Short;
			else if (type == typeof(int))
				tagType = NbtTagType.Int;
			else if (type == typeof(long))
				tagType = NbtTagType.Long;
			else if (type == typeof(float))
				tagType = NbtTagType.Float;
			else if (type == typeof(double))
				tagType = NbtTagType.Double;
			else if (type == typeof(byte[]) || typeof(IEnumerable<byte>).IsAssignableFrom(type))
				tagType = NbtTagType.ByteArray;
			else if (type == typeof(string))
				tagType = NbtTagType.String;
			else if (type == typeof(int[]) || typeof(IEnumerable<int>).IsAssignableFrom(type))
				tagType = NbtTagType.IntArray;

			else if (type.IsArray)
			{
				NbtTagType t;
				TryGetNbtTagType(type.GetElementType(), out t, out elementType);
				elementType = t;
				tagType = NbtTagType.List;
			}
			else if (typeof(IEnumerable).IsAssignableFrom(type) && type.IsGenericType)
			{
				NbtTagType t;
				TryGetNbtTagType(type.GetGenericArguments().First(), out t, out elementType);
				elementType = t;
				tagType = NbtTagType.List;
			}

			else
			{
				tagType = NbtTagType.Compound;
			}

			return true;
		}

		readonly NbtTagType m_tagType;
		readonly NbtTagType? m_elementType;
		readonly string m_tagName;
		readonly Action<object, object, object[]> m_setMethod;
		readonly Func<object, object[], object> m_getMethod;
	}
}
