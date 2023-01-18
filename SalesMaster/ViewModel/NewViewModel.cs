using SalesMaster.Model;
using SalesMaster.Utils;
using SalesMaster.Service;
using SalesMaster.View;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Globalization;

namespace SalesMaster.ViewModel
{
    public class NewViewModel : ViewModelBase, IChangeState, IChangeLineNumber, IChangeChosenSalesList
    {
        public event Action<object, string> OnChangeState;
        public event Action<object, string> OnChangeLineNumber;
        public event Action<object, string> OnChangeChosenSalesList;

        private ISalesListService salesListService;
        private IConfigService configService;
        private ICompanyService companyService;
        protected SalesList newSalesList = new SalesList();

        protected ObservableCollection<Sale> sales = new ObservableCollection<Sale>();
        protected ObservableCollection<string> companies = new ObservableCollection<string>();
        protected string consignee;
        protected string saleTime;
        protected float sumPrice;
        protected Visibility isShow;

        public RelayCommand Add { get; set; }
        public RelayCommand Confirm { get; set; }
        public RelayCommand OpenTime { get; set; }

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

        public DateTime RawSaleTime
        {
            get => default;
            set
            {
                SaleTime = value.ToString("D");
                OnPropertyChanged();
            }
        }

        public float SumPrice
        {
            get => sumPrice;
            set
            {
                sumPrice = newSalesList.SumPrice;
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

        public NewViewModel()
        {
            configService = new JsonConfigService();
            companyService = new JsonCompanyService();
            salesListService = new SQLiteSalesListService();

            OnChangeState += Broadcaster.Instace.Publish;
            OnChangeLineNumber += Broadcaster.Instace.Publish;
            OnChangeChosenSalesList += Broadcaster.Instace.Publish;

            Broadcaster.Instace.Subscribe(DeleteSale, "NewDeleteSale");
            Broadcaster.Instace.Subscribe((parameter) => SumPrice = 1, "SumPriceUpdate");

            Add = new RelayCommand(new Action<object>(AddNewSale));
            Confirm = new RelayCommand(new Action<object>(ConfirmNewSalesList));
            OpenTime = new RelayCommand(new Action<object>(OpenCalender));

            if (configService.GetConfig().IsAutoUseNowDate)
                SaleTime = DateTime.Now.ToString("D");
            foreach (string item in companyService.GetCompanies().CompanyName)
            {
                Companies.Add(item);
            }
            IsShow = Visibility.Collapsed;
        }

        protected virtual void ResetView()
        {
            Sales.Clear();
            foreach (Sale sale in newSalesList)
                Sales.Add(sale);
            OnChangeLineNumber?.Invoke(Sales.Count, "ChangeLineNumber");
            OnChangeChosenSalesList?.Invoke($"新建清单 {newSalesList.TimeID}", "ChangeChosenSalesList");
        }

        protected void AddNewSale(object parameter)
        {
            OnChangeState?.Invoke(StateName.Edit, "ChangeState");
            newSalesList.Add(new Sale(newSalesList.Count));
            ResetView();
        }

        protected void DeleteSale(object saleID)
        {
            newSalesList.Remove(new Sale((int)saleID));
            newSalesList.ResetID();
            ResetView();
        }

        protected virtual void ConfirmNewSalesList(object parameter)
        {
            newSalesList.Consignee = Consignee;
            newSalesList.SaleTime = DateTime.ParseExact(SaleTime, "D", CultureInfo.CurrentCulture);
            salesListService.AddSalesList(newSalesList);
            Sales.Clear();
            ResetView();

            if (configService.GetConfig().IsAutoSaveCompany && !companyService.GetCompanies().CompanyName.Contains(consignee))
            {
                companyService.AddCompany(consignee);
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "添加新清单与新单位成功" };
                notifyBox.Show();
            }
            else
            {
                NotifyBox notifyBox = new NotifyBox { NotifyMessage = "添加新清单成功" };
                notifyBox.Show();
            }

            OnChangeState?.Invoke(StateName.View, "ChangeState");
        }

        protected void OpenCalender(object parameter)
        {
            if (IsShow == Visibility.Collapsed)
                IsShow = Visibility.Visible;
            else
                IsShow = Visibility.Collapsed;
        }
    }
}
