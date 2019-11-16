using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Konves.Nbt.Serialization
{
	internal class SerializationContext : IDisposable
	{
		internal SerializationContext(NbtWriter nbtWriter)
			: this(nbtWriter, new Dictionary<Type, SerializationInfo[]>()) { }

		internal SerializationContext(NbtWriter nbtWriter, Dictionary<Type, SerializationInfo[]> serializationInfoCache)
		{
			m_nbtWriter = nbtWriter;
			m_serializationInfoCache = serializationInfoCache;
		}

		internal void SerializeObject(NbtTagType tagType, string name, object value, NbtTagType? elementType,  bool writeHeader)
		{
			switch (tagType)
			{
				case NbtTagType.Byte:
					m_nbtWriter.Write(name, Convert.ToByte(value), writeHeader);
					break;
				case NbtTagType.ByteArray:
					SerializeByteArray(name, value, writeHeader);
					break;
				case NbtTagType.Compound:
					SerializeCompound(name, value, writeHeader);
					break;
				case NbtTagType.Double:
					m_nbtWriter.Write(name, Convert.ToDouble(value), writeHeader);
					break;
				case NbtTagType.End:
					throw new ArgumentException("tagType End cannot be serialized.", "tagType");
				case NbtTagType.Float:
					m_nbtWriter.Write(name, Convert.ToSingle(value), writeHeader);
					break;
				case NbtTagType.Int:
					m_nbtWriter.Write(name, Convert.ToInt32(value), writeHeader);
					break;
				case NbtTagType.IntArray:
					SerializeIntArray(name, value, writeHeader);
					break;
				case NbtTagType.List:
					SerializeList(name, elementType.Value, value, writeHeader);
					break;
				case NbtTagType.Long:
					m_nbtWriter.Write(name, Convert.ToInt64(value), writeHeader);
					break;
				case NbtTagType.Short:
					m_nbtWriter.Write(name, Convert.ToInt16(value), writeHeader);
					break;
				case NbtTagType.String:
					m_nbtWriter.Write(name, Convert.ToString(value), writeHeader);
					break;
				default:
					break;
			}
		}

		internal void SerializeCompound(string name, object value, bool writeHeader)
		{
			if (writeHeader)
				m_nbtWriter.WriteTagHeader(NbtTagType.Compound, name);

			foreach (SerializationInfo si in GetSerializationInfo(value.GetType()))
				SerializeObject(si.TagType, si.TagName, si.GetValue(value), si.ElementType, writeHeader);

			m_nbtWriter.Write((byte)0x00);
		}

		internal void SerializeByteArray(string name, object value, bool writeHeader)
		{
			byte[] bytes = value as byte[];

			if (bytes == null)
			{
				IEnumerable<byte> data = value as IEnumerable<byte>;

				if (data == null)
					bytes = ConvertToEnumerableOfByte(value as IEnumerable).ToArray();
				else
					bytes = data.ToArray();
			}

			if (bytes != null)
			{
				if(writeHeader)
					m_nbtWriter.WriteTagHeader(NbtTagType.ByteArray, name);

				m_nbtWriter.Write(bytes.Length);
				m_nbtWriter.Write(bytes);
			}
		}

		internal void SerializeIntArray(string name, object value, bool writeHeader)
		{
			int[] ints = value as int[];

			if (ints == null)
			{
				IEnumerable<int> data = value as IEnumerable<int>;

				if (data == null)
					ints = ConvertToEnumerableOfInt(value as IEnumerable).ToArray();
				else
					ints = data.ToArray();
			}

			if (ints != null)
			{
				if (writeHeader)
					m_nbtWriter.WriteTagHeader(NbtTagType.IntArray, name);

				m_nbtWriter.Write(ints.Length);
				foreach (int i in ints)
					m_nbtWriter.Write(i);
			}
		}

		internal void SerializeList(string name, NbtTagType elementType, object value, bool writeHeader)
		{
			object[] objects = value as object[];

			if (objects == null)
			{
				IEnumerable data = (value as IEnumerable);

				if(data != null)
					objects = data.Cast<object>().ToArray();
			}

			if (objects != null)
			{
				if (writeHeader)
					m_nbtWriter.WriteTagHeader(NbtTagType.List, name);

				m_nbtWriter.Write((byte)elementType);

				m_nbtWriter.Write((int)objects.Length);

				foreach (object obj in objects)
					SerializeObject(elementType, null, obj, null, false); // TODO: This may cause a bug with nested lists: eg. SomeType[][]
			}
		}

		internal static IEnumerable<byte> ConvertToEnumerableOfByte(IEnumerable value)
		{
			if (value == null)
				yield break;

			foreach (object obj in value)
				yield return Convert.ToByte(obj);
		}

		internal static IEnumerable<int> ConvertToEnumerableOfInt(IEnumerable value)
		{
			if (value == null)
				yield break;

			foreach (object obj in value)
				yield return Convert.ToInt32(obj);
		}
		
		internal SerializationInfo[] GetSerializationInfo(Type type)
		{
			SerializationInfo[] si;
			if (m_serializationInfoCache.TryGetValue(type, out si))
				return si;

			si = SerializationInfo.GetSerializationInfo(type);

			m_serializationInfoCache.Add(type, si);

			return si;
		}

		internal Dictionary<Type, SerializationInfo[]> SerializationInfoCache { get { return m_serializationInfoCache; } }

		void IDisposable.Dispose()
		{
			(m_nbtWriter as IDisposable).Dispose();
		}

		readonly NbtWriter m_nbtWriter;
		readonly Dictionary<Type, SerializationInfo[]> m_serializationInfoCache;
	}
}
