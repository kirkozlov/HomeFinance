using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomeFinance.Import.Desktop
{
    class LoginViewModel: INotifyPropertyChanged
    {
        public string Username
        {
            get => this._username;
            set
            {
                if (value == this._username) return;
                this._username = value;
                this.OnPropertyChanged();
                ButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => this._password;
            set
            {
                if (value == this._password) return;
                this._password = value;
                this.OnPropertyChanged();
                ButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ButtonClickCommand { get; set; }

        private bool _isLoggedIn = false;
        private string _username = string.Empty;
        private string _password = string.Empty;

        public string ButtonText => _isLoggedIn ? "Logout" : "Login";

        public LoginViewModel()
        {
            this.ButtonClickCommand = new RelayCommand(o => ButtonClick(), o => CanClick());
        }
        
        public void ButtonClick()
        {
            if (_isLoggedIn)
            {
                this.Login();
            }
            else
            {
                this._isLoggedIn = false;
            }
            this.OnPropertyChanged(nameof(ButtonText));
        }

        public void Login()
        {
            this._isLoggedIn = true;
        }

        public bool CanClick()
        {
            return _isLoggedIn ||
                   (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    class MainViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }=new LoginViewModel();
    }
}
