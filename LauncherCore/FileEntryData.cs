using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherCore
{
    public record class FileEntryData
    {
        public string FileName { get; set; }
        public string CurseforgeKey { get; set; }
        public string CurseforgeId { get; set; }
    }
}
