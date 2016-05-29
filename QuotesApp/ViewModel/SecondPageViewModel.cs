using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Microsoft.WindowsAzure.MobileServices;
using QuotesApp.DatabaseClients;
using QuotesApp.IsolatedStorage;
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
        private IMobileServiceTable<QuoteItem> quoteItemsTable = App.MobileService.GetTable<QuoteItem>();

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

        private List<string> _answers;

        public List<string> Answers
        {
            get { return _answers; }
            set
            {
                _answers = new List<string>();
                NotifyPropertyChanged("Answers");
            }
        }

        private int _score;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }

        private int _highScore;

        public int HighScore
        {
            get { return _highScore; }
            set
            {
                _highScore = value;
                NotifyPropertyChanged("HighScore");
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

        private async void GameViewModel_AnswerGiven(object sender, bool e)
        {
            if(e)
            {
                Score++;
            }
            else
            {
                if(Score > HighScore)
                {
                    var userCredentials = await IsolatedStorageManager.LoadFromIsolatedStorage();
                    if (userCredentials == null) return;

                    string[] cred = userCredentials.Split(';');

                    if (cred == null) return;

                    if (cred.Length != 4) return;

                    var signUpItem = new QuoteItem()
                    {
                        Id = cred[0],
                        EMail = cred[1],
                        Password = cred[2],
                        Highscore = Score
                    };

                    await quoteItemsTable.UpdateAsync(signUpItem);
                    HighScore = Score;
                }

                Score = 0;
            }

            await Initialize();
        }


        #endregion

        #region Functions

        private void InitializeProperties()
        {
            GameViewModel = new GameViewModel(_dataService, _navigationService);
            GameViewModel.AnswerGiven += GameViewModel_AnswerGiven;
            Answers = new List<string>();
            Score = 0;
        }

        private async Task Initialize()
        {
            var dialog = ServiceLocator.Current.GetInstance<IDialogService>();

            try
            {
                GameViewModel.Item = await _dataService.GetData();
                GetHighScore();
                ManipulateBindingItems();

                var wrongAnswers = await _dataService.GetWrongAnswersData();

                if (wrongAnswers == null) return;
                if (wrongAnswers.Count == 0) return;

                Answers.Clear();

                Answers.Add(GameViewModel.Item.Title);

                foreach(var item in wrongAnswers)
                {
                    if (item == null) continue;

                    Answers.Add(item.Title);
                }

                Shuffle(Answers);
                GameViewModel.Answers = Answers;
            }
            catch (Exception ex)
            {
                await dialog.ShowMessage(ex.Message, "Data Error");
            }
        }

        private async void GetHighScore()
        {
            var userCredentials = await IsolatedStorageManager.LoadFromIsolatedStorage();
            if (userCredentials == null) return;

            string[] cred = userCredentials.Split(';');

            if (cred == null) return;

            if (cred.Length == 4)
            {
                HighScore = Convert.ToInt32(cred[3]);
                return;
            }
        }

        private void ManipulateBindingItems()
        {
            if (GameViewModel == null) return;

            // Remove <p> from the beginning
            GameViewModel.Item.Content = GameViewModel.Item.Content.Remove(0, 3);

            // Remove the </p> from the end
            GameViewModel.Item.Content = GameViewModel.Item.Content.Remove(GameViewModel.Item.Content.Length - 5, 4);
        }

        #endregion

        #region Randomize Answers

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                Random rng = new Random();
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion
    }
}
