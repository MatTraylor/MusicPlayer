using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public class Playlist
    {
        public string Name { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        public Playlist(string name, ObservableCollection<Song> songs)
        {
            Name = name;
            Songs = songs;
        }
    }
}
