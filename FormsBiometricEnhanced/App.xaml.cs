using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
 * FormsBiometricEnhanced - https://github.com/ShravanJ/FormsBiometricEnhanced
 * Author: Shravan Jambukesan <shravan.j97@gmail.com>
 * Date: 4/11/19
 */

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace FormsBiometricEnhanced
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
