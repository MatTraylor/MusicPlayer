﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using MediaPlayer.Media;
using System.Diagnostics;

namespace MediaPlayer
{
    public class Song
    {
        public string Name { get; private set; }
        public string Artists { get; private set; }
        public string Album { get; private set; }
        public string Filename { get; private set; }

        public int ID { get; set; }

        public Song(string filename)
        {
            Filename = filename;
            TagLib.File fileMetaData = TagLib.File.Create(filename);
            try
            {
                Name = fileMetaData.Tag.Title;
                string artits = String.Join(", ", fileMetaData.Tag.Performers);
                Artists = String.IsNullOrEmpty(artits) ? "Unkown Artists" : artits;
                Album = String.IsNullOrEmpty(fileMetaData.Tag.Album) ? "Unknown Album" : fileMetaData.Tag.Album;
            }
            catch (Exception)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry($"Failed to gather meta data information information on file '{filename}'", EventLogEntryType.Warning);
                }
            }

            if (String.IsNullOrEmpty(Name)) Name = filename.Contains("\\") ? filename.Substring(filename.LastIndexOf("\\") + 1) : filename;
        }

        public void Play(bool setCurrentPlayingSongs = false)
        {
            if (MediaUtils.CurrentSong == null || (!MediaUtils.CurrentSong.Equals(this) || MediaUtils.SongsCurrentlyPlaying != MediaUtils.SongsCurrentlySelected)) MediaUtils.MediaElement.Source = new Uri(Filename);
            if (setCurrentPlayingSongs) MediaUtils.SongsCurrentlyPlaying = MediaUtils.SongsCurrentlySelected;
            MediaUtils.CurrentSong = this;
            MainWindow.Current.faPlayPause.Icon = FontAwesome5.EFontAwesomeIcon.Solid_Pause;
            MainWindow.Current.lblCurrentArtists.Content = MediaUtils.CurrentSong.Artists;
            MainWindow.Current.lblCurrentSong.Content = MediaUtils.CurrentSong.Name;
            MediaUtils.IsPlaying = true;
            MediaUtils.MediaElement.Play();
        }
    }
}
