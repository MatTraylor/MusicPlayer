using MediaPlayer.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer.UI
{
    /// <summary>
    /// Interaction logic for SongList.xaml
    /// </summary>
    public partial class SongList : UserControl
    {
        public ObservableCollection<Song> MediaList { get; set; }

        public SongList()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Song song = (Song)((Button)sender).DataContext;
            song.Play(true);
        }

        private void ChangeSelectiong()
        {
            if (lvSongs.Items.Contains(MediaUtils.CurrentSong))
            {
                lvSongs.SelectedItem = MediaUtils.CurrentSong;
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {

            IEnumerable<Song> itemSource = (IEnumerable<Song>)lvSongs.ItemsSource;
            lvSongs.ItemsSource = itemSource.ToList().OrderBy(x => x.Name);
        }
        
    }
}
