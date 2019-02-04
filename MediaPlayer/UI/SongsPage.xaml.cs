using MediaPlayer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaPlayer.UI
{
    /// <summary>
    /// Interaction logic for SongsPage.xaml
    /// </summary>
    public partial class SongsPage : Page
    {
        private Playlist myPlaylist;

        public SongsPage(ObservableCollection<Song> mediaList, string pageTitle = "Songs")
        {
            InitializeComponent();
            lblSongPageTitle.Content = pageTitle;
            ucSongsList.lvSongs.ItemsSource = mediaList;
        }

        public SongsPage(Playlist playlist = null)
        {
            InitializeComponent();
            lblSongPageTitle.Content = playlist.Name;
            ucSongsList.lvSongs.ItemsSource = playlist.Songs;
            btnAddSongs.Visibility = Visibility.Visible;
            myPlaylist = playlist;            
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IEnumerable<Song> songs = (IEnumerable<Song>)ucSongsList.lvSongs.ItemsSource;
            ucSongsList.lvSongs.ItemsSource = songs.Where(x => x.Name.Contains(((TextBox)sender).Text, StringComparison.InvariantCultureIgnoreCase));
        }

        private void btnAddSongs_Click(object sender, RoutedEventArgs e)
        {
            AddSongs addSongs = new AddSongs(myPlaylist);
            addSongs.ShowDialog();
        }
    }
}
