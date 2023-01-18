using System;
using SalesMaster.Utils;

namespace SalesMaster.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        public event Action<object, string> OnCurrentViewChanged;

        private object currentView;
        private bool canChangePage;

        public object CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public bool CanChangePage
        {
            get => canChangePage;
            set
            {
                canChangePage = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenHomePage { get; set; }
        public RelayCommand OpenNewPage { get; set; }
        public RelayCommand OpenSearchPage { get; set; }
        public RelayCommand OpenCompanyPage { get; set; }
        public RelayCommand OpenSettingsPage { get; set; }

        private void CreateHomePage(object parameter)
        {
            CurrentView = new HomeViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Home, "CurrentViewChanged");
        }

        private void CreateNewPage(object parameter)
        {
            CurrentView = new NewViewModel();
            OnCurrentViewChanged?.Invoke(PageName.New, "CurrentViewChanged");
        }

        private void CreateSearchPage(object parameter)
        {
            CurrentView = new SearchViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Search, "CurrentViewChanged");
        }

        private void CreateCompanyPage(object parameter)
        {
            CurrentView = new CompanyViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Company, "CurrentViewChanged");
        }

        private void CreateSettingsPage(object parameter)
        {
            CurrentView = new SettingsViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Settings, "CurrentViewChanged");
        }

        public NavigationViewModel()
        {
            CanChangePage = true;

            OnCurrentViewChanged += Broadcaster.Instace.Publish;
            Broadcaster.Instace.Subscribe(new Action<object>((parameter) => CanChangePage = (bool)parameter), "CanPageChange");

            OpenHomePage = new RelayCommand(new Action<object>(CreateHomePage), new Predicate<object>((parameter) => CanChangePage));
            OpenNewPage = new RelayCommand(new Action<object>(CreateNewPage), new Predicate<object>((parameter) => CanChangePage));
            OpenSearchPage = new RelayCommand(new Action<object>(CreateSearchPage), new Predicate<object>((parameter) => CanChangePage));
            OpenCompanyPage = new RelayCommand(new Action<object>(CreateCompanyPage), new Predicate<object>((parameter) => CanChangePage));
            OpenSettingsPage = new RelayCommand(new Action<object>(CreateSettingsPage), new Predicate<object>((parameter) => CanChangePage));

            CurrentView = new HomeViewModel();
        }
    }
}
