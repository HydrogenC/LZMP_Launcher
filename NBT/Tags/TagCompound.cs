using System;
using System.IO;
using System.Linq;
using System.Text;
using NBT.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace NBT.Tags
{
    public sealed class TagCompound : Tag, IEnumerable<KeyValuePair<string, Tag>>, IEnumerable, IEquatable<TagCompound>
    {
        private Dictionary<string, Tag> value;

        public TagCompound()
        {
            this.value = new Dictionary<string, Tag>();
        }

        internal TagCompound(Stream stream) : this()
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.readTag(stream);
        }

        public TagCompound(Dictionary<string, Tag> value) : base()
        {
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.value = new Dictionary<string, Tag>(value);
        }

        public TagCompound(IEnumerable<KeyValuePair<string, Tag>> values) : this()
        {
            if (values == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            foreach (KeyValuePair<string, Tag> valueItem in values)
            {
                this.value.Add(valueItem.Key, valueItem.Value);
            }
        }

        public override byte tagID
        {
            get
            {
                return TagTypes.TagCompound;
            }
        }

        public override string toString()
        {
            return "";
        }

        internal override void readTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            this.Clear();
            bool exit = false;
            while (exit != true)
            {
                byte id = TagByte.ReadByte(stream);
                if (id == TagTypes.TagEnd)
                {
                    exit = true;
                }
                if (exit != true)
                {
                    string tagEntry_Key = TagString.ReadString(stream);
                    Tag tagEntry_Value = Tag.ReadTag(stream, id);
                    this.value.Add(tagEntry_Key, tagEntry_Value);
                }
            }
        }

        internal override void writeTag(Stream stream)
        {
            if (stream == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            foreach (KeyValuePair<string, Tag> keyValue in this.value) 
            {
                //Escribimos el identificador de la etiqueta
                stream.WriteByte(keyValue.Value.tagID);
                //Escribimos el nombre de la etiqueta
                TagString.WriteString(stream, keyValue.Key);
                //Escribimos el contenido de la etiqueta
                keyValue.Value.writeTag(stream);
            }
            //Escribimos la etiqueta de cierre
            stream.WriteByte(TagTypes.TagEnd);
        }

        public int Count
        {
            get
            {
                return this.value.Count;
            }
        }

        public IEnumerator<KeyValuePair<string, Tag>> GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        public override object Clone()
        {
            TagCompound result = new TagCompound();

            foreach (KeyValuePair<string, Tag> value in this.value) 
            {
                result.Add(value.Key, (Tag)value.Value.Clone());
            }

            return result;
        }

        public ICollection<Tag> Values
        {
            get
            {
                return this.value.Values;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return this.value.Keys;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.value.GetEnumerator();
        }

        public void Add(string key, Tag value)
        {
            if (key == null)
            {
                throw new NBT_InvalidArgumentNullException("key");
            }
            if (value == null)
            {
                throw new NBT_InvalidArgumentNullException("value");
            }
            if (this.Contains(key) == true)
            {
                throw new NBT_InvalidArgumentException("This key already exist");
            }
            this.value.Add(key, value);
        }

        public void AddRange(IEnumerable<KeyValuePair<string, Tag>> items)
        {
            if (items == null)
            {
                throw new NBT_InvalidArgumentNullException();
            }
            foreach (KeyValuePair<string, Tag> pair in items)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        public void Clear()
        {
            this.value.Clear();
        }

        public bool Contains(string key)
        {
            return this.value.ContainsKey(key);
        }
        
        public bool Remove(string key)
        {
            if (this.Contains(key) == false) 
            {
                throw new NBT_InvalidArgumentException();
            }
            return this.value.Remove(key);
        }

        public void Rename(string oldKeyName, string newKeyName)
        {
            if (this.Contains(oldKeyName) == false)
            {
                throw new NBT_InvalidArgumentException("oldKeyName");
            }
            if (this.Contains(newKeyName) == true)
            {
                throw new NBT_InvalidArgumentException("newKeyName");
            }
            Tag tag = this[oldKeyName];
            this.Remove(oldKeyName);
            this.Add(newKeyName, tag);
        }

        public override Tag this[string key]
        {
            get
            {
                if (this.Contains(key) == false)
                {
                    throw new NBT_InvalidArgumentException();
                }
                return this.value[key];
            }
            set
            {
                if (key == null)
                {
                    throw new NBT_InvalidArgumentNullException("key");
                }
                if (value == null)
                {
                    throw new NBT_InvalidArgumentNullException("value");
                }
                if (this.Contains(key) == true)
                {
                    this.value[key] = value;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        public static explicit operator TagCompound(Dictionary<string, Tag> value)
        {
            return new TagCompound(value);
        }
        public override Type getType()
        {
            return typeof(TagCompound);
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value == null)
                {
                    throw new NBT_InvalidArgumentNullException();
                }
                if (value.GetType() != typeof(Dictionary<string, Tag>))
                {
                    throw new NBT_InvalidArgumentException();
                }
                this.value.Clear();
                this.value = new Dictionary<string, Tag>((Dictionary<string, Tag>)value);
            }
        }

        public bool Equals(TagCompound other)
        {
            bool bResult = false;
            bool exitFor = false;
            try
            {
                if (this.value.Keys.Count == other.value.Keys.Count)
                {
                    foreach (KeyValuePair<string, Tag> kvp in this.value) 
                    {
                        Tag tmpValue;
                        if (other.value.TryGetValue(kvp.Key, out tmpValue) == false)
                        {
                            exitFor = true;
                        }
                        else
                        {
                            exitFor = !kvp.Value.Equals(tmpValue);
                        }
                        if (exitFor == true)
                        {
                            break;
                        }
                    }
                    bResult = (exitFor == false);
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

            if (typeof(TagCompound) != other.getType())
            {
                bResult = false;
            }
            else
            {
                bResult = this.Equals((TagCompound)other);
            }

            return bResult;
        }
    }
}
