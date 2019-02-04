using MediaPlayer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace MediaPlayer.Media
{
    static class MediaUtils
    {
        public static MediaElement MediaElement { get; set; }       
        public static Song CurrentSong { get; set; }
        public static bool IsPlaying { get; set; }
        public static bool IsShuffleSelected { get; set; } = false;

        public static ObservableCollection<Song> AllSongs { get; set; } = new ObservableCollection<Song>();
        public static ObservableCollection<Playlist> Playlists { get; set; } = new ObservableCollection<Playlist>();
        public static ObservableCollection<Artist> Artists { get; set; } = new ObservableCollection<Artist>();
        public static ObservableCollection<Album> Albums { get; set; } = new ObservableCollection<Album>();
        public static ObservableCollection<Song> SongsCurrentlySelected { get; set; } = new ObservableCollection<Song>();
        public static ObservableCollection<Song> SongsCurrentlyPlaying { get; set; } = new ObservableCollection<Song>();

        public static int CurrentSongIndex => SongsCurrentlyPlaying.IndexOf(CurrentSong);
        public static bool IsNextEnabled => (SongsCurrentlyPlaying.Count > 1 && IsShuffleSelected) || SongsCurrentlyPlaying.IndexOf(CurrentSong) < SongsCurrentlyPlaying.Count() - 1;
        public static bool IsPreviousEnabled => (SongsCurrentlyPlaying.Count > 1 && IsShuffleSelected) || SongsCurrentlyPlaying.IndexOf(CurrentSong) > 0;

        private static Random ourRandom = new Random();

        public static void Pause()
        {
            MediaElement.Pause();
            MainWindow.Current.faPlayPause.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Play;
            IsPlaying = false;
        }

        public static void Stop()
        {
            MediaElement.Stop();
            CurrentSong = null;
            IsPlaying = false;
            SongsCurrentlyPlaying = null;
            MainWindow.Current.lblCurrentArtists.Content = MainWindow.Current.lblCurrentSong.Content = null;
            MainWindow.Current.faPlayPause.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Play;
        }

        public static void Next()
        {
            if (IsNextEnabled)
            {
                if (IsShuffleSelected)
                {
                    Song nextSong;
                    do
                    {
                        nextSong = SongsCurrentlyPlaying[ourRandom.Next(SongsCurrentlyPlaying.Count)];
                    } while (nextSong == CurrentSong);
                    nextSong.Play();
                }
                else
                {
                    AllSongs[CurrentSongIndex + 1].Play();
                }
            }
        }

        public static void Previous()
        {
            if (IsPreviousEnabled)
            {
                if (IsShuffleSelected)
                {
                    Song nextSong;
                    do
                    {
                        nextSong = SongsCurrentlyPlaying[ourRandom.Next(SongsCurrentlyPlaying.Count)];
                    } while (nextSong == CurrentSong);
                    nextSong.Play();
                }
                else
                {
                    SongsCurrentlyPlaying[CurrentSongIndex - 1].Play();
                }
            }
        }

        public static void LoadAndSortSongs()
        {
            IEnumerable<string> songFiles = Directory.EnumerateFiles(@"C:\Users\mathewtraylor\Downloads", "*.*", SearchOption.AllDirectories)
                .Where(f => f.EndsWith(".mp3", StringComparison.InvariantCultureIgnoreCase) || f.EndsWith(".wav", StringComparison.InvariantCultureIgnoreCase));

            foreach (string filename in songFiles)
            {
                AllSongs.Add(new Song(filename));
            }

            IEnumerable<IGrouping<string, Song>> artists = AllSongs.GroupBy(s => s.Artists);
            foreach (IGrouping<string, Song> artist in artists)
            {
                Artists.Add(new Artist(artist.Key, artist.Count()));
            }

            IEnumerable<IGrouping<string, Song>> albums = AllSongs.GroupBy(s => s.Album);
            foreach (IGrouping<string, Song> album in albums)
            {
                Albums.Add(new Album(album.Key, album.Count()));
            }
        }

        public static void SavePlaylists()
        {
            XElement playlistsXml = new XElement("playlists");
            foreach (Playlist playlist in Playlists)
            {
                XElement playlistXml = new XElement("playlist", new XAttribute("name", playlist.Name));
                playlistsXml.Add(playlistXml);
                foreach (Song song in playlist.Songs)
                {
                    playlistXml.Add(new XElement("song", song.Name));
                }
            }

            playlistsXml.Save("Playlists.xml");
        }

        public static void LoadPlaylists()
        {
            if (File.Exists("Playlists.xml"))
            {
                XElement playlists = XElement.Load("Playlists.xml");
                foreach (XElement playlist in playlists.Elements("playlist"))
                {
                    ObservableCollection<Song> songs = new ObservableCollection<Song>();
                    foreach (XElement song in playlist.Elements("song"))
                    {
                        Song songToAdd = AllSongs.FirstOrDefault(s => s.Name == song.Value);
                        if (songToAdd != null)
                        {
                            songs.Add(songToAdd);
                        }
                    }
                    Playlists.Add(new Playlist(playlist.Attribute("name").Value, songs));
                }
            }
        }

        public static void CreateTimer()
        {
            Timer timer = new Timer((e) => CheckUserInput(), null, TimeSpan.Zero, TimeSpan.FromSeconds(2));
        }
        private static void CheckUserInput()
        {
            TimeSpan spent = TimeSpan.FromMilliseconds(IdleTimeFinder.GetIdleTime());
            if (spent.TotalSeconds > 30)
            {
                MainWindow.Current.Close();
            }
        }
    }
}
