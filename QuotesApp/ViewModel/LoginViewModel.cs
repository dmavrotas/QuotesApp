using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using QuotesApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.ViewModel
{
    public class LoginViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private LoginPageEnum _loginEnum;

        public LoginPageEnum LoginEnum
        {
            get { return _loginEnum; }
            set
            {
                _loginEnum = value;
                NotifyPropertyChanged("LoginEnum");
            }
        }

        public event EventHandler NavigateToPageTriggered;

        private string _buttonText;

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                NotifyPropertyChanged("ButtonText");
            }
        }

        protected virtual void NavigateToPage()
        {
            NavigateToPageTriggered?.Invoke(this, new EventArgs());
        }

        private RelayCommand _navigateCommand;

        public RelayCommand NavigateCommand
        {

            set { _navigateCommand = new RelayCommand(InvokeNavigateEvent); }
            get { return _navigateCommand; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged("Email");
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
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

        #region Constructors

        public LoginViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand(InvokeNavigateEvent);
            LoginEnum = LoginPageEnum.Login;
        }

        #endregion

        #region Events

        private void InvokeNavigateEvent()
        {
            NavigateToPage();
        }

        #endregion
    }
}
