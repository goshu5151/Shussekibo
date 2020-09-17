using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Shussekibo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new Login();
            MainPage = new MainPage();
            //MainPage = new ContentPage { Title = "App Lifecycle Sample" };
            //MainPage = new NavigationPage(new MainPage());

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
