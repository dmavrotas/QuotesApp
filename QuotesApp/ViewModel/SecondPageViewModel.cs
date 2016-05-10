using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using QuotesApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesApp.ViewModel
{
    public class SecondPageViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        private GameViewModel _gameViewModel;

        public GameViewModel GameViewModel
        {
            get { return _gameViewModel; }
            set
            {
                _gameViewModel = value;
                NotifyPropertyChanged("GameViewModel");
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

        public SecondPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            InitializeProperties();
            Initialize();
        }

        #endregion

        #region Events



        #endregion

        #region Functions

        private void InitializeProperties()
        {
            GameViewModel = new GameViewModel(_dataService, _navigationService);
        }

        private async Task Initialize()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            try
            {
                GameViewModel.Item = await _dataService.GetData();
            }
            catch (Exception ex)
            {
                await dialog.ShowMessage(ex.Message, "Data Error");
            }
        }

        #endregion
    }
}
