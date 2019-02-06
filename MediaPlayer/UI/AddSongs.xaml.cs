using MediaPlayer.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayer.UI
{
    public partial class AddSongs : Window
    {
        private List<Song> mySongsToAdd = new List<Song>();
        private Playlist myPlaylist;

        public AddSongs(Playlist playlist)
        {
            myPlaylist = playlist;

            InitializeComponent();
            lvSongs.ItemsSource = MediaUtils.AllSongs.Where(s => !playlist.Songs.Contains(s));
        }

        private void chkSingleSong_StateChanged(object sender, RoutedEventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;
            if ((bool)chkBox.IsChecked)
            {
                mySongsToAdd.Add((Song)chkBox.DataContext);
            }
            else
            {
                mySongsToAdd.Remove((Song)chkBox.DataContext);
            }
        }

        private void btnCancel_OnClick(object sender, RoutedEventArgs e) =>Close();        

        private void btnAdd_OnClick(object sender, RoutedEventArgs e) => AddSongsToPlaylist();        

        private void btnAddAllSongs_OnClick(object sender, RoutedEventArgs e)
        {
            mySongsToAdd.AddRange((IEnumerable<Song>)lvSongs.ItemsSource);
            AddSongsToPlaylist();
        }

        private void AddSongsToPlaylist()
        {
            foreach (Song song in mySongsToAdd)
            {
                myPlaylist.Songs.Add(song);
            }
            Close();
        }
    }
}
