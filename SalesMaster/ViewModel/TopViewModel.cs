using SalesMaster.Utils;
using System;
using System.Linq;
using System.Windows;

namespace SalesMaster.ViewModel
{
    public class TopViewModel : ViewModelBase
    {
        private readonly MainWindow mainWindow;

        public RelayCommand MinWindow { get; set; }
        public RelayCommand MaxWindow { get; set; }
        public RelayCommand CloseWindow { get; set; }

        public TopViewModel()
        {
            mainWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            MinWindow = new RelayCommand(new Action<object>((parameter) => mainWindow.WindowState = WindowState.Minimized));
            MaxWindow = new RelayCommand(new Action<object>(MakeWindowMax));
            CloseWindow = new RelayCommand(new Action<object>((parameter) => mainWindow.Close()));
        }

        private void MakeWindowMax(object parameter)
        {
            if (mainWindow.WindowState == WindowState.Maximized)
                mainWindow.WindowState = WindowState.Normal;
            else
                mainWindow.WindowState = WindowState.Maximized;
        }
    }
}
