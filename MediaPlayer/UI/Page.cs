using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer.UI
{
    public class Page : UserControl, INotifyPropertyChanged
    {
        public MainWindow CurrentMainWindow => (MainWindow)Window.GetWindow(this);

        public PageFrame Frame => (PageFrame)Parent;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
