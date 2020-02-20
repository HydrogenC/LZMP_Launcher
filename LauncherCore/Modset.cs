using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LauncherCore
{
    public class Modset : IEditable
    {
        public Modset(ref Dictionary<string, Mod> dict)
        {
            foreach (var i in dict)
            {
                pairs.Add(i.Key, i.Value.ToInstall);
                foreach (var j in i.Value.Addons)
                {
                    pairs.Add(j.Key, j.Value.ToInstall);
                }
            }
        }

        public Modset(ref Dictionary<string, bool> dict)
        {
            pairs = dict;
        }

        private Dictionary<string, bool> pairs = new Dictionary<string, bool>();

        /// <summary>
        /// Get the to install state
        /// </summary>
        /// <param name="key">Mod key</param>
        /// <returns>To install state</returns>
        public bool this[string key]
        {
            get
            {
                return pairs[key];
            }
            set
            {
                pairs[key] = value;
            }
        }

        public void Apply()
        {
            foreach (var i in pairs)
            {
                Mod target;
                if (Mod.FindMod(ref SharedData.Mods, i.Key, out target))
                {
                    target.ToInstall = i.Value;
                }
            }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void ExportTo(string dest)
        {
            throw new NotImplementedException();
        }

        public override void Rename(string newName, bool type)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
