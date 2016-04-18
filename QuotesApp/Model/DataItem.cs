using System.ComponentModel;

namespace QuotesApp.Model
{
    public class DataItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region Members

        private string _iD;

        public string ID
        {
            get { return _iD; }
            set
            {
                _iD = value;
                NotifyPropertyChanged("ID");
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                NotifyPropertyChanged("Content");
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string _link;

        public string Link
        {
            get { return _link; }
            set
            {
                _link = value;
                NotifyPropertyChanged("Link");
            }
        }

        #endregion

        #region Constructors

        public DataItem(string id, string content, string title, string link)
        {
            ID = id;
            Content = content;
            Title = title;
            Link = link;
        }

        #endregion
    }
}