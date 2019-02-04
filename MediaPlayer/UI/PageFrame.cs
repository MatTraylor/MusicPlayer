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

        public ObservableCollection<Page> Pages
        {
            get => (ObservableCollection<Page>)GetValue(PagesProperty);
            set => SetValue(PagesProperty, value);
        }

        public static readonly DependencyProperty PagesProperty = DependencyProperty.Register("Pages", typeof(ObservableCollection<Page>), typeof(PageFrame));

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
