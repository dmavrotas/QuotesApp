using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using QuotesApp.ViewModel;

namespace QuotesApp
{
    public sealed partial class MainPage
    {
        #region Members

        public MainViewModel Vm => (MainViewModel)DataContext;

        #endregion

        #region Constructors

        public MainPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManagerBackRequested;
        }

        #endregion

        #region System Events

        private void SystemNavigationManagerBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        #endregion
    }
}
