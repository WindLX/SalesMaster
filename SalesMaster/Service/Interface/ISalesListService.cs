using System;
using System.Collections.Generic;
using SalesMaster.Model;

namespace SalesMaster.Service
{
    interface ISalesListService
    {
        List<string> GetSalesLists();
        List<string> GetSalesLists(string consignee);
        List<string> GetSalesLists(DateTime startTime, DateTime endTime);
        List<string> GetSalesLists(string consignee, DateTime startTime, DateTime endTime);
        SalesList GetSalesList(string targetTimeID);
        void AddSalesList(SalesList newSalesList);
        void AddSales(string targetTimeID, List<Sale> newSales);
        void DeleteSalesList(string targetTimeID);
        void DeleteSale(string targetTimeID, List<int> targetSaleIDs);
        void ChangeSalesList(string targetTimeID, SalesList newSalesList);
    }
}
