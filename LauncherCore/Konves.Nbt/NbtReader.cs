/*
  Copyright 2014 Steve Konves

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License. 
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Konves.Nbt
{
	/// <summary>
	/// Represents a reader that provides fast, noncached, forward-only access to NBT data.
	/// </summary>
	public sealed class NbtReader : IDisposable
	{
		/// <summary>
		/// Initializes a new <see cref="NbtReader" /> instance using the specified stream.
		/// </summary>
		/// <param name="stream">The stream containing NBT data.</param>
		/// <exception cref="System.ArgumentException">The stream does not support reading, the stream is <c>null</c>, or the stream is already closed.</exception>
		public NbtReader(Stream stream)
		{
			try
			{
				m_binaryReader = new BinaryReader(stream);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("The stream does not support reading, the stream is null, or the stream is already closed.", "stream");
			}
		}

		/// <summary>
		/// Initializes a new <see cref="NbtReader" /> instance using the specified <see cref="System.IO.BinaryReader">BinaryReader</see>.
		/// </summary>
		/// <param name="binaryReader">The <see cref="System.IO.BinaryReader">BinaryReader</see> from which to read NBT data.</param>
		/// <exception cref="System.ArgumentException">The underlying stream does not support reading.</exception>
		public NbtReader(BinaryReader binaryReader)
		{
			if (!binaryReader.BaseStream.CanRead)
			{
				throw new ArgumentException("The underlying stream does not support reading.");
			}

			m_binaryReader = binaryReader;
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtTagInfo" /> object.
		/// </summary>
		/// <returns>
		/// The content as an <see cref="NbtTagInfo" /> object.
		/// </returns>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		/// <exception cref="System.FormatException">Value is not a valid tag type.</exception>
		public NbtTagInfo ReadTagInfo()
		{
			byte[] data = new byte[3];

			data[0] = m_binaryReader.ReadByte();

			if (data[0] == 0)
				return (NbtTagInfo)new byte[] { 0x00 };

			if (data[0] > (byte)NbtTagType.IntArray)
			{
				if (m_binaryReader.BaseStream.CanSeek)
					throw new FormatException(string.Format("Value {0:X} at position {1} is not a valid tag type.", data[0], m_binaryReader.BaseStream.Position));
				else
					throw new FormatException(string.Format("Value {0:X} is not a valid tag type.", data[0]));
			}

			m_binaryReader.Read(data, 1, 2);

			return (NbtTagInfo)data;
		}
		
		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtTag"/> object of the specified type.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentOutOfRangeException"><paramref name="tagInfo"/> does not specify a valid tag type.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtTag ReadTag(NbtTagInfo tagInfo)
		{
			switch (tagInfo.Type)
			{
				case NbtTagType.Byte:
					return ReadByte(tagInfo);
				case NbtTagType.Short:
					return ReadShort(tagInfo);
				case NbtTagType.Int:
					return ReadInt(tagInfo);
				case NbtTagType.Long:
					return ReadLong(tagInfo);
				case NbtTagType.Float:
					return ReadFloat(tagInfo);
				case NbtTagType.Double:
					return ReadDouble(tagInfo);
				case NbtTagType.ByteArray:
					return ReadByteArray(tagInfo);
				case NbtTagType.String:
					return ReadString(tagInfo);
				case NbtTagType.List:
					return ReadList(tagInfo);
				case NbtTagType.Compound:
					return ReadCompound(tagInfo);
				case NbtTagType.IntArray:
					return ReadIntArray(tagInfo);
				default:
					throw new ArgumentOutOfRangeException("tagInfo", tagInfo, "tagInfo does not specify a valid tag type.");
			}
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtByte"/> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>The content as an <see cref="NbtByte"/> object.</returns>
		/// <exception cref="System.ArgumentException"><paramref name="tagInfo"/> does not describe a short tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtByte ReadByte(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Byte)
				throw new ArgumentException("tagInfo does not describe a byte tag.","tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			byte value = ReadNumberData<byte>()[0];
			return new NbtByte(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtShort" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtShort" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException"><paramref name="tagInfo"/> does not describe a short tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtShort ReadShort(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Short)
				throw new ArgumentException("tagInfo does not describe a short tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			short value = BitConverter.ToInt16(ReadNumberData<short>(), 0);
			return new NbtShort(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtInt" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtInt" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe an int tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtInt ReadInt(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Int)
				throw new ArgumentException("tagInfo does not describe an int tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			int value = BitConverter.ToInt32(ReadNumberData<int>(), 0);
			return new NbtInt(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtInt" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtInt" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a long tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtLong ReadLong(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Long)
				throw new ArgumentException("tagInfo does not describe a long tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			long value = BitConverter.ToInt64(ReadNumberData<long>(), 0);
			return new NbtLong(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtFloat" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtFloat" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a float tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtFloat ReadFloat(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Float)
				throw new ArgumentException("tagInfo does not describe a float tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			float value = BitConverter.ToSingle(ReadNumberData<float>(), 0);
			return new NbtFloat(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtDouble" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtDouble" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a double tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtDouble ReadDouble(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Double)
				throw new ArgumentException("tagInfo does not describe a double tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			double value = BitConverter.ToDouble(ReadNumberData<double>(), 0);
			return new NbtDouble(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtByteArray" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtByteArray" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a byte array tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtByteArray ReadByteArray(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.ByteArray)
				throw new ArgumentException("tagInfo does not describe a byte array tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			int size = BitConverter.ToInt32(ReadNumberData<int>(), 0);
			byte[] value = m_binaryReader.ReadBytes(size);

			return new NbtByteArray(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtString" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtString" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a string tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtString ReadString(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.String)
				throw new ArgumentException("tagInfo does not describe a string tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);

			byte[] lengthData = m_binaryReader.ReadBytes(2);

			if (BitConverter.IsLittleEndian)
				Array.Reverse(lengthData);

			short length = BitConverter.ToInt16(lengthData, 0);

			string value = ReadStringData(length);

			return new NbtString(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtList" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtList" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a list tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtList ReadList(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.List)
				throw new ArgumentException("tagInfo does not describe a list tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			NbtTagType type = (NbtTagType)m_binaryReader.ReadByte();
			NbtTagInfo childTagInfo = (NbtTagInfo)(new byte[] { (byte)type, 0x00, 0x00 });
			int size = BitConverter.ToInt32(ReadNumberData<int>(), 0);

			NbtTag[] value = new NbtTag[size];

			for (int i = 0; i < size; i++)
				value[i] = ReadTag(childTagInfo);

			return new NbtList(name, type, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtCompound" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtCompound" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a compound tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtCompound ReadCompound(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.Compound)
				throw new ArgumentException("tagInfo does not describe a compound tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);

			NbtTagCollection value = new NbtTagCollection();

			NbtTagInfo childTagInfo;

			while (true)
			{
				childTagInfo = ReadTagInfo();

				if (childTagInfo.Type == NbtTagType.End)
					break;

				value.Add(ReadTag(childTagInfo));
			}

			return new NbtCompound(name, value);
		}

		/// <summary>
		/// Reads binary content at the current position as an <see cref="NbtIntArray" /> object.
		/// </summary>
		/// <param name="tagInfo">The tag information.</param>
		/// <returns>
		/// The content as an <see cref="NbtIntArray" /> object.
		/// </returns>
		/// <exception cref="System.ArgumentException">tagInfo does not describe a int array tag.</exception>
		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public NbtIntArray ReadIntArray(NbtTagInfo tagInfo)
		{
			if (tagInfo.Type != NbtTagType.IntArray)
				throw new ArgumentException("tagInfo does not describe a int array tag.", "tagInfo");

			string name = ReadStringData(tagInfo.NameLength);
			int size = BitConverter.ToInt32(ReadNumberData<int>(), 0);
			int[] value = new int[size];

			for (int i = 0; i < size; i++)
				value[i] = BitConverter.ToInt32(ReadNumberData<int>(), 0);

			return new NbtIntArray(name, value);
		}

		/// <summary>
		/// Exposes access to the underlying stream of the <see cref="NbtReader" />.
		/// </summary>
		public Stream BaseStream { get { return m_binaryReader.BaseStream; } }
		
		/// <summary>
		/// Gets a value that indicates whether the current stream position is at the end of the stream.
		/// </summary>
		/// <value>
		///   <c>true</c> if the current stream position is at the end of the stream; otherwise <c>false</c>.
		/// </value>
		/// <exception cref="System.NotSupportedException">The underlying stream does not support seeking.</exception>
		public bool EndOfStream
		{
			get
			{
				if (!m_binaryReader.BaseStream.CanSeek)
					throw new NotSupportedException("The underlying stream does not support seeking.");

				return m_binaryReader.PeekChar() == -1;
			}
		}

		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		byte[] ReadNumberData<T>() where T : struct
		{
			int length = Marshal.SizeOf(typeof(T));

			byte[] data = m_binaryReader.ReadBytes(Marshal.SizeOf(typeof(T)));

			if (data.Length != length)
				throw new System.IO.EndOfStreamException("The end of the stream is reached.");

			if (BitConverter.IsLittleEndian)
				Array.Reverse(data);

			return data;
		}

		/// <exception cref="System.IO.EndOfStreamException">The end of the stream is reached.</exception>
		string ReadStringData(int length)
		{
			string name = length == 0 ? string.Empty : Encoding.UTF8.GetString(m_binaryReader.ReadBytes(length));

			if (name.Length != length)
				throw new System.IO.EndOfStreamException("The end of the stream is reached");

			return name;
		}

		readonly BinaryReader m_binaryReader;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			(m_binaryReader as IDisposable).Dispose();
		}
	}
}
