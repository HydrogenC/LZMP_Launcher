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
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Konves.Nbt
{
	/// <summary>
	/// Provides functionality for writing named binary tags to a stream.
	/// </summary>
	public sealed class NbtWriter : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NbtWriter"/> class based on the supplied stream.
		/// </summary>
		/// <param name="output">The output stream.</param>
		/// <exception cref="System.ArgumentException">The stream does not support writing, or the stream is already closed.</exception>
		/// <exception cref="System.ArgumentNullException"><paramref name="output"/> is <c>null</c>.</exception>
		public NbtWriter(Stream output)
		{
			if (output == null)
				throw new ArgumentNullException("output", "output is null.");

			m_binaryWriter = new BinaryWriter(output);			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NbtWriter"/> class based on the supplied <see cref="System.IO.BinaryWriter">BinaryWriter</see> object.
		/// </summary>
		/// <param name="binaryWriter">The binary writer.</param>
		/// <exception cref="System.ArgumentException">The underlying stream does not support writing, or is already closed.</exception>
		/// <exception cref="System.ArgumentNullException">binaryWriter is null.</exception>
		public NbtWriter(BinaryWriter binaryWriter)
		{
			if (binaryWriter == null)
				throw new ArgumentNullException("binaryWriter", "binaryWriter is null.");

			if (!binaryWriter.BaseStream.CanWrite)
				throw new ArgumentException("The underlying stream does not support writing, or is already closed.", "binaryWriter");

			m_binaryWriter = binaryWriter;
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtTag tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		void Write(NbtTag tag, bool writeHeader)
		{
			switch (tag.Type)
			{
				case NbtTagType.Byte:
					Write((NbtByte)tag, writeHeader);
					break;
				case NbtTagType.Short:
					Write((NbtShort)tag, writeHeader);
					break;
				case NbtTagType.Int:
					Write((NbtInt)tag, writeHeader);
					break;
				case NbtTagType.Long:
					Write((NbtLong)tag, writeHeader);
					break;
				case NbtTagType.Float:
					Write((NbtFloat)tag, writeHeader);
					break;
				case NbtTagType.Double:
					Write((NbtDouble)tag, writeHeader);
					break;
				case NbtTagType.ByteArray:
					Write((NbtByteArray)tag, writeHeader);
					break;
				case NbtTagType.String:
					Write((NbtString)tag, writeHeader);
					break;
				case NbtTagType.List:
					Write((NbtList)tag, writeHeader);
					break;
				case NbtTagType.Compound:
					Write((NbtCompound)tag, writeHeader);
					break;
				case NbtTagType.IntArray:
					Write((NbtIntArray)tag, writeHeader);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtByte tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, byte value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtByte tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, byte value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Byte, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtShort tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, short value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtShort tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, short value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Short, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtInt tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, int value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtInt tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, int value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Int, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtLong tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, long value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtLong tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, long value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Long, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtFloat tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, float value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtFloat tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, float value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Float, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtDouble tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, double value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtDouble tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, double value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Double, name);
			Write(value);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtByteArray tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, byte[] value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtByteArray tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, byte[] value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.ByteArray, name);
			if (value == null)
			{
				Write((int)0);
			}
			else
			{
				Write((int)value.Length);
				Write(value);
			}
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtString tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and value.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, string value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtString tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, string value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.String, name);

			if (string.IsNullOrEmpty(value))
			{
				Write((short)0);
			}
			else
			{
				Write((short)value.Length);
				Write(value);
			}
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtList tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and array of values.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="elementType">Type of the element in <paramref name="value"/>.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ArgumentException"><paramref name="value"/> contains elements of a type other than that specified by <paramref name="elementType"/>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, NbtTagType elementType, NbtTag[] value)
		{
			Write(name, elementType, value, true);
		}
		internal void Write(NbtList tag, bool writeHeader)
		{
			Write(tag.Name, tag.ElementType, tag.Value, writeHeader);
		}
		internal void Write(string name, NbtTagType elementType, NbtTag[] value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.List, name);

			Write((byte)elementType);

			if (value == null)
			{
				Write((int)0);
			}
			else
			{
				Write((int)value.Length);

				foreach (var element in value)
					Write(element, false);
			}
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtCompound tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and array of <see cref="NbtTag">NbtTags</see>.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The array of <see cref="NbtTag">NbtTags</see>.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, IEnumerable<NbtTag> value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtCompound tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, IEnumerable<NbtTag> value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.Compound, name);

			if (value != null)
				foreach (var element in value)
					Write(element);

			Write((byte)0x00);
		}

		/// <summary>
		/// Writes out the specified tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="tag"/> is <c>null</c>.</exception>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(NbtIntArray tag)
		{
			if (tag == null)
				throw new ArgumentNullException("tag", "tag is null.");

			Write(tag, true);
		}
		/// <summary>
		/// Writes out a tag containing the specified name and array of integers.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="value">The array of integers.</param>
		/// <exception cref="System.ObjectDisposedException">The stream is closed.</exception>
		/// <exception cref="System.IO.IOException">An I/O error occured.</exception>
		public void Write(string name, int[] value)
		{
			Write(name, value, true);
		}
		internal void Write(NbtIntArray tag, bool writeHeader)
		{
			Write(tag.Name, tag.Value, writeHeader);
		}
		internal void Write(string name, int[] value, bool writeHeader)
		{
			if (writeHeader)
				WriteTagHeader(NbtTagType.IntArray, name);
			Write(value == null ? 0 : (int)value.Length);
			foreach (int element in value)
				Write(element);
		}

		internal void Write(byte value)
		{
			m_binaryWriter.Write(value);
		}

		void Write(short value)
		{
			WriteAsBigEndian(BitConverter.GetBytes(value));
		}

		internal void Write(int value)
		{
			WriteAsBigEndian(BitConverter.GetBytes(value));
		}

		void Write(long value)
		{
			WriteAsBigEndian(BitConverter.GetBytes(value));
		}

		void Write(float value)
		{
			WriteAsBigEndian(BitConverter.GetBytes(value));
		}

		void Write(double value)
		{
			WriteAsBigEndian(BitConverter.GetBytes(value));
		}

		internal void Write(byte[] value)
		{
			m_binaryWriter.Write(value);
		}

		void Write(string value)
		{
			m_binaryWriter.Write(Encoding.UTF8.GetBytes(value));
		}

		internal void WriteTagHeader(NbtTagType tagType, string name)
		{
			Write((byte)tagType);
			Write((short)name.Length);
			if (!string.IsNullOrEmpty(name))
				Write(name);
		}

		void WriteTagHeader(NbtTag tag)
		{
			WriteTagHeader(tag.Type, tag.Name);
		}

		void WriteAsBigEndian(byte[] data)
		{
			if (BitConverter.IsLittleEndian)
				Array.Reverse(data);
			m_binaryWriter.Write(data);
		}

		readonly BinaryWriter m_binaryWriter;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		void IDisposable.Dispose()
		{
			//(m_binaryWriter as IDisposable).Dispose();
		}
	}
}
