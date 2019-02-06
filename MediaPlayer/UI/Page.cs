using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer.UI
{
    public class Page : UserControl
    {
        public MainWindow CurrentMainWindow => (MainWindow)Window.GetWindow(this);

        public PageFrame Frame => (PageFrame)Parent;
    }
}
