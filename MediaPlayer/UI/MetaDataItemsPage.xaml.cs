using MediaPlayer.Media;
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
    /// Interaction logic for ArtistsPage.xaml
    /// </summary>
    public partial class MetaDataItemsPage : Page
    {
        public MetaDataItemsPage(ObservableCollection<Artist> mediaList)
        {
            InitializeComponent();
            lvMetaDataItem.ItemsSource = mediaList;
            lblPageTitle.Content = "Artists";
        }

        public MetaDataItemsPage(ObservableCollection<Album> mediaList)
        {
            InitializeComponent();
            lvMetaDataItem.ItemsSource = mediaList;
            lblPageTitle.Content = "Albums";
        }

        private void lvPlaylistsItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MetaDataItem metaDataItem = (MetaDataItem)((ListViewItem)sender).DataContext;
            ObservableCollection<Song> songs = new ObservableCollection<Song>();
            foreach (Song song in MediaUtils.AllSongs.Where(s => s.Artists.Equals(metaDataItem.Name)))
            {
                songs.Add(song);
            }
            Frame.CurrentPage = new SongsPage(songs, metaDataItem.Name);
            MediaUtils.SongsCurrentlySelected = songs;
        }

        private void txtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IEnumerable<MetaDataItem> metaDataItems = (IEnumerable<MetaDataItem>)lvMetaDataItem.ItemsSource;
            lvMetaDataItem.ItemsSource = metaDataItems.Where(x => x.Name.Contains(((TextBox)sender).Text, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
