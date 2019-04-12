using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Fingerprint;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FormsBiometricEnhanced
{
    public partial class MainPage : ContentPage
    {

        private string username;
        private string password;

        private static ISettings AppSettings => CrossSettings.Current;

        public static bool BiometricToggle
        {
            get => AppSettings.GetValueOrDefault(nameof(BiometricToggle), false);
            set => AppSettings.AddOrUpdateValue(nameof(BiometricToggle), value);
        }

        public MainPage()
        {
            InitializeComponent();
            SetupBiometricUI();
        }

        async void SetupBiometricUI()
        {
            if(!string.IsNullOrEmpty(await GetUsernameFromSecureStorage()) && !string.IsNullOrEmpty(await GetPasswordFromSecureStorage()))
            {
                if(BiometricToggle == true)
                {
                    biometricSwitch.IsToggled = true;
                    usernameEntry.Text = await GetUsernameFromSecureStorage();
                    passwordEntry.Text = await GetPasswordFromSecureStorage();
                }
            }
        }

        async void LoginButton_Clicked(object sender, System.EventArgs e)
        {
            if (!biometricSwitch.IsToggled)
            {
                username = usernameEntry.Text;
                password = passwordEntry.Text;
                await DisplayAlert("Login", "Logged in as " + usernameEntry.Text + " successfully", "OK");
            }
            else
            {
                BiometricLogin();
            }
        }

        void ClearCredentialsButton_Clicked(object sender, System.EventArgs e)
        {
            SecureStorage.RemoveAll();
            biometricSwitch.IsToggled = false;
            BiometricToggle = false;
            usernameEntry.Text = "";
            passwordEntry.Text = "";
        }

        public async void BiometricLogin()
        {
            var result = await CrossFingerprint.Current.AuthenticateAsync("Login using Biometrics");
            if (result.Authenticated)
            {
                SaveCredentials(usernameEntry.Text, passwordEntry.Text);
                BiometricToggle = true;
                await DisplayAlert("Login", "Logged in as " + usernameEntry.Text + " successfully", "OK");
            }
        }

        public async void SaveCredentials(string username, string password)
        {
            try
            {
                await SecureStorage.SetAsync("FormsBiometricEnhanced_username", username);
                await SecureStorage.SetAsync("FormsBiometricEnhanced_password", password);
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to save username and password to secure storage");
            }
        }

        private async Task<string> GetUsernameFromSecureStorage()
        {
            string username = "";
            try
            {
                username = await SecureStorage.GetAsync("FormsBiometricEnhanced_username");
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to retrieve username from secure storage");
            }
            return username;
        }

        private async Task<string> GetPasswordFromSecureStorage()
        {
            string password = "";
            try
            {
                password = await SecureStorage.GetAsync("FormsBiometricEnhanced_password");
            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to retrieve password from secure storage");
            }
            return password;
        }

    }
}
