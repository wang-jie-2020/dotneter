using System.ComponentModel;

namespace Generator
{
    public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
    {
        protected void InvokePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}