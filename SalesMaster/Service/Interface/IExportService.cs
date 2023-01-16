using SalesMaster.Model;

namespace SalesMaster.Service
{
    interface IExportService
    {
        void ExportFile(SalesList salesList, string path);
    }
}
