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
using QuotesApp.IsolatedStorage;

namespace QuotesApp.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand<string> _navigateCommand;
        private int _userHighScore;
        private MobileServiceCollection<QuoteItem, QuoteItem> QuoteItems;
        private IMobileServiceTable<QuoteItem> quoteItemsTable = App.MobileService.GetTable<QuoteItem>();

        private LoginViewModel _loginViewModel;

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

        public MainViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            InitializeProperties();
            LoginViewModel.ButtonText = "SIGN IN";
            _userHighScore = 0;
            Initialize();
        }

        #endregion

        #region Events

        private void LoginViewModel_SignUpTriggered(object sender, EventArgs e)
        {
            switch(LoginViewModel.LoginEnum)
            {
                case LoginPageEnum.Login:
                    LoginViewModel.LoginEnum = LoginPageEnum.SignUp;
                    LoginViewModel.ButtonText = "SIGN UP";
                    LoginViewModel.LoginText = "Sign in instead";
                    //LoginViewModel.SignUp = false;
                    break;
                case LoginPageEnum.SignUp:
                    LoginViewModel.LoginEnum = LoginPageEnum.Login;
                    LoginViewModel.ButtonText = "SIGN IN";
                    LoginViewModel.LoginText = "Forgot to sign up ?";
                    //LoginViewModel.SignUp = false;
                    break;
                default:
                    break;
            }
        }

        private void LoginViewModel_NavigateToPageTriggered(object sender, EventArgs e)
        {
            switch (LoginViewModel.LoginEnum)
            {
                case LoginPageEnum.Login:
                    PerformLoginActions();
                    break;
                case LoginPageEnum.SignUp:
                    PerformSignUpActions();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Functions

        private async Task Initialize()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            try
            {
                QuoteItems = await quoteItemsTable.ToCollectionAsync();
                switch (LoginViewModel.LoginEnum)
                {
                    case LoginPageEnum.Login:
                        LoginViewModel.ButtonText = "SIGN IN";
                        break;
                    case LoginPageEnum.SignUp:
                        LoginViewModel.ButtonText = "SIGN UP";
                        break;
                    default:
                        break;
                }
                LoginViewModel.IsLoading = false;

                if (await CheckIfUserExists())
                {
                    PerformLoginActions();
                    return;
                }
            }
            catch (Exception ex)
            {
                await dialog.ShowMessage(ex.Message, "Data Error");
            }
        }

        private async Task<bool> CheckIfUserExists()
        {
            var userCredentials = await IsolatedStorageManager.LoadFromIsolatedStorage();
            if (userCredentials == null) return false;

            string[] cred = userCredentials.Split(';');

            if (cred == null) return false;

            if(cred.Length == 4)
            {
                LoginViewModel.UserID = new Guid(cred[0]);
                LoginViewModel.Email = cred[1];
                LoginViewModel.Password = cred[2];
                return true;
            }

            return false;
        }

        private void InitializeProperties()
        {
            LoginViewModel = new LoginViewModel(_dataService, _navigationService);
            LoginViewModel.NavigateToPageTriggered += LoginViewModel_NavigateToPageTriggered;
            LoginViewModel.SignUpTriggered += LoginViewModel_SignUpTriggered;
            LoginViewVisibility = true;
        }

        private async void PerformLoginActions()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            if ((string.IsNullOrEmpty(LoginViewModel.Email)) && (string.IsNullOrEmpty(LoginViewModel.Password)))
            {
                await dialog.ShowMessage("Please complete the fields before pressing Sign In", "Login Error");
            }
            else if ((string.IsNullOrEmpty(LoginViewModel.Email)))
            {
                await dialog.ShowMessage("Please complete the email field before pressing Sign In", "Login Error");
            }
            else if ((string.IsNullOrEmpty(LoginViewModel.Password)))
            {
                await dialog.ShowMessage("Please complete the password field before pressing Sign In", "Login Error");
            }
            else
            {
                #region Validate User

                switch (ValidateUser())
                {
                    case UserAuthenticationEnum.Success:
                        _navigationService.NavigateTo(ViewModelLocator.SecondPageKey);
                        LoginViewModel.LoginEnum = LoginPageEnum.Login;
                        IsolatedStorageManager.SaveToIsolatedStorage(MakeStringParsable());
                        break;
                    case UserAuthenticationEnum.UserCredentialsWrong:
                        await dialog.ShowMessage("The user password is wrong", "Login Error");
                        LoginViewModel.LoginEnum = LoginPageEnum.Login;
                        break;
                    case UserAuthenticationEnum.UserNotFound:
                        dialog = ServiceLocator.Current.GetInstance<IDialogService>();
                        await dialog.ShowMessage("This user does not exist", "Login Error");
                        LoginViewModel.LoginEnum = LoginPageEnum.SignUp;
                        break;
                    default:
                        break;
                }

                #endregion
            }
        }

        private async void PerformSignUpActions()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            try
            {
                var signUpItem = new QuoteItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    EMail = LoginViewModel.Email,
                    Password = LoginViewModel.Password,
                    Highscore = 0
                };
                await quoteItemsTable.InsertAsync(signUpItem);
                QuoteItems.Add(signUpItem);
                IsolatedStorageManager.SaveToIsolatedStorage(MakeStringParsable());
                _navigationService.NavigateTo(ViewModelLocator.SecondPageKey);
            }
            catch(Exception ex)
            {
                await dialog.ShowMessage(ex.Message, "Data Error");
            }
        }

        private string MakeStringParsable()
        {
            return string.Format("{0};{1};{2};{3}", LoginViewModel.UserID, LoginViewModel.Email, LoginViewModel.Password, _userHighScore);
        }

        private UserAuthenticationEnum ValidateUser()
        {
            if (QuoteItems == null) return UserAuthenticationEnum.UserNotFound;

            if (QuoteItems.Count == 0) return UserAuthenticationEnum.UserNotFound;

            foreach(var user in QuoteItems)
            {
                if (user.EMail.ToLower() == LoginViewModel.Email.ToLower() && user.Password.ToLower() == LoginViewModel.Password.ToLower())
                {
                    _userHighScore = user.Highscore;
                    return UserAuthenticationEnum.Success;
                }
                else if (user.EMail.ToLower() == LoginViewModel.Email.ToLower() && user.Password.ToLower() != LoginViewModel.Password.ToLower())
                    return UserAuthenticationEnum.UserCredentialsWrong;
            }

            return UserAuthenticationEnum.UserNotFound;
        }

        #endregion
    }
}