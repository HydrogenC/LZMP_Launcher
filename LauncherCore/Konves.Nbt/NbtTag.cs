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
	/// Provides base functionality for representing a named value.
	/// </summary>
	public abstract class NbtTag
	{
		/// <summary>
		/// Initializes a new instance of an <see cref="NbtTag"/>.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The type.</param>
		internal NbtTag(string name, NbtTagType type)
		{
			m_name = name;
			m_type = type;
		}

		/// <summary>
		/// Gets the tag type.
		/// </summary>
		public NbtTagType Type
		{
			get { return m_type; }
		}

		/// <summary>
		/// Gets or sets the tag name.
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// Gets a <see cref="System.Object"/> representing the value of this tag when implemented in a derived class.
		/// </summary>
		public abstract object GetValue();

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("{0} {1}: {2}", Type, Name, GetValue());
		}

		readonly NbtTagType m_type;
		string m_name;
	}

	/// <summary>
	/// Provides base functionality for representing a named value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in this tag.</typeparam>
	public abstract class NbtTag<T> : NbtTag
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NbtTag{T}"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="type">The tag type.</param>
		/// <param name="value">The value.</param>
		internal NbtTag(string name, NbtTagType type, T value)
			: base(name, type)
		{
			m_value = value;
		}

		/// <summary>
		/// Gets or sets the tag value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public T Value
		{
			get { return m_value; }
			set { m_value = value; }
		}

		/// <summary>
		/// Gets a <see cref="System.Object" /> representing the value of this tag.
		/// </summary>
		/// <returns></returns>
		public override object GetValue()
		{
			return m_value;
		}

		T m_value;
	}
}
