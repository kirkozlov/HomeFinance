using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Accessibility;
using HomeFinance.DataAccess.Proxy;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Enums;
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

        public RelayCommand ButtonClickCommand { get; }

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
                if(this.IsLoggedIn)
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
            try
            {
                Client.Instance.Login(Username, Password);
                this.IsLoggedIn = true;
            }
            catch
            {
                MessageBox.Show("Can not login", "Can not login");
            }
           
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

    class CSVViewModel : INotifyPropertyChanged
    {
        private DataView _data;
        private string _skipLines = string.Empty;
        private string _dateTime = string.Empty;
        private string _income = string.Empty;
        private string _expense = string.Empty;
        private string _description = string.Empty;

        private readonly Func<Guid> _getSelectedWalletId;
 
        public ICommand OpenCSVCommand { get; }

        public RelayCommand RefreshCommand { get; }

        public string SkipLines
        {
            get => this._skipLines;
            set
            {
                if (value == this._skipLines) return;
                this._skipLines = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public string DateTime
        {
            get => this._dateTime;
            set
            {
                if (value == this._dateTime) return;
                this._dateTime = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public string Income
        {
            get => this._income;
            set
            {
                if (value == this._income) return;
                this._income = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public string Expense
        {
            get => this._expense;
            set
            {
                if (value == this._expense) return;
                this._expense = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get => this._description;
            set
            {
                if (value == this._description) return;
                this._description = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public CSVViewModel(Func<Guid> getSelectedWalletId)
        {
            this._getSelectedWalletId = getSelectedWalletId;
            OpenCSVCommand = new RelayCommand(_ => this.OpenCSV());
            RefreshCommand = new RelayCommand(_ => this.Refresh(), _ => CanRefresh());
        }

        public DataView Data
        {
            get => this._data;
            set
            {
                if (Equals(value, this._data)) return;
                this._data = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged();
                this.OnPropertyChanged();
            }
        }

        List<List<string>> _listData;
        private IEnumerable<TransientOperation> _resultData;

        public IEnumerable<TransientOperation> ResultData
        {
            get => this._resultData;
            set
            {
                if (Equals(value, this._resultData)) return;
                this._resultData = value;
                this.OnPropertyChanged();
            }
        }

        void OpenCSV()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
                //,Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                var lines= File.ReadAllLines(dialog.FileName);
                _listData = lines.Select(l => l.Split(';').ToList()).ToList();

                DataTable dt = new DataTable();

                
                dt.Columns.AddRange(_listData
                    .MaxBy(i => i.Count)!
                    .Select((_, i) => new DataColumn(i.ToString(), typeof(string)))
                    .ToArray()
                );

                _listData.ForEach(line =>
                {
                    var dr = dt.NewRow();
                    var index = 0;
                    line.ForEach(column => dr[index++]=column);
                    dt.Rows.Add(dr);
                });
                this.Data = new DataView(dt);
            }
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void Refresh()
        {
            var skipLines = int.Parse(this.SkipLines);
            var dateTimeColumn = int.Parse(this.DateTime);
            var incomeColumn=int.Parse(this.Income);
            var expenseColumn=int.Parse(this.Expense);

            try
            {
                ResultData = this._listData.Skip(skipLines).Select(v =>
                {
                    OperationType operationType = OperationType.Income;
                    double amount;
                    if (incomeColumn == expenseColumn)
                    {
                        amount = double.Parse(v[incomeColumn].Replace(',', '.'), CultureInfo.InvariantCulture);
                        if (amount < 0)
                        {
                            operationType = OperationType.Expense;
                            amount = Math.Abs(amount);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(v[incomeColumn]))
                        {
                            amount = double.Parse(v[incomeColumn].Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            operationType = OperationType.Expense;
                            amount = double.Parse(v[expenseColumn].Replace(',', '.'), CultureInfo.InvariantCulture);
                        }
                    }

                    var description = this.Description;
                    for (int i = 0; i < v.Count; i++)
                    {
                        description = description.Replace("{" + i + "}", v[i]);
                    }

                    description = description.Replace("\\n", "\n");

                    return new TransientOperation(Guid.NewGuid(), this._getSelectedWalletId(), operationType, amount,
                        description,
                        System.DateTime.Parse(v[dateTimeColumn]));
                });
            }
            catch
            {
                MessageBox.Show("Something wrong", "Something wrong");
            }
        }

        bool CanRefresh()
        {
            var result = int.TryParse(this.SkipLines, out int _);
            result &= int.TryParse(this.DateTime, out int _);
            result &= int.TryParse(this.Income, out int _);
            result &= int.TryParse(this.Expense, out int _);

            return result;
        }
    }

    class MainViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public SelectWalletViewModel SelectWalletViewModel { get; set; } = new SelectWalletViewModel();
        public CSVViewModel CSVViewModel { get; set; }

        public RelayCommand SendCommand { get;  }

        public MainViewModel()
        {
            LoginViewModel = new LoginViewModel(SelectWalletViewModel.UpdateWallets);
            this.CSVViewModel = new CSVViewModel(() => SelectWalletViewModel.SelectedWallet.Id!.Value);
            SendCommand = new RelayCommand(_ => this.Send());
        }

        async void Send()
        {
            if (!this.LoginViewModel.IsLoggedIn ||  !this.CSVViewModel.ResultData.Any())
            {
                MessageBox.Show("Can not send", "Can not send");
            }
            else
            {
                await Client.Instance.Gateway.TransientOperationRepository.AddRange(this.CSVViewModel.ResultData);
                MessageBox.Show("Data sent", "Data sent");
            }
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
            
            var baseUrl="https://homefinanceapi.azurewebsites.net/api/";
            //var baseUrl= "https://localhost:7080/api";
            this._gateway=new Gateway(baseUrl, username, password);
        }

        public void Logout()
        {
            this._gateway = null;
        }

    }
}
