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

using System.Linq;

namespace Konves.Nbt
{
	/// <summary>
	/// Represents a named binary tag whose value is an array of named binary tags of a single type.
	/// </summary>
	public sealed class NbtList : NbtTag<NbtTag[]>
	{
		/// <summary>
		/// Initializes a new instance of an <see cref="NbtList"/> tag.
		/// </summary>
		/// <param name="name">The tag name.</param>
		/// <param name="elementType">Type of the element in <paramref name="value"/>.</param>
		/// <param name="value">The value.</param>
		/// <exception cref="System.ArgumentException"><paramref name="value"/> contains elements of a type other than that specified by <paramref name="elementType"/>.</exception>
		public NbtList(string name, NbtTagType elementType, NbtTag[] value)
			: base(name, NbtTagType.List, value)
		{
			if (value.Any(v => v.Type != elementType))
				throw new System.ArgumentException("value contains elements of a type other than that specified by elementType", "value");

			m_elementType = elementType;
		}

		/// <summary>
		/// Gets the type of the elements contains within this tag's value.
		/// </summary>
		/// <value>
		/// The element type.
		/// </value>
		public NbtTagType ElementType { get { return m_elementType; } }

		readonly NbtTagType m_elementType;
	}
}
