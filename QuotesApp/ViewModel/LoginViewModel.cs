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

        public event EventHandler SignUpTriggered;

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

        protected void TriggerSignUp()
        {
            SignUpTriggered?.Invoke(this, new EventArgs());
        }

        private bool _signUp;

        public bool SignUp
        {
            get { return _signUp; }
            set
            {
                _signUp = value;
                NotifyPropertyChanged("SignUp");
            }
        }

        private RelayCommand _navigateCommand;

        public RelayCommand NavigateCommand
        {
            set
            {
                _navigateCommand = new RelayCommand(InvokeNavigateEvent);
                NotifyPropertyChanged("NavigateCommand");
            }
            get { return _navigateCommand; }
        }

        private RelayCommand _signUpCommand;

        public RelayCommand SignUpCommand
        {
            get { return _signUpCommand; }
            set
            {
                _signUpCommand = new RelayCommand(InvokeSignUpEvent);
                NotifyPropertyChanged("SignUpCommand");
            }
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
            SignUpCommand = new RelayCommand(InvokeSignUpEvent);
            LoginEnum = LoginPageEnum.Login;
            SignUp = true;
        }

        #endregion

        #region Events

        private void InvokeNavigateEvent()
        {
            NavigateToPage();
        }

        private void InvokeSignUpEvent()
        {
            TriggerSignUp();
        }

        #endregion
    }
}
