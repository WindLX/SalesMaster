using System;
using SalesMaster.Utils;

namespace SalesMaster.ViewModel
{
    public class NavigationViewModel : SingletonViewModelBase<NavigationViewModel>
    {
        public event Action<PageName> OnCurrentViewChanged;
        private object currentView;
        public object CurrentView
        { 
            get => currentView;
            set
            {
                currentView = value;
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
            OnCurrentViewChanged?.Invoke(PageName.Home);
        }

        private void CreateNewPage(object parameter)
        {
            CurrentView = new NewViewModel();
            OnCurrentViewChanged?.Invoke(PageName.New);
        }

        private void CreateSearchPage(object parameter)
        {
            CurrentView = new SearchViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Search);
        }

        private void CreateCompanyPage(object parameter)
        {
            CurrentView = new CompanyViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Company);
        }

        private void CreateSettingsPage(object parameter)
        {
            CurrentView = new SettingsViewModel();
            OnCurrentViewChanged?.Invoke(PageName.Settings);
        }

        public NavigationViewModel()
        {
            OpenHomePage = new RelayCommand(new Action<object>(CreateHomePage));
            OpenNewPage = new RelayCommand(new Action<object>(CreateNewPage));
            OpenSearchPage = new RelayCommand(new Action<object>(CreateSearchPage));
            OpenCompanyPage = new RelayCommand(new Action<object>(CreateCompanyPage));
            OpenSettingsPage = new RelayCommand(new Action<object>(CreateSettingsPage));

            CurrentView = new HomeViewModel();
        }
    }
}
