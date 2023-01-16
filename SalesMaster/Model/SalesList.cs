using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;

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

        public void Change(Sale targetSale, Sale newSale)
        {
            if (IsLock)
                return;
            else
                sales = sales.Select(n => n == targetSale ? newSale : n).ToList();
        }
    }

    public class Sale
    {
        private int id;
        public int ID { get => id; }
        public string CommodityName { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get => Quantity * UnitPrice; }

        public Sale() { }

        public Sale(int id)
        {
            this.id = id;
        }
    }
}
