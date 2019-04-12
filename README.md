# FormsBiometricEnhanced
An enhanced example of cross platform biometric login using Xamarin Forms (enhanced from my original solution: https://github.com/ShravanJ/FormsBiometric)

## Features
* Compiles against Xamarin.Forms 3.6 using up-to-date iOS and Android targets
* Uses Xamarin.Essential's SecureStorage, Microsoft's new secure storage mechanism
* Supports Face ID and Touch ID on iOS devices and generic fingerprint authentication on Android devices

## Compiling and running
Make sure you have Xcode and Visual Studio for Mac up-to-date. For iOS devices and the Simulator you'll need to use Xcode to generate a Team Provisioning profile for the bundle ID "com.shravanj.FormsBiometricEnhanced". This can be done by creating a new empty project within Xcode with the specified bundle ID, ensure you are logged in to Xcode, and select Xcode Managed Profile. Click on the Info icon (i) next to the 'Xcode Managed Profile' then drag the provisioning profile icon onto your desktop. With your iPhone connected, double click to install it. You should now be able to run the app on your iPhone or Simulator. 

## Running in simulators
For the iOS Simulator, you'll need to specify that Face ID/Touch ID has been 'enrolled' before trying to login using biometric auth. To do this, run the app in the simulator and before trying to login select Hardware > Face ID (or Touch ID) > Enrolled. You will then see a prompt confirming you want to allow access and then click Hardware > Face ID (or Touch ID) > Matching Face (or print).

For Android, see the following link: https://stackoverflow.com/questions/35335892/android-m-fingerprint-scanner-on-android-emulator
