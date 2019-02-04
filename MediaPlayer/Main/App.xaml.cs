using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using MediaPlayer.Media;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon myNotifyIcon;
        private bool myIsExit;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            myNotifyIcon = new System.Windows.Forms.NotifyIcon();
            myNotifyIcon.Click += (s, args) => ShowMainWindow();
            myNotifyIcon.Icon = MediaPlayer.Properties.Resources.MusicPlayer;
            myNotifyIcon.Visible = true;

            CreateContextMenu();
            ShowMainWindow();
        }

        private void CreateContextMenu()
        {
            myNotifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            myNotifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            myNotifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            myIsExit = true;
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

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!myIsExit)
            {               
                e.Cancel = true;
                MainWindow.Hide();
                MediaUtils.SavePlaylists();
            }
        }
    }
}
