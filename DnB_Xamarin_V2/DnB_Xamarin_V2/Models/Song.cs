using DnB_Xamarin_V2.Services;

namespace DnB_Xamarin_V2.Models
{
    public sealed class Song : ObservableObj
    {
        private string idSong;
        public string IdSong
        {
            get => idSong;
            set
            {
                idSong = value;
                OnPropertyChanged("IdSong");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
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

        private string songUrl;
        public string SongUrl
        {
            get => songUrl;
            set
            {
                songUrl = value;
                OnPropertyChanged("SongUrl");
            }
        }

        private string musicalStyle;
        public string MusicalStyle
        {
            get => musicalStyle;
            set
            {
                musicalStyle = value;
                OnPropertyChanged("MusicalStyle");
            }
        }

        private string totalTime;
        public string TotalTime
        {
            get => totalTime;
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
            }
        }

        private bool songIsPlaying;
        public bool SongIsPlaying
        {
            get => songIsPlaying;
            set
            {
                songIsPlaying = value;
                OnPropertyChanged("SongIsPlaying");
            }
        }
    }
}