using SalesMaster.Utils;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace SalesMaster.Model
{
    public class SalesList: IEnumerable
    {
        private string timeID;
        private List<Sale> sales;

        public string TimeID { get => timeID; }
        public bool IsLock { get; set; }
        public string Consignee { get; set; }
        public DateTime SaleTime { get; set; }
        public int Count { get => sales.Count; }

        public float SumPrice
        { 
            get
            {
                float sumPrice = 0;
                foreach (var sale in sales)
                {
                    sumPrice += sale.TotalPrice;
                }
                return sumPrice;
            }
        }

        public SalesList(string timeID = default, string consignee = default, DateTime saleTime = default, bool isLock = false)
        {
            IsLock = isLock;
            Consignee = consignee;
            SaleTime = saleTime;
            sales = new List<Sale>();
            if (timeID == default)
                this.timeID = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            else
                this.timeID = timeID;
        }

        public Sale this[int index]
        {
            get => sales[index];
            set => sales[index] = value;
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Sale sale in sales)
                yield return sale;
        }

        public void Add(Sale sale)
        {
            if (IsLock)
                return;
            else
                sales.Add(sale);
        }

        public void Remove(Sale sale)
        {
            if (IsLock)
                return;
            else
                sales.RemoveAll(n => n.ID == sale.ID);
        }

        public void ResetID()
        {
            for (int i = 0; i < Count; i++)
                sales[i].ID = i;
        }

        //public void Change(Sale targetSale, Sale newSale)
        //{
        //    if (IsLock)
        //        return;
        //    else
        //        sales = sales.Select(n => n == targetSale ? newSale : n).ToList();
        //}

        //public void Clear()
        //{
        //    if (IsLock)
        //        return;
        //    else
        //    {
        //        sales.Clear();
        //        IsLock = false;
        //        SaleTime = default;
        //        Consignee = "";
        //    }
        //}
    }

    public class Sale : ViewModelBase
    {
        public event Action<object, string> OnDelete;
        public event Action<object, string> OnSumPriceUpdate;

        private bool isEditing;
        private int id;
        private string commodityName;
        private string unit;
        private int quantity;
        private float unitPrice;

        public RelayCommand Delete { get; set; }

        public bool IsEditing 
        {
            get => isEditing;
            set 
            {
                isEditing = value;
                OnPropertyChanged(); 
            }
        }

        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string CommodityName 
        {
            get => commodityName;
            set
            {
                commodityName = value;
                OnPropertyChanged();
            }
        }

        public string Unit 
        {
            get => unit;
            set
            {
                unit = value;
                OnPropertyChanged();
            }
        }

        public int Quantity 
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged();
                TotalPrice = 1;
                OnSumPriceUpdate?.Invoke(null, "SumPriceUpdate");
            }
        }

        public float UnitPrice
        {
            get => unitPrice;
            set
            {
                unitPrice = value;
                OnPropertyChanged();
                TotalPrice = 1;
                OnSumPriceUpdate?.Invoke(null, "SumPriceUpdate");
            }
        }

        public float TotalPrice 
        {
            get => Quantity * UnitPrice;
            set
            {
                OnPropertyChanged();
            }
        }

        public Sale() 
        {
            OnDelete += Broadcaster.Instace.Publish;
            OnSumPriceUpdate += Broadcaster.Instace.Publish;

            Broadcaster.Instace.Subscribe(new Action<object>((parameter) => IsEditing = (bool)parameter), "IsEditing");

            Delete = new RelayCommand(new Action<object>((parameter) => OnDelete?.Invoke(ID, "NewDeleteSale")));
        }

        public Sale(int id)
        {
            this.id = id;

            OnDelete += Broadcaster.Instace.Publish;
            OnSumPriceUpdate += Broadcaster.Instace.Publish;

            Broadcaster.Instace.Subscribe(new Action<object>((parameter) => IsEditing = (bool)parameter), "IsEditing");

            Delete = new RelayCommand(new Action<object>((parameter) => OnDelete?.Invoke(ID, "NewDeleteSale")));
        }
    }
}
