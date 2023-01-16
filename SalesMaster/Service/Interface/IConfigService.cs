using SalesMaster.Model;

namespace SalesMaster.Service
{
    interface IConfigService
    {
        Config GetConfig();
        void SetConfig(Config config);
        void LoadDefaultConfig();
    }
}
