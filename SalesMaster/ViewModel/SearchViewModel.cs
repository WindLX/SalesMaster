using SalesMaster.Model;
using SalesMaster.Service;
using SalesMaster.View;
using SalesMaster.Utils;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SalesMaster.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
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

        public SearchViewModel()
        {
            CurrentView = new SearchMainViewModel();

            Broadcaster.Instace.Subscribe((parameter) => CurrentView = new EditMain(parameter), "OpenTargetSalesList");
            Broadcaster.Instace.Subscribe((parameter) => CurrentView = new SearchMainViewModel(), "ReturnSearchMainView");
        }
    }

    public class SearchMainViewModel : ViewModelBase
    {
        private ICompanyService companyService;
        private ISalesListService salesListService;

        private ObservableCollection<SalesListViewModel> salesLists = new ObservableCollection<SalesListViewModel>();
        private ObservableCollection<string> companies = new ObservableCollection<string>();
        private string consignee;
        private string startTime;
        private string endTime;
        private Visibility isShow1;
        private Visibility isShow2;

        public RelayCommand Search { get; set; }
        public RelayCommand OpenStartTime { get; set; }
        public RelayCommand OpenEndTime { get; set; }

        public ObservableCollection<SalesListViewModel> SalesLists
        {
            get => salesLists;
            set
            {
                salesLists = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Companies
        {
            get => companies;
            set
            {
                companies = value;
                OnPropertyChanged();
            }
        }

        public string Consignee
        {
            get => consignee;
            set
            {
                consignee = value;
                OnPropertyChanged();
            }
        }

        public string StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                OnPropertyChanged();
            }
        }

        public string EndTime
        {
            get => endTime;
            set
            {
                endTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime RawStartTime
        {
            get => default;
            set
            {
                StartTime = value.ToString("D");
                OnPropertyChanged();
            }
        }
        public DateTime RawEndTime
        {
            get => default;
            set
            {
                EndTime = value.ToString("D");
                OnPropertyChanged();
            }
        }

        public Visibility IsShow1
        {
            get => isShow1;
            set
            {
                isShow1 = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsShow2
        {
            get => isShow2;
            set
            {
                isShow2 = value;
                OnPropertyChanged();
            }
        }

        public SearchMainViewModel()
        {
            companyService = new JsonCompanyService();
            salesListService = new SQLiteSalesListService();

            Search = new RelayCommand(new Action<object>(SearchSalesLists));
            OpenStartTime = new RelayCommand(new Action<object>(OpenCalender));
            OpenEndTime = new RelayCommand(new Action<object>(OpenCalender));

            foreach (string item in companyService.GetCompanies().CompanyName)
            {
                Companies.Add(item);
            }
            IsShow1 = Visibility.Collapsed;
            IsShow2 = Visibility.Collapsed;

            ResetView(salesListService.GetSalesLists());
        }

        private void ResetView(List<string> salesListIDs)
        {
            SalesLists.Clear();
            int i = 0;
            foreach (string item in salesListIDs)
            {
                SalesLists.Add(new SalesListViewModel(salesListService.GetSalesList(item), i));
                i++;
            }
        }

        private void OpenCalender(object parameter)
        {
            if ((string)parameter == "1")
            {
                if (IsShow1 == Visibility.Collapsed)
                    IsShow1 = Visibility.Visible;
                else
                    IsShow1 = Visibility.Collapsed;
            }
            else
            {
                if (IsShow2 == Visibility.Collapsed)
                    IsShow2 = Visibility.Visible;
                else
                    IsShow2 = Visibility.Collapsed;
            }
        }

        private void SearchSalesLists(object parameter)
        {
            if ((consignee == "" || consignee == null) && StartTime == null && EndTime == null)
                ResetView(salesListService.GetSalesLists());
            else if (consignee != "" && consignee != null && StartTime == null && EndTime == null)
                ResetView(salesListService.GetSalesLists(consignee));
            else if (consignee != "" && consignee != null && StartTime != null && EndTime != null)
                ResetView(salesListService.GetSalesLists(consignee, DateTime.ParseExact(StartTime, "D", CultureInfo.CurrentCulture), DateTime.ParseExact(EndTime, "D", CultureInfo.CurrentCulture)));
            else if ((consignee == "" || consignee == null) && StartTime != null && EndTime != null)
                ResetView(salesListService.GetSalesLists(DateTime.ParseExact(StartTime, "D", CultureInfo.CurrentCulture), DateTime.ParseExact(EndTime, "D", CultureInfo.CurrentCulture)));
            else
                ResetView(salesListService.GetSalesLists());
        }
    }

    public class SalesListViewModel : ViewModelBase
    {
        private event Action<object, string> OnOpenTargetSalesList;

        private int index;
        private SalesList salesList;
        public RelayCommand Open { set; get; }

        public int Index 
        {
            get =>index;
            set 
            {
                index = value;
                OnPropertyChanged();
            }
        }

        public string TimeID
        {
            get => salesList.TimeID;
            set => OnPropertyChanged();
        }

        public string Name 
        {
            get => $"清单{salesList.TimeID}";
            set => OnPropertyChanged();
        }

        public string Company
        {
            get => salesList.Consignee;
            set => OnPropertyChanged();
        }

        public string SaleDate
        {
            get => salesList.SaleTime.ToString("D");
            set => OnPropertyChanged();
        }

        public float SumPrice
        {
            get => salesList.SumPrice;
            set => OnPropertyChanged();
        }

        public SalesListViewModel(SalesList salesList, int index)
        {
            this.salesList = salesList;

            Index = index;

            OnOpenTargetSalesList += Broadcaster.Instace.Publish;

            Open = new RelayCommand(new Action<object>((parameter) => OnOpenTargetSalesList?.Invoke(salesList.TimeID, "OpenTargetSalesList")));
        }
    }

    public class EditMainViewModel : ViewModelBase, IChangeState, IChangeLineNumber, IChangeChosenSalesList
    {
        public event Action<object, string> OnChangeState;
        public event Action<object, string> OnChangeLineNumber;
        public event Action<object, string> OnChangeChosenSalesList;

        public event Action<object, string> OnIsEditing;
        private event Action<object, string> OnReturnSearchMainView;

        private IExportService exportService;
        private ISalesListService salesListService;
        private ICompanyService companyService;
        private IConfigService configService;

        private string targetID;
        private SalesList newSalesList = new SalesList();

        private bool isEditing = false;
        private bool canEditing = true;
        private Visibility isShow;

        private ObservableCollection<Sale> sales = new ObservableCollection<Sale>();
        private ObservableCollection<string> companies = new ObservableCollection<string>();
        private string consignee;
        private string saleTime;

        public RelayCommand Return { get; set; }
        public RelayCommand Delete { get; set; }
        public RelayCommand Edit { get; set; }
        public RelayCommand Export { get; set; }
        public RelayCommand Add { get; set; }
        public RelayCommand Confirm { get; set; }
        public RelayCommand OpenTime { get; set; }

        public bool IsEditing
        {
            get => isEditing;
            set
            {
                isEditing = value;
                OnIsEditing?.Invoke(value, "IsEditing");
                OnPropertyChanged();
            }
        }

        public bool CanEditing
        {
            get => canEditing;
            set
            {
                canEditing = value;
                OnPropertyChanged();
            }
        }

        public Visibility IsShow
        {
            get => isShow;
            set
            {
                isShow = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Sale> Sales
        {
            get => sales;
            set
            {
                sales = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Companies
        {
            get => companies;
            set
            {
                companies = value;
                OnPropertyChanged();
            }
        }

        public string Consignee
        {
            get => consignee;
            set
            {
                consignee = value;
                OnPropertyChanged();
            }
        }

        public string SaleTime
        {
            get => saleTime;
            set
            {
                saleTime = value;
                OnPropertyChanged();
            }
        }

        public float SumPrice
        {
            get => newSalesList.SumPrice;
            set => OnPropertyChanged();
        }

        public DateTime RawSaleTime
        {
            get => default;
            set
            {
                SaleTime = value.ToString("D");
                OnPropertyChanged();
            }
        }

        public EditMainViewModel(object targetID)
        {
            this.targetID = targetID.ToString();
            
            configService = new JsonConfigService();
            companyService = new JsonCompanyService();
            salesListService = new SQLiteSalesListService();
            exportService = new ExcelExportService();

            IsShow = Visibility.Collapsed;
            newSalesList = salesListService.GetSalesList(this.targetID);
            foreach (string item in companyService.GetCompanies().CompanyName)
            {
                Companies.Add(item);
            }

            OnChangeState += Broadcaster.Instace.Publish;
            OnChangeLineNumber += Broadcaster.Instace.Publish;
            OnChangeChosenSalesList += Broadcaster.Instace.Publish;
            OnIsEditing += Broadcaster.Instace.Publish;
            OnReturnSearchMainView += Broadcaster.Instace.Publish;

            Broadcaster.Instace.Subscribe(DeleteSale, "NewDeleteSale");
            Broadcaster.Instace.Subscribe((parameter) => SumPrice = 1, "SumPriceUpdate");

            Return = new RelayCommand(new Action<object>(ReturnMain));
            Delete = new RelayCommand(new Action<object>(DeleteTargetSalesList));
            Edit = new RelayCommand(new Action<object>(EditSalesList));
            Export = new RelayCommand(new Action<object>(ExportFile));
            Confirm = new RelayCommand(new Action<object>(ConfirmChangeSalesList));
            Add = new RelayCommand(new Action<object>(AddNewSale));
            OpenTime = new RelayCommand(new Action<object>(OpenCalender));

            ResetView();
        }

        private void ReturnMain(object parameter)
        {
            OnReturnSearchMainView?.Invoke("", "ReturnSearchMainView");
            OnChangeState?.Invoke(StateName.View, "ChangeState");
        }

        private void DeleteTargetSalesList(object parameter)
        {
            salesListService.DeleteSalesList(targetID);

            OnChangeState?.Invoke(StateName.View, "ChangeState");
            ReturnMain(null);

            NotifyBox notifyBox = new NotifyBox { NotifyMessage = "删除清单成功" };
            notifyBox.Show();
        }

        private void EditSalesList(object parameter)
        {
            IsEditing = true;
            CanEditing = false;
            OnChangeState?.Invoke(StateName.Edit, "ChangeState");
        }

        private void ExportFile(object parameter)
        {
            exportService.ExportFile(salesListService.GetSalesList(targetID.ToString()), configService.GetConfig().ExportLocation);

            OnChangeState?.Invoke(StateName.View, "ChangeState");

            NotifyBox notifyBox = new NotifyBox { NotifyMessage = "导出清单成功" };
            notifyBox.Show();
        }

        private void ConfirmChangeSalesList(object parameter)
        {
            IsEditing = false;
            CanEditing = true;

            OnChangeState?.Invoke(StateName.View, "ChangeState");

            newSalesList.Consignee = Consignee;
            newSalesList.SaleTime = DateTime.ParseExact(SaleTime, "D", CultureInfo.CurrentCulture);
            salesListService.ChangeSalesList(targetID, newSalesList);

            Sales.Clear();
            ResetView();

            if (configService.GetConfig().IsAutoSaveCompany && !companyService.GetCompanies().CompanyName.Contains(consignee))
            {
                companyService.AddCompany(consignee);
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "添加新单位与更改清单成功" };
                notifyBox.Show();
            }
            else
            {
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "更改清单成功" };
                notifyBox.Show();
            }
        }

        private void OpenCalender(object parameter)
        {
            if (IsShow == Visibility.Collapsed)
                IsShow = Visibility.Visible;
            else
                IsShow = Visibility.Collapsed;
        }

        private void AddNewSale(object parameter)
        {
            OnChangeState?.Invoke(StateName.Edit, "ChangeState");
            OnIsEditing?.Invoke(true, "IsEditing");

            newSalesList.Add(new Sale(newSalesList.Count));

            ResetView();
        }

        private void DeleteSale(object saleID)
        {
            newSalesList.Remove(new Sale((int)saleID));
            newSalesList.ResetID();

            ResetView();
        }

        private void ResetView()
        {
            Sales.Clear();
            foreach (Sale sale in newSalesList)
                Sales.Add(sale);
            Consignee = newSalesList.Consignee;
            SaleTime = newSalesList.SaleTime.ToString("D");

            OnPropertyChanged("SumPrice");

            OnChangeLineNumber?.Invoke(Sales.Count, "ChangeLineNumber");
            OnChangeChosenSalesList?.Invoke($"查看清单 {newSalesList.TimeID}", "ChangeChosenSalesList");
        }
    }
}
