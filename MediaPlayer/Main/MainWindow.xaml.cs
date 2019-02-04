using MediaPlayer.Media;
using MediaPlayer.UI;
using MediaPlayer.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<LibraryItem> LibraryItems { get; set; }
        public static MainWindow Current { get; set; }
        public string TimerContents { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        

        public MainWindow()
        {
            Current = this;
            LibraryItems = new ObservableCollection<LibraryItem>()
            {
                new LibraryItem(LibraryItem.LibraryItemName.Songs),
                new LibraryItem(LibraryItem.LibraryItemName.Artists),
                new LibraryItem(LibraryItem.LibraryItemName.Albums)
            };

            MediaUtils.LoadAndSortSongs();
            MediaUtils.LoadPlaylists();
            MediaUtils.SongsCurrentlySelected = MediaUtils.AllSongs;

            DataContext = this;
            InitializeComponent();

            MediaUtils.MediaElement = mePlayer;
            lvPlaylists.ItemsSource = MediaUtils.Playlists;
            pageFrame.CurrentPage = new SongsPage(MediaUtils.AllSongs);
            faPlayPause.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Play;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += CheckUserInput;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void btnPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (MediaUtils.IsPlaying)
            {
                MediaUtils.Pause();
            }
            else
            {
                if (MediaUtils.CurrentSong == null)
                {
                    MediaUtils.SongsCurrentlySelected.First().Play(true);
                }
                else
                {
                    MediaUtils.CurrentSong.Play();
                }
            }         
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) => MediaUtils.Stop();

        private void mePlayer_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (MediaUtils.IsNextEnabled)
            {
                MediaUtils.Next();
            }
            else
            {
                MediaUtils.Stop();
            }
        }

        private void lvPlaylistsItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lvLibraryItems.SelectedItem = null;

            Playlist selectedPlaylist = (Playlist)((System.Windows.Controls.ListViewItem)sender).DataContext;
            pageFrame.CurrentPage = new SongsPage(selectedPlaylist);
            MediaUtils.SongsCurrentlySelected = selectedPlaylist.Songs;
        }

        private void lvLibraryItemsItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lvPlaylists.SelectedItem = null;

            LibraryItem item = (LibraryItem)((System.Windows.Controls.ListViewItem)sender).DataContext;
            switch (item.Name)
            {
                case LibraryItem.LibraryItemName.Songs:
                    pageFrame.CurrentPage = new SongsPage(MediaUtils.AllSongs);
                    MediaUtils.SongsCurrentlySelected = MediaUtils.AllSongs;
                    break;
                case LibraryItem.LibraryItemName.Artists:
                    pageFrame.CurrentPage = new MetaDataItemsPage(MediaUtils.Artists);
                    break;
                case LibraryItem.LibraryItemName.Albums:
                    pageFrame.CurrentPage = new MetaDataItemsPage(MediaUtils.Albums);
                    break;
            }
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            lblAddPlaylist.Visibility = Visibility.Hidden;
            txtPlaylistName.Visibility = Visibility.Visible;
            txtPlaylistName.Focus();
        }

        private void txtPlaylistName_LostFocus(object sender, RoutedEventArgs e) =>AddNewPlaylist();   
        private void txtPlaylistName_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) AddNewPlaylist();
        }

        private void AddNewPlaylist()
        {
            if (!string.IsNullOrEmpty(txtPlaylistName.Text)) MediaUtils.Playlists.Add(new Playlist(txtPlaylistName.Text, null));
            txtPlaylistName.Text = "";
            lblAddPlaylist.Visibility = Visibility.Visible;
            txtPlaylistName.Visibility = Visibility.Hidden;
        }        

        private void btnNext_Click(object sender, RoutedEventArgs e) => MediaUtils.Next();

        private void btnPrevious_Click(object sender, RoutedEventArgs e) => MediaUtils.Previous();

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            if (MediaUtils.IsShuffleSelected)
            {
                MediaUtils.IsShuffleSelected = false;
                Current.faShuffle.Foreground = Brushes.Red;
            }
            else
            {
                MediaUtils.IsShuffleSelected = true;
                Current.faShuffle.Foreground = Brushes.Green;
            }
        }

        private void CheckUserInput(object sender, EventArgs e)
        {
            TimeSpan spent = TimeSpan.FromMilliseconds(IdleTimeFinder.GetIdleTime());
            if (spent.TotalSeconds > 30)
            {
                Close();
            }
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(MediaUtils.MediaElement != null) MediaUtils.MediaElement.Volume = volumeSlider.Value;
        }

        private TimeSpan myTotalTime;
        private bool mySetSliderValue = true;
        private void Element_MediaOpened(object sender, RoutedEventArgs e)
        {
            myTotalTime = MediaUtils.MediaElement.NaturalDuration.TimeSpan;
            timelineSlider.Maximum = MediaUtils.MediaElement.NaturalDuration.TimeSpan.TotalSeconds;

            DispatcherTimer timerVideoTime = new DispatcherTimer(){ Interval = TimeSpan.FromSeconds(1) };
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            timerVideoTime.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (myTotalTime.TotalSeconds > 0)
            {
                lblTimer.Content = $"{string.Format("{0:00}:{1:00}", MediaUtils.MediaElement.Position.Minutes, MediaUtils.MediaElement.Position.Seconds)}/{string.Format("{0:00}:{1:00}", myTotalTime.Minutes, myTotalTime.Seconds)}";
                if (mySetSliderValue) timelineSlider.Value = MediaUtils.MediaElement.Position.TotalSeconds;
            }            
        }

        private void timelineSlider_DragFinished(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0, (int)timelineSlider.Value);
            MediaUtils.MediaElement.Position = ts;  
            mySetSliderValue = true;
        }

        private void timelineSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mySetSliderValue = false;
        }
    }

    public class LibraryItem
    {
        public LibraryItemName Name { get; set; }

        public LibraryItem(LibraryItemName name) => Name = name;        

        public enum LibraryItemName
        {
            Songs,
            Artists,
            Albums
        }
    }
}
