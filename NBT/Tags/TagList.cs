using System;
using System.IO;
using System.Linq;
using NBT.Exceptions;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace NBT.Tags
{
    public sealed class TagList : Tag, IList<Tag>, ICollection<Tag>, IEnumerable<Tag>, IEnumerable, IEquatable<TagList>
    {
        private List<Tag> value;
        private byte typeOfList;

        public TagList(byte idTagType)
        {
            this.value = new List<Tag>();
            this.typeOfList = idTagType;
        }

        internal TagList(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = new List<Tag>();
            this.readTag(stream);
        }

        public override Tag this[int index]
        {
            get
            {
                return this.value[index];
            }
            set
            {
                if (value == null)
                {
                    throw new NBT_InvalidArgumentNullException();
                }
                if (value.tagID != this.typeOfList)
                {
                    throw new NBT_InvalidArgumentException("TagType doesn't match.");
                }
                this.value[index] = value;
            }
        }

        public byte Type
        {
            get
            {
                return this.typeOfList;
            }
            set
            {
                if (this.Count > 0)
                {
                    throw new NBT_InvalidArgumentException("Clear the TagList before changing its TagType.");
                }
                this.typeOfList = value;
            }
        }

        public void Add(Tag item)
        {
            if (item == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            if (item.tagID != this.typeOfList)
            {
                throw new NBT_InvalidArgumentException("TagType doesn't match.");
            }
            if (this.value.Count == int.MaxValue)
            {
                throw new NBT_Exception("List is in the limit.");
            }
            this.value.Add(item);
        }

        public void AddRange(IEnumerable<Tag> items)
        {
            foreach (Tag tag in items)
            {
                this.Add(tag);
            }
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        public void Clear()
        {
            this.value.Clear();
        }

        public bool Contains(Tag item)
        {
            return this.value.Contains(item);
        }

        public void CopyTo(Tag[] array, int arrayIndex)
        {
            this.value.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get 
            {
                return this.value.Count;
            }
        }

        public bool IsReadOnly
        {
            get 
            {
                return false;
            }
        }

        public bool Remove(Tag item)
        {
            return this.value.Remove(item);
        }

        public int IndexOf(Tag item)
        {
            return this.value.IndexOf(item);
        }

        public void Insert(int index, Tag item)
        {
            if (item == null)
            {
                throw new NBT_InvalidArgumentNullException("item");
            }
            if (item.tagID != this.typeOfList)
            {
                throw new NBT_InvalidArgumentException("TagType doesn't match.");
            }
            this.value.Insert(index, item);
        }

        public void InsertRange(int index, IEnumerable<Tag> items)
        {
            foreach (Tag tag in items)
            {
                this.Add(tag);
                index++;
            }
        }

        public void Move(Tag item, int index)
        {
            this.Remove(item);
            this.Insert(index, item);
        }

        public void Move(int fromIndex, int toIndex)
        {
            Tag item = this[fromIndex];
            this.Remove(item);
            this.Insert(toIndex, item);
        }

        public void RemoveAt(int index)
        {
            this.value.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            this.value.RemoveRange(index, count);
        }

        public void Reverse()
        {
            this.value.Reverse();
        }

        public void Reverse(int index, int count)
        {
            this.value.Reverse(index, count);
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagList;
            }
        }

        public override string toString()
        {
            return "";
        }

        internal override void readTag(Stream stream)
        {
            this.Clear();
            this.typeOfList = TagByte.ReadByte(stream);
            int count = TagInt.ReadInt(stream);
            this.value.Capacity = count;
            for (int i = 0; i < count; i++)
            {
                Tag item = Tag.ReadTag(stream, this.typeOfList);
                if (item == null)
                {
                    throw new NBT_Exception("Unexpected TagEnd.");
                }
                this.Add(item);
            }
        }

        internal override void writeTag(Stream stream)
        {
            TagByte.WriteByte(stream, this.typeOfList);
            TagInt.WriteInt(stream, this.Count);
            foreach (Tag tag in this)
            {
                tag.writeTag(stream);
            }
        }

        public override object Clone()
        {
            TagList list = new TagList(this.typeOfList);
            foreach (Tag tag in this)
            {
                list.Add((Tag)tag.Clone());
            }
            return list;
        }

        public static TagList operator +(TagList list, Tag tag)
        {
            list.Add(tag);
            return list;
        }

        public override Type getType()
        {
            return typeof(TagList);
        }

        public string getNamedTypeOfList()
        {
            return Tag.GetNamedTypeFromId(this.typeOfList);
        }

        public bool Equals(TagList other)
        {
            bool bResult = false;
            try
            {
                if (this.Type == other.Type)
                {
                    bResult = this.value.SequenceEqual(other.value);
                }
            }
            catch (ArgumentNullException nullEx)
            {
                throw new NBT_InvalidArgumentNullException(nullEx.Message, nullEx.InnerException);
            }
            catch (Exception ex)
            {
                throw new NBT_InvalidArgumentException(ex.Message, ex.InnerException);
            }
            return bResult;
        }

        public override bool Equals(Tag other)
        {
            bool bResult = true;

            if (typeof(TagList) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagList)other);
            }

            return bResult;
        }
    }
}
