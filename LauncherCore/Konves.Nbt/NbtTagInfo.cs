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
using System.Diagnostics;

namespace Konves.Nbt
{
	/// <summary>
	/// Represents information describing an <see cref="NbtTag">NbtTag's</see> type and name length.
	/// </summary>
	[DebuggerDisplay("{Type}")]
	public struct NbtTagInfo
	{
		private NbtTagInfo(byte[] data)
		{
			m_data = data.Length == 3 ? (data[0] << 16) + (data[1] << 8) + data[2] : 0;			
		}

		/// <summary>
		/// Gets the tag type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public NbtTagType Type { get { return (NbtTagType)(m_data >> 16); } }

		/// <summary>
		/// Gets the length of the tag name.
		/// </summary>
		/// <value>
		/// The length of the tag name.
		/// </value>
		public int NameLength { get { return m_data & 0x00FFFF; } }

		/// <summary>
		/// Casts the specified <see cref="NbtTagInfo"/> object as a byte array.
		/// </summary>
		/// <param name="info">The <see cref="NbtTagInfo"/> object.</param>
		/// <returns>A byte array.</returns>
		public static explicit operator byte[](NbtTagInfo info)
		{
			if (info.Type == NbtTagType.End)
				return new byte[] { 0x00 };

			return new[] { (byte)(info.m_data / 0x010000), (byte)(info.m_data & 0x00FF00 / 0x000100), (byte)(info.m_data & 0x0000FF) };
		}

		/// <summary>
		/// Casts the specified byte array info as an <see cref="NbtTagInfo"/> object.
		/// </summary>
		/// <param name="byteArray">The byte array.</param>
		/// <returns>An <see cref="NbtTagInfo"/> object</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="byteArray"/> is null</exception>
		/// <exception cref="System.ArgumentException"><paramref name="byteArray"/> is not 1 or 3 bytes in length.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException"><paramref name="byteArray"/> does not specify a valid tag type.</exception>
		public static explicit operator NbtTagInfo(byte[] byteArray)
		{
			if (byteArray == null)
				throw new ArgumentNullException("byteArray", "byteArray is null");

			if (!((byteArray.Length == 3 && byteArray[0] != 0) || (byteArray.Length == 1 && byteArray[0] == 0)))
				throw new ArgumentException("byteArray is not 3 bytes in length or a single-byte end tag.", "data");

			if (byteArray[0] > (byte)NbtTagType.IntArray)
				throw new ArgumentOutOfRangeException("byteArray", "byteArray does not specify a valid tag type.");

			return new NbtTagInfo(byteArray);
		}

		readonly int m_data;
	}
}
