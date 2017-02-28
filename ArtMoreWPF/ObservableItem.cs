namespace ArtMoreWPF
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public abstract class ObservableItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
