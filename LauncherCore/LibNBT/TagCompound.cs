using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LibNBT
{
    public class TagCompound : AbstractTag
    {
        private Dictionary<string, AbstractTag> _dictionary;

        public override TagType Type
        {
            get { return TagType.Compound; }
        }

        public override void Write(Stream output)
        {
            output.WriteByte((byte)Type);
            TagString.WriteString(output, Name);
            TagCompound.WriteCompound(output, this);
        }

        private static void WriteCompound(Stream output, TagCompound tagCompound)
        {
            var enumerator = tagCompound._dictionary.GetEnumerator();

            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.Write(output);
            }

            TagEnd.Singleton.Write(output);
        }

        public TagCompound()
        {
            Name = string.Empty;
            _dictionary = new Dictionary<string, AbstractTag>();
        }

        public TagCompound(Stream input)
        {
            Name = TagString.ReadString(input);
            _dictionary = ReadDictionary(input);
        }

        internal static Dictionary<string, AbstractTag> ReadDictionary(Stream input)
        {
            Dictionary<string, AbstractTag> dict = new Dictionary<string, AbstractTag>();
            AbstractTag tag = AbstractTag.Read(input);
            while (tag.Type != TagType.End)
            {
                dict[tag.Name] = tag;
                tag = AbstractTag.Read(input);
            }

            return dict;
        }

        internal static TagCompound ReadUnnamedTagCompound(Stream input)
        {
            return new TagCompound() { _dictionary = ReadDictionary(input) };
        }

        public void SetTag(string name, AbstractTag tag)
        {
            tag.Name = name;
            _dictionary[name] = tag;
        }

        public TagByte GetByte(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagByte;
            }
            else
            {
                return null;
            }
        }

        public TagByteArray GetByteArray(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagByteArray;
            }
            else
            {
                return null;
            }
        }

        public TagCompound GetCompound(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagCompound;
            }
            else
            {
                return null;
            }
        }

        public TagDouble GetDouble(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagDouble;
            }
            else
            {
                return null;
            }
        }

        public TagFloat GetFloat(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagFloat;
            }
            else
            {
                return null;
            }
        }

        public TagInt GetInt(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagInt;
            }
            else
            {
                return null;
            }
        }

        public TagIntArray GetIntArray(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagIntArray;
            }
            else
            {
                return null;
            }
        }

        public TagList GetList(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagList;
            }
            else
            {
                return null;
            }
        }

        public TagLong GetLong(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagLong;
            }
            else
            {
                return null;
            }
        }

        public TagShort GetShort(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagShort;
            }
            else
            {
                return null;
            }
        }

        public TagString GetString(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name] as TagString;
            }
            else
            {
                return null;
            }
        }

        public AbstractTag GetAbstractTag(string name)
        {
            if (_dictionary.ContainsKey(name))
            {
                return _dictionary[name];
            }
            else
            {
                return null;
            }
        }

        public void WriteToFile(string filename)
        {
            using (FileStream output = File.Open(filename, FileMode.Create))
            {
                using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress))
                {
                    Write(gzipStream);
                }
            }
        }

        public override void WriteUnnamed(Stream output)
        {
            WriteCompound(output, this);
        }

        public override string ToString(string indentString)
        {
            StringBuilder sb = new StringBuilder();

            if (_dictionary.Keys.Count == 0)
            {
                return string.Format("{0}[Compound: {1}]", indentString, Name);
            }

            sb.AppendLine(string.Format("{0}[Compound: {1}", indentString, Name));

            foreach (string key in _dictionary.Keys)
            {
                sb.AppendLine(string.Format("{0}  {1}={2}", indentString, key, _dictionary[key].ToString(indentString + "  ").Trim()));
            }

            sb.AppendLine(string.Format("{0}]", indentString));

            return sb.ToString();
        }
    }
}
