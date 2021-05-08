using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.Windows.ViewModel
{
    public class ObservableObj : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            var handle = PropertyChanged;
            if (handle == null)
                return;
            handle(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
