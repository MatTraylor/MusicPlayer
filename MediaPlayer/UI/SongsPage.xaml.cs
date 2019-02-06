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
        private ObservableCollection<Song> mySongs;

        public SongsPage(ObservableCollection<Song> mediaList, string pageTitle = "Songs")
        {
            InitializeComponent();
            lblSongPageTitle.Content = pageTitle;
            ucSongsList.lvSongs.ItemsSource = mySongs = mediaList;
        }

        public SongsPage(Playlist playlist = null)
        {
            InitializeComponent();
            lblSongPageTitle.Content = playlist.Name;
            ucSongsList.lvSongs.ItemsSource = mySongs = playlist.Songs;
            btnAddSongs.Visibility = Visibility.Visible;
            myPlaylist = playlist;
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textboxText = ((TextBox)sender).Text;
            ucSongsList.lvSongs.ItemsSource = mySongs.Where(x => x.Name.Contains(textboxText, StringComparison.InvariantCultureIgnoreCase)
            || x.Artists.Contains(textboxText, StringComparison.InvariantCultureIgnoreCase)
            || x.Album.Contains(textboxText, StringComparison.InvariantCultureIgnoreCase));
        }

        private void btnAddSongs_Click(object sender, RoutedEventArgs e)
        {
            AddSongs addSongs = new AddSongs(myPlaylist);
            addSongs.ShowDialog();
        }
    }
}
