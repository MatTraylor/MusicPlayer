using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using MediaPlayer.Media;
using System.Windows.Forms;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private NotifyIcon myNotifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            myNotifyIcon = new NotifyIcon();
            myNotifyIcon.Click += (s, args) => ShowMainWindow();
            myNotifyIcon.Icon = MediaPlayer.Properties.Resources.MusicPlayer;
            myNotifyIcon.Visible = true;

            myNotifyIcon.ContextMenuStrip = new ContextMenuStrip();
            myNotifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, ev) => ShowMainWindow();
            myNotifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, ev) => ExitApplication();

            ShowMainWindow();
        }

        private void ExitApplication()
        {
            MainWindow.Close();
            myNotifyIcon.Dispose();
            myNotifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) => MediaUtils.SavePlaylists();        
    }
}
