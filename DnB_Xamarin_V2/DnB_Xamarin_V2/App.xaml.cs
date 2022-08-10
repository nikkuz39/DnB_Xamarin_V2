using DnB_Xamarin_V2.Views;
using Xamarin.Forms;

namespace DnB_Xamarin_V2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SongListPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
