using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private DataItem _item;

        public DataItem Item
        {
            get { return _item; }
            set
            {
                _item = value;
                NotifyPropertyChanged("Item");
            }
        }

        private bool _answer;

        public bool Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                NotifyPropertyChanged("Answer");
            }
        }

        private RelayCommand<string> _answerPressed;

        public RelayCommand<string> AnswerPress
        {
            get { return _answerPressed; }
            set
            {
                _answerPressed = new RelayCommand<string>(AnswerPressedEvent);
                NotifyPropertyChanged("AnswerPress");
            }
        }

        public event EventHandler<bool> AnswerGiven;

        private void AnswerGivenTrigger()
        {
            AnswerGiven?.Invoke(this, Answer);
        }

        private List<string> _answers;

        public List<string> Answers
        {
            get { return _answers; }
            set
            {
                _answers = value;
                NotifyPropertyChanged("Answers");
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

        public GameViewModel(IDataService dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _answerPressed = new RelayCommand<string>(AnswerPressedEvent);
            Answers = new List<string>();
        }

        #endregion

        #region Functions

        private void AnswerPressedEvent(string answer)
        {
            if(answer == "Answer1")
            {
                Answer = Answers[0] == Item.Title;
            }
            else if(answer == "Answer2")
            {
                Answer = Answers[1] == Item.Title;
            }
            else if(answer == "Answer3")
            {
                Answer = Answers[2] == Item.Title;
            }

            AnswerGivenTrigger();
        }

        #endregion
    }
}
