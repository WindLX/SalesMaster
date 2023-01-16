using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SalesMaster.Model;
using SalesMaster.Service;
using SalesMaster.View;
using SalesMaster.ViewModel;

namespace SalesMaster
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight - 9;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            MinHeight = 600;
            MinWidth = 500;

            //ExcelExportService e = new ExcelExportService();

            //SQLiteSalesListService s = new SQLiteSalesListService();
            //s.DeleteSale("1673690889", new List<int> { 1 });

            //SalesList ss = new SalesList(consignee:"aaa");
            //Sale sss = new Sale
            //{
            //    CommodityName = "a",
            //    Unit = "b",
            //    UnitPrice = 12.3f,
            //    Quantity = 10
            //};
            //ss.Add(sss);
            //Sale ssss = sss;
            //ss.Add(ssss);

            //e.ExportFile(ss, "..\\..\\SalesLists");
            //s.AddSalesList(ss);
        }
    }
}
