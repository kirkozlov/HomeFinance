using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using HomeFinance.DataAccess.Proxy;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Utils;
using Microsoft.Win32;

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

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand ButtonClickCommand { get; set; }

        private bool _isLoggedIn = false;
        private string _username = string.Empty;
        private string _password = string.Empty;

        public string ButtonText => IsLoggedIn ? "Logout" : "Login";

        private Action _onSuccessfullyLogin;

        public LoginViewModel(Action onSuccessfullyLogin)
        {
            this._onSuccessfullyLogin = onSuccessfullyLogin;
            this.ButtonClickCommand = new RelayCommand(o => ButtonClick(), o => CanClick());
        }
        
        public void ButtonClick()
        {
            if (!IsLoggedIn)
            {
                this.Login();
                this._onSuccessfullyLogin();
            }
            else
            {
                this.IsLoggedIn = false;
            }
            this.OnPropertyChanged(nameof(ButtonText));
        }

        public void Login()
        {
            Client.Instance.Login(Username, Password);
            this.IsLoggedIn = true;
        }

        public bool CanClick()
        {
            return IsLoggedIn ||
                   (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password));
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    class WalletViewModel
    {
        public Wallet Wallet { get; }
        public string DisplayName => $"{this.Wallet.Name} ({this.Wallet.GroupName})";

        public WalletViewModel(Wallet wallet)
        {
            this.Wallet = wallet;
        }
    }

    class SelectWalletViewModel
    {
        readonly ObservableCollection<WalletViewModel> _wallets=new ();
        public IEnumerable<WalletViewModel> Wallets => _wallets;

        public Wallet SelectedWallet { get; set; }

        public async void UpdateWallets()
        {
            var walletsToAdd = await Client.Instance.Gateway.WalletRepository.GetAll();
            this._wallets.Clear();
            walletsToAdd.ToList().ForEach(w=> _wallets.Add(new WalletViewModel(w)));
        }
    }

    class CSVViewModel
    {
        public ICommand OpenCSVCommand { get; }

        public CSVViewModel()
        {
            this.OpenCSVCommand = new RelayCommand(_ => this.OpenCSV());
        }

        void OpenCSV()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                
            }
            
        }
    }

    class MainViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public SelectWalletViewModel SelectWalletViewModel { get; set; } = new SelectWalletViewModel();
        public CSVViewModel CSVViewModel { get; set; } = new CSVViewModel();

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel(SelectWalletViewModel.UpdateWallets);
        }
    }

    public class Client
    {
        private Client()
        {

        }

        public static Client Instance=new Client();

        private IGateway? _gateway = null;
        public IGateway Gateway => this._gateway ?? throw new NullReferenceException();

        public void Login(string username, string password)
        {
            this._gateway=new Gateway("https://localhost:7080/api/", username, password);
        }

        public void Logout()
        {
            this._gateway = null;
        }

    }
}
