using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayer.UI
{
    public class PageFrame : ContentControl
    {
        public MainWindow CurrentMainWindow => (MainWindow)Window.GetWindow(this);

        private Page myCurrentPage;
        public Page CurrentPage
        {
            get => myCurrentPage;
            set
            {
                if (myCurrentPage != value)
                {
                    myCurrentPage = value;
                    Content = myCurrentPage;
                }
            }
        }
    }
}
