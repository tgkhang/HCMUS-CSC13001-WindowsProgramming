using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using POS_For_Small_Shop.Helpers;

namespace POS_For_Small_Shop.ViewModels
{
    public class AuthViewModel : INotifyPropertyChanged
    {

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private Window? _host = null;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AuthViewModel(Window host)
        {
            _host = host;
        }

        public bool CanLogin()
        {
            return Username.IsNotEmpty() && Password.IsNotEmpty();
        }

        public void Login()
        {
            var screen = new DashboardWindow(Username);
            screen.Activate();

            _host?.Close();
        }

    }
}
