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
using System.Linq;
using System.Text;

namespace Konves.Nbt
{
	/// <summary>
	/// Specifies the type of data contained within an <see cref="NbtTag"/>.
	/// </summary>
	public enum NbtTagType
	{
		/// <summary>Specifies the end of an <see cref="NbtCompound">Compound</see> tag.</summary>
		End = 0x00,
		/// <summary>Specifies an 8-bit unsigned integer.</summary>
		Byte = 0x01,
		/// <summary>Specifies a 16-bit signed integer.</summary>
		Short = 0x02,
		/// <summary>Specifies a 32-bit signed integer.</summary>
		Int = 0x03,
		/// <summary>Specifies a 64-bit signed integer.</summary>
		Long = 0x04,
		/// <summary>Specifies a single-precision floating-point number.</summary>
		Float = 0x05,
		/// <summary>Specifies a double-precision floating-point number.</summary>
		Double = 0x06,
		/// <summary>Specifies an array of 8-bit unsigned integers.</summary>
		ByteArray = 0x07,
		/// <summary>Specifies text as a series of UTF-8 characters.</summary>
		String = 0x08,
		/// <summary>Specifies list of <see cref="NbtTag">Tags</see> of a single type.</summary>
		List = 0x09,
		/// <summary>Specifies list of <see cref="NbtTag">Tags</see> of multiple types.</summary>
		Compound = 0x0A,
		/// <summary>Specifies an array of 32-bit signed integers.</summary>
		IntArray = 0x0B
	}
}
