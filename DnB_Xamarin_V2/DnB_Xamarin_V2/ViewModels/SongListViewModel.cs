using DnB_Xamarin_V2.Models;
using DnB_Xamarin_V2.Services;
using MediaManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DnB_Xamarin_V2.ViewModels
{
    internal class SongListViewModel : ObservableObj
    {
        public ObservableCollection<Song> SongCollection { get; set; }
        private GetPostBassBlog GetPostBassBlog;

        public SongListViewModel()
        {
            Connectivity.ConnectivityChanged += ChangeNetworkConnection;

            SongCollection = new ObservableCollection<Song>();
            GetPostBassBlog = new GetPostBassBlog();

            AddSongListCommand = new Command(AddSongList);
            RewindSongCommand = new Command(RewindSong);

            TimelineMinimum = 0;
            TimelineMaximum = 1000;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                networkConnectionStatus = true;
                Task.Run(() => GetBassBlogFeatured());
            }
            else
            {
                networkConnectionStatus = false;
                ErrorMessage = "No connection";
            }
        }

        private async void GetBassBlogFeatured()
        {
            List<Song> songs = new List<Song>(30);

            songs = await GetPostBassBlog.GetDataBassBlogFeatured();

            foreach (Song song in songs)
                SongCollection.Add(song);
        }

        private double timelineValue;
        public double TimelineValue
        {
            get => timelineValue;
            set
            {
                timelineValue = value;
                OnPropertyChanged("TimelineValue");
            }
        }        

        private double timelineMinimum;
        public double TimelineMinimum
        {
            get => timelineMinimum;
            set
            {
                timelineMinimum = value;
                OnPropertyChanged("TimelineMinimum");
            }
        }        

        private double timelineMaximum;
        public double TimelineMaximum
        {
            get => timelineMaximum;
            set
            {
                if (value > 0)
                    timelineMaximum = value;

                OnPropertyChanged("TimelineMaximum");
            }
        }

        private TimeSpan songDuration;
        public TimeSpan SongDuration
        {
            get => songDuration;
            set
            {
                songDuration = value;
                OnPropertyChanged("SongDuration");
            }
        }

        private TimeSpan songCurrentPosition;
        public TimeSpan SongCurrentPosition
        {
            get => songCurrentPosition;
            set
            {
                songCurrentPosition = value;
                OnPropertyChanged("SongCurrentPosition");
            }
        }

        private string nameSong;
        public string NameSong
        {
            get => nameSong;
            set
            {
                nameSong = value;
                OnPropertyChanged("NameSong");
            }
        }

        private Song selectedSong;
        public Song SelectedSong
        {
            get => selectedSong;
            set
            {
                selectedSong = value;

                if (selectedSong.SongIsPlaying == true)
                    selectedSong.SongIsPlaying = false;

                PlaySong(selectedSong);
                NameSong = selectedSong.NameSong;

                OnPropertyChanged("SelectedSong");
            }
        }

        private async void PlaySong(Song song)
        {
            var mediaPlayer = CrossMediaManager.Current;            

            await mediaPlayer.Play(song.SongUrl);

            PlayPauseToggleButton = true;
            TimelineMinimum = 0;       

            mediaPlayer.MediaItemFinished += (sender, args) =>
            {
                PlayPauseToggleButton = false;
            };

            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                SongDuration = mediaPlayer.Duration;
                SongCurrentPosition = mediaPlayer.Position;
                TimelineMaximum = SongDuration.TotalMilliseconds;
                TimelineValue = mediaPlayer.Position.TotalMilliseconds;
                return true;
            });            
        }

        public ICommand RewindSongCommand { protected set; get; }
        private async void RewindSong()
        {
            await CrossMediaManager.Current.SeekTo(new TimeSpan(0, 0, 0, 0, (int)timelineValue));
        }

        public ICommand AddSongListCommand { protected set; get; }
        private async void AddSongList()
        {
            if (networkConnectionStatus == false)
                return;

            List<Song> songs = new List<Song>(30);

            songs = await GetPostBassBlog.PostDataBassBlogFeatured();

            foreach (Song song in songs)
                SongCollection.Add(song);
        }        

        private bool playPauseToggleButton = false;
        public bool PlayPauseToggleButton
        {
            get => playPauseToggleButton;
            set
            {
                playPauseToggleButton = value;

                if (value == true)
                {
                    CrossMediaManager.Current.Play();
                }
                else
                {
                    CrossMediaManager.Current.Pause();
                }

                OnPropertyChanged("PlayPauseToggleButton");
            }
        }                     

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private bool networkConnectionStatus;
        private async void ChangeNetworkConnection(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                networkConnectionStatus = true;
                ErrorMessage = "";

                if (SongCollection.Count == 0)
                    await Task.Run(() => GetBassBlogFeatured());
            }
            else
            {
                networkConnectionStatus = false;
                ErrorMessage = "No connection";
            }
        }
    }
}