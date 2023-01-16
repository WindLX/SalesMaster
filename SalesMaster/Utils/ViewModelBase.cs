using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalesMaster.Utils
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class SingletonViewModelBase<T> : INotifyPropertyChanged where T : new()
    {
        private static T instance;
        private static readonly object locker = new object();
        public static T Instace
        { 
            get 
            { 
                if (instance == null)
                {
                    lock(locker)
                    {
                        if (instance == null)
                            instance = new T();
                    }
                }
                return instance;
            } 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
