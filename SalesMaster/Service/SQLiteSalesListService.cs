using SalesMaster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Data;
using System.Windows.Controls;

namespace SalesMaster.Service
{
    public class SQLiteSalesListService : ISalesListService
    {
        private SQLiteConnection connection;

        public SQLiteSalesListService()
        {
            connection = new SQLiteConnection(@"Data Source=..\..\Data\SalesListDB.sqlite;Version=3;");
        }

        private void ConnectDB() => connection.Open();

        private void DisconnectDB() => connection.Close();

        private bool CompareTime(string targetTime, DateTime startTime, DateTime endTime)
        {
            DateTime dt = Convert.ToDateTime(targetTime);
            if (dt >= startTime && dt <= endTime)
                return true;
            return false;
        }

        public void AddSalesList(SalesList newSalesList)
        {
            ConnectDB();
            string sql =
                $"CREATE TABLE IF NOT EXISTS [TB{newSalesList.TimeID}] ([ID] INTEGER PRIMARY KEY AUTOINCREMENT, [CommodityName] TEXT, [Unit] TEXT, [Quantity] INTEGER, [UnitPrice] REAL);" +
                $"INSERT INTO [SalesListInfo] VALUES ('{newSalesList.TimeID}', '{newSalesList.Consignee}', '{newSalesList.SaleTime}');";
            foreach (Sale sale in newSalesList)
            {
                sql += $"INSERT INTO [TB{newSalesList.TimeID}] ([CommodityName], [Unit], [Quantity], [UnitPrice]) VALUES ('{sale.CommodityName}', '{sale.Unit}', {sale.Quantity}, {sale.UnitPrice});";
            }
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            DisconnectDB();
        }

        public void AddSales(string targetTimeID, List<Sale> newSales)
        {
            ConnectDB();
            string sql = "";
            foreach (Sale sale in newSales)
            {
                sql += $"INSERT INTO [TB{targetTimeID}] ([CommodityName], [Unit], [Quantity], [UnitPrice]) VALUES ('{sale.CommodityName}', '{sale.Unit}', {sale.Quantity}, {sale.UnitPrice});";
            }
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            DisconnectDB();
        }

        public void ChangeSalesList(string targetTimeID, SalesList newSalesList)
        {
            DeleteSalesList(targetTimeID);
            AddSalesList(newSalesList);
        }

        public void DeleteSalesList(string targetTimeID)
        {
            ConnectDB();
            string sql =
                $"DELETE FROM [SalesListInfo] WHERE [TimeID] = '{targetTimeID}';" +
                $"DROP TABLE [TB{targetTimeID}];";
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            DisconnectDB();
        }

        public void DeleteSale(string targetTimeID, List<int> targetSaleIDs)
        {
            ConnectDB();
            string sql = "";
            foreach (var ID in targetSaleIDs)
            {
                sql += $"DELETE FROM [TB{targetTimeID}] WHERE [ID] = {ID};";
            }
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            cmd.ExecuteNonQuery();
            DisconnectDB();
        }

        public List<string> GetSalesLists()
        {
            ConnectDB();
            string sql = "SELECT * FROM [SalesListInfo]";
            List<string> result = new List<string>();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                result.Add(reader["TimeID"].ToString());
            DisconnectDB();
            return result;
        }

        public List<string> GetSalesLists(string consignee)
        {
            ConnectDB();
            string sql = $"SELECT * FROM [SalesListInfo] WHERE [Consignee] = '{consignee}'";
            List<string> result = new List<string>();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                result.Add(reader["TimeID"].ToString());
            DisconnectDB();
            return result;
        }

        public List<string> GetSalesLists(DateTime startTime, DateTime endTime)
        {
            ConnectDB();
            string sql = "SELECT * FROM [SalesListInfo]";
            List<string> result = new List<string>();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                if (CompareTime(reader["SaleTime"].ToString(), startTime, endTime))
                    result.Add(reader["TimeID"].ToString());
            DisconnectDB();
            return result;
        }

        public List<string> GetSalesLists(string consignee, DateTime startTime, DateTime endTime)
        {
            ConnectDB();
            string sql = $"SELECT * FROM [SalesListInfo] WHERE [Consignee] = '{consignee}'";
            List<string> result = new List<string>();
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                if (CompareTime(reader["SaleTime"].ToString(), startTime, endTime))
                    result.Add(reader["TimeID"].ToString());
            DisconnectDB();
            return result;
        }

        public SalesList GetSalesList(string targetTimeID)
        {
            ConnectDB();
            string sql = $"SELECT * FROM [TB{targetTimeID}]";
            SalesList salesList = new SalesList(targetTimeID);
            SQLiteCommand cmd = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Sale sale = new Sale(Convert.ToInt32(reader["ID"]));
                sale.CommodityName = reader["CommodityName"].ToString();
                sale.Quantity = Convert.ToInt32(reader["Quantity"]);
                sale.Unit = reader["Unit"].ToString();
                var i = reader["UnitPrice"];
                sale.UnitPrice = (float)(double)reader["UnitPrice"];
                salesList.Add(sale);
            }
            DisconnectDB();
            return salesList;
        }
    }
}
