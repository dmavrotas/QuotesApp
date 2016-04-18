using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using QuotesApp.Model;

namespace QuotesApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;
        private RelayCommand _incrementCommand;
        private RelayCommand<string> _navigateCommand;
        private RelayCommand _sendMessageCommand;
        private RelayCommand _showDialogCommand;
        private string _welcomeTitle = string.Empty;

        #endregion

        #region Commands

        public RelayCommand<string> NavigateCommand
        {
            get
            {
                return _navigateCommand;
                       //?? (_navigateCommand = new RelayCommand<string>(
                       //    p => _navigationService.NavigateTo(ViewModelLocator.SecondPageKey, p),
                       //    p => !string.IsNullOrEmpty(p)));
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
            Initialize();
        }

        #endregion

        private async Task Initialize()
        {
            try
            {
                var item = await _dataService.GetData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}