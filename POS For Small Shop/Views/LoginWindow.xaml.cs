using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_For_Small_Shop.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        public AuthViewModel ViewModel { get; set; }
        public LoginWindow()
        {
            this.InitializeComponent();
            ViewModel = new AuthViewModel(this);
            container.DataContext = ViewModel;

            //var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
            //var username = localStorage.Values["Username"] as string;
            //var encryptedInBase64 = localStorage.Values["Password"] as string;
            //var entropyInBase64 = localStorage.Values["Entropy"] as string;

            //if (!string.IsNullOrEmpty(username))
            //{
            //    var encryptedInBytes = Convert.FromBase64String(encryptedInBase64);
            //    var entropyInBytes = Convert.FromBase64String(entropyInBase64);
            //    var passwordInBytes = ProtectedData.Unprotect(encryptedInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
            //    var password = Encoding.UTF8.GetString(passwordInBytes);

            //    ViewModel.Username = username;
            //    ViewModel.Password = password;
            //    ViewModel.RememberMe = true;
            //}
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            // Empty event handler
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CanLogin())
            {
                bool success = ViewModel.Login();
                //var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;

                //if (ViewModel.RememberMe == true)
                //{
                //    var passwordInBytes = Encoding.UTF8.GetBytes(ViewModel.Password);
                //    var entropyInBytes = new byte[20];
                //    using (var rng = RandomNumberGenerator.Create())
                //    {
                //        rng.GetBytes(entropyInBytes);
                //    }
                //    var encryptedInBytes = ProtectedData.Protect(passwordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                //    var encryptedInBase64 = Convert.ToBase64String(encryptedInBytes);
                //    var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

                //    localStorage.Values["Username"] = ViewModel.Username;
                //    localStorage.Values["Password"] = encryptedInBase64;
                //    localStorage.Values["Entropy"] = entropyInBase64;

                //    Debug.WriteLine($"Encrypted password in base 64 is: {encryptedInBase64}");
                //}
                //else
                //{
                //    //var localStorage = Windows.Storage.ApplicationData.Current.LocalSettings;
                //    //localStorage.Values["Username"] = null;
                //    //localStorage.Values["Password"] = null;
                //    //localStorage.Values["Entropy"] = null;

                //    localStorage.Values.Remove("Username");
                //    localStorage.Values.Remove("Password");
                //    localStorage.Values.Remove("Entropy");
                //    Debug.WriteLine("Login info cleared from local storage.");
                //}

                if (success)
                {
                    var screen = new DashboardWindow();
                    screen.Activate();
                    this.Close();
                }
            }
        }
    }
}
