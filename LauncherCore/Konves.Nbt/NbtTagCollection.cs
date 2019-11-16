using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Konves.Nbt
{
	public class NbtTagCollection : IList<NbtTag>
	{
		public int IndexOf(NbtTag item)
		{
			return m_data.IndexOf(item);
		}

		public void Insert(int index, NbtTag item)
		{
			m_data.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			RemoveAt(index);
		}

		public NbtTag this[int index]
		{
			get
			{
				return m_data[index];
			}
			set
			{
				m_data[index] = value;
			}
		}

		public NbtTag this[string name]
		{
			get
			{
				try
				{
					return m_data.SingleOrDefault(t => t.Name == name);
				}
				catch (InvalidOperationException ex)
				{
					throw new ArgumentException("name is not unique", "name", ex);
				}
			}
			set
			{
				m_data[m_data.IndexOf(this[name])] = value;
			}
		}

		public void Add(NbtTag item)
		{
			m_data.Add(item);
		}

		public void AddRange(IEnumerable<NbtTag> collection)
		{
			m_data.AddRange(collection);
		}

		public void Clear()
		{
			m_data.Clear();
		}

		public bool Contains(NbtTag item)
		{
			return m_data.Contains(item);
		}

		public void CopyTo(NbtTag[] array, int arrayIndex)
		{
			m_data.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return m_data.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(NbtTag item)
		{
			return m_data.Remove(item);
		}

		public IEnumerator<NbtTag> GetEnumerator()
		{
			return m_data.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_data.GetEnumerator();
		}

		private readonly List<NbtTag> m_data = new List<NbtTag>();
	}
}
