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
    public class GameViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Members

        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region Constructors

        public GameViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            Initialize();
        }

        #endregion

        #region Events



        #endregion

        #region Functions

        private async Task Initialize()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            try
            {
                var item = await _dataService.GetData();
            }
            catch (Exception ex)
            {
                await dialog.ShowMessage(ex.Message, "Data Error");
            }
        }

        #endregion
    }
}
