using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Media
{
    public class MetaDataItem
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public MetaDataItem(string name, int count)
        {
            Name = name;
            Count = count;
        }
    }

    public class Artist : MetaDataItem
    {
        public Artist(string name, int count) : base(name, count) { }
    }

    public class Album : MetaDataItem
    {
        public Album(string name, int count) : base(name, count) { }
    }
}
