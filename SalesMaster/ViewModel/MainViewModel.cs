using SalesMaster.Utils;

namespace SalesMaster.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private TopViewModel topView;
        private NavigationViewModel navigationView;
        private StateViewModel stateView;

        public TopViewModel TopView
        {
            get => topView;
            set
            {
                topView = value;
                OnPropertyChanged();
            }
        }

        public NavigationViewModel NavigationView
        {
            get => navigationView;
            set
            {
                navigationView = value;
                OnPropertyChanged();
            }
        }

        public StateViewModel StateView
        {
            get => stateView;
            set
            {
                stateView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            TopView = new TopViewModel();
            NavigationView = new NavigationViewModel();
            StateView = new StateViewModel();
        }
    }
}
