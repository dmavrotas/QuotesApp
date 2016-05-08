using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using QuotesApp.Model;
using System.ComponentModel;
using QuotesApp.DatabaseClients;
using Microsoft.WindowsAzure.MobileServices;

namespace QuotesApp.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand _incrementCommand;
        private RelayCommand<string> _navigateCommand;
        private RelayCommand _sendMessageCommand;
        private RelayCommand _showDialogCommand;
        private LoginViewModel _loginViewModel;

        private MobileServiceCollection<QuoteItem, QuoteItem> QuoteItems;
        private IMobileServiceTable<QuoteItem> quoteItemsTable = App.MobileService.GetTable<QuoteItem>();

        public LoginViewModel LoginViewModel
        {
            get { return _loginViewModel; }
            set
            {
                _loginViewModel = value;
                NotifyPropertyChanged("LoginViewModel");
            }
        }

        private bool _loginViewVisiblity;

        public bool LoginViewVisibility
        {
            get { return _loginViewVisiblity; }
            set
            {
                _loginViewVisiblity = value;
                NotifyPropertyChanged("LoginViewVisibility");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region Commands

        public RelayCommand<string> NavigateCommand
        {
            get
            {
                return _navigateCommand = 
                        (_navigateCommand = new RelayCommand<string>(
                           p => _navigationService.NavigateTo(ViewModelLocator.SecondPageKey, p),
                           p => !string.IsNullOrEmpty(p)));
            }
        }

        #endregion

        #region Constructors

        public MainViewModel(
            IDataService dataService,
            INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            InitializeProperties();
            Initialize();
        }

        #endregion

        private async Task Initialize()
        {
            try
            {
                var item = await _dataService.GetData();
                QuoteItems = await quoteItemsTable.ToCollectionAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Events

        private void InitializeProperties()
        {
            LoginViewModel = new LoginViewModel(_dataService, _navigationService);
            LoginViewModel.NavigateToPageTriggered += loginViewModel_NavigateToPageTriggered;
            LoginViewVisibility = true;
        }

        private void loginViewModel_NavigateToPageTriggered(object sender, EventArgs e)
        {
            _navigationService.NavigateTo(ViewModelLocator.SecondPageKey);
        }

        #endregion
    }
}