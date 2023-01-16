using SalesMaster.Model;
using System.Text.Json;
using System.IO;

namespace SalesMaster.Service
{
    internal class JsonConfigService : IConfigService
    {
        public Config GetConfig()
        {
            string fileName = @"..\..\Data\Config.json";
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<Config>(jsonString);
        }

        public void LoadDefaultConfig()
        {
            Config defaultConfig = new Config
            {
                IsAutoSaveCompany = true,
                IsAutoUseNowDate = true,
                ExportLocation = "..\\..\\SalesLists"
            };
            SetConfig(defaultConfig);
        }

        public void SetConfig(Config config)
        {
            string fileName = @"..\..\Data\Config.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(config, options);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
