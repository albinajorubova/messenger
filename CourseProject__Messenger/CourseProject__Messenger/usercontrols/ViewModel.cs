using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CourseProject__Messenger.usercontrols
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FriendItem> _friends;

        public ObservableCollection<FriendItem> Friends
        {
            get { return _friends; }
            set
            {
                if (_friends != value)
                {
                    _friends = value;
                    OnPropertyChanged(nameof(Friends));
                }
            }
        }

        public ViewModel()
        {
            Friends = new ObservableCollection<FriendItem>
            {
                new FriendItem { Title = "Friend1", Message = "Hello", Color = "#73AFFF", MessageCount = 2, Image = "pack://application:,,,/CourseProject__Messenger;component/img/review1.jpg" },
                new FriendItem { Title = "Friend2", Message = "Hi", Color = "#5AC795", MessageCount = 3, Image = "pack://application:,,,/CourseProject__Messenger;component/img/review2.jpg" },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
