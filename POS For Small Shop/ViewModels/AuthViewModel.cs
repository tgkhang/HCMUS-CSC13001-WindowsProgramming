using Microsoft.UI.Xaml;
using PropertyChanged;

namespace POS_For_Small_Shop.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AuthViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        private Window? _host;

        public AuthViewModel(Window host)
        {
            _host = host;
        }

        public bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        public bool Login()
        {
            return ((Username == "admin" || Username == "tester") && Password == "1234");
        }
    }
}
