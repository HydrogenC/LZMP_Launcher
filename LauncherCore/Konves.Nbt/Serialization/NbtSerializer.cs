using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Konves.Nbt.Serialization
{
	public class NbtSerializer : IDisposable
	{
		public NbtSerializer(Type type)
		{
			m_type = type;
		}

		public object Deserialize(Stream stream)
		{
			return Deserialize(new NbtReader(stream));
		}

		public object Deserialize(BinaryReader binaryReader)
		{
			return Deserialize(new NbtReader(binaryReader));
		}

		public object Deserialize(NbtReader nbtReader)
		{
			NbtTagInfo tagInfo = nbtReader.ReadTagInfo();
			if (tagInfo.Type != NbtTagType.Compound)
				throw new FormatException("the deserialized stream must contain root tag which is compound.");

			NbtCompound root = nbtReader.ReadCompound(tagInfo);

			return DeserializeTag(root, m_type);
		}

		public void Serialize(Stream stream, object o)
		{
			Serialize(new NbtWriter(stream), o);
		}

		public void Serialize(BinaryWriter binaryWriter, object o)
		{
			Serialize(new NbtWriter(binaryWriter), o);
		}

		public void Serialize(NbtWriter nbtWriter, object o)
		{
			SerializationContext ctx = new SerializationContext(nbtWriter, m_serializationInfoCache);

			ctx.SerializeObject(NbtTagType.Compound, string.Empty, o, null, true);
		}

		private static object DeserializeTag(NbtTag tag, Type type)
		{
			switch (tag.Type)
			{
				case NbtTagType.Byte:
				case NbtTagType.Double:
				case NbtTagType.Float:
				case NbtTagType.Int:
				case NbtTagType.Long:
				case NbtTagType.Short:
				case NbtTagType.String:
					return Convert.ChangeType(tag.GetValue(), type);
				case NbtTagType.ByteArray:
					return DeserializeArray((tag as NbtIntArray).Value.Cast<object>().ToArray(), type);
				case NbtTagType.Compound:
					return DeserializeCompound(tag as NbtCompound, type);
				case NbtTagType.IntArray:
					return DeserializeArray((tag as NbtIntArray).Value.Cast<object>().ToArray(), type);
				case NbtTagType.List:
					return DeserializeList((tag as NbtList).Value, type);
				default:
					throw new ArgumentOutOfRangeException();
			}

			throw new NotImplementedException();
		}

		private static object DeserializeCompound(NbtCompound tag, Type type)
		{
			object obj = type.Assembly.CreateInstance(type.FullName);

			var data = GetTypeData(type);

			foreach (NbtTag t in tag.Value)
			{
				PropertyInfo pi;

				if (data.TryGetValue(t.Name, out pi))
					pi.SetValue(obj, DeserializeTag(t, pi.PropertyType), null);
			}

			return obj;
		}

		private static object DeserializeList(NbtTag[] tags, Type type)
		{
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();

				Array array = Array.CreateInstance(elementType, tags.Length);

				for (int i = 0; i < tags.Length; i++)
					array.SetValue(DeserializeTag(tags[i], elementType), i);

				return array;
			}

			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				Type elementType = type.GetGenericArguments()[0];

				MethodInfo mi = type.GetMethod("Add", new Type[] { elementType });

				if (mi == null)
					throw new ArgumentException("Tags cannot be deserialized to the specified type", "type");

				IEnumerable obj = (IEnumerable)Activator.CreateInstance(type);

				foreach (NbtTag tag in tags)
				{
					mi.Invoke(obj, new object[] { DeserializeTag(tag, elementType) });
				}

				return obj;
			}

			throw new ArgumentException("Tags cannot be deserialized to the specified type", "type");
		}

		private static object DeserializeArray(object[] values, Type type)
		{
			if (type.IsArray)
			{
				Type elementType = type.GetElementType();

				Array array = Array.CreateInstance(elementType, values.Length);

				for (int i = 0; i < values.Length; i++)
					array.SetValue(Convert.ChangeType(values[i], elementType), i);

				return (object[])array;
			}

			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				Type elementType = type.GetGenericArguments()[0];

				MethodInfo mi = type.GetMethod("Add", new Type[] { elementType });

				if (mi == null)
					throw new ArgumentException("Tags cannot be deserialized to the specified type", "type");

				IEnumerable obj = (IEnumerable)Activator.CreateInstance(type);

				foreach (object value in values)
					mi.Invoke(obj, new object[] { Convert.ChangeType(value, elementType) });

				return obj;
			}

			throw new ArgumentException("Tags cannot be deserialized to the specified type", "type");
		}
		
		private static Dictionary<string, PropertyInfo> GetTypeData(Type type)
		{
			return
				type.GetProperties()
				.Select(pi => new { PropertyInfo = pi, Attributes = pi.GetCustomAttributes(true) })
				.Where(g => !g.Attributes.Select(a => a.GetType().AssemblyQualifiedName).Contains(typeof(NbtIgnoreAttribute).AssemblyQualifiedName))
				.Select(x => new { x.PropertyInfo, Attribute = x.Attributes.FirstOrDefault(a => typeof(NbtTagAttribute).IsAssignableFrom(a.GetType())) })
				.ToDictionary(x => x.Attribute == null || !(x.Attribute as NbtTagAttribute).HasName ? x.PropertyInfo.Name : (x.Attribute as NbtTagAttribute).Name, x => x.PropertyInfo);
		}

		readonly Type m_type;

		private Dictionary<Type, SerializationInfo[]> m_serializationInfoCache = new Dictionary<Type,SerializationInfo[]>();

		void IDisposable.Dispose()
		{
			m_serializationInfoCache = null;
		}
	}
}
