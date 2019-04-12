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

/*
 * FormsBiometricEnhanced - https://github.com/ShravanJ/FormsBiometricEnhanced
 * Author: Shravan Jambukesan <shravan.j97@gmail.com>
 * Date: 4/11/19
 */

namespace FormsBiometricEnhanced
{
    public partial class MainPage : ContentPage
    {

        private string username;
        private string password;

        /*
         * Plugin.Settings allows us to save the state of the biometric toggle to tell us whether or not to use
         * biomeric login the next time the user launces the app        
         */
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

        /*
         * Setup the UI depending on whether or not we enabled biometric auth before
         */
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

        /*
         * Login button click event, based on biometric switch toggle use traditional or biometric authentication
         */
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

        /*
         * Clear all SecureStorage keys for this app, useful for debugging
         */
        void ClearCredentialsButton_Clicked(object sender, System.EventArgs e)
        {
            SecureStorage.RemoveAll();
            biometricSwitch.IsToggled = false;
            BiometricToggle = false;
            usernameEntry.Text = "";
            passwordEntry.Text = "";
        }

        /*
         * Use Plugin.Fingerprint to authenticate using local device biometrics such as Face ID or Touch ID
         */
        public async void BiometricLogin()
        {
            // Message for local authentication promot
            var result = await CrossFingerprint.Current.AuthenticateAsync("Login using Biometrics");
            if (result.Authenticated)
            {
                // Save the username and password each time to it's always up-to-date
                SaveCredentials(usernameEntry.Text, passwordEntry.Text);
                // Set the toggle to on so Plugin.Settings loads it next time we run the app
                BiometricToggle = true;
                // Successful login!
                await DisplayAlert("Login", "Logged in as " + usernameEntry.Text + " successfully", "OK");
            }
        }

        /*
         * Credentials are saved using Xamarin.Essential's SecureStorage class (Xamarin.Auth's AccountStore is now deprecated)
         * Saving credentials is as easy as creating a name for the keys ("FormsBiometricEnhanced_username") and assigning a value
         * (username passed in from the text entry). Entries are written asynchronously. 
         * 
         * NOTE for iOS: You'll need to enable Keychain Sharing in your Entitlements file to run inside of the iOS Simulator (this has
         * already been done for you in this project)
         * More info here: https://docs.microsoft.com/en-us/xamarin/essentials/secure-storage?tabs=ios        
         */
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

        /*
         * Since SecureStorage uses async functions we need to use a Task that returns a string to
         * grab the username from SecureStorage        
         */
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

        /*
        * Since SecureStorage uses async functions we need to use a Task that returns a string to
        * grab the password from SecureStorage        
        */
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
