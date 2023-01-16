using SalesMaster.Model;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace SalesMaster.Service
{
    class JsonCompanyService : ICompanyService
    {
        private void SaveCompanies(Companies companies)
        {
            string fileName = @"..\..\Data\Companies.json";
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(companies, options);
            File.WriteAllText(fileName, jsonString);
        }

        public void AddCompany(string newCompany)
        {
            Companies companies = GetCompanies();
            companies.CompanyName.Add(newCompany);
            SaveCompanies(companies);
        }

        public void ChangeCompany(string targetCompany, string newCompany)
        {
            Companies companies = GetCompanies();
            companies.CompanyName = companies.CompanyName.Select(n => n == targetCompany ? newCompany : n).ToHashSet();
            SaveCompanies(companies);
        }

        public void DeleteCompany(string targetCompany)
        {
            Companies companies = GetCompanies();
            companies.CompanyName.RemoveWhere(n => n == targetCompany);
            SaveCompanies(companies);
        }

        public Companies GetCompanies()
        {
            string fileName = @"..\..\Data\Companies.json";
            string jsonString = File.ReadAllText(fileName);
            Companies companies = JsonSerializer.Deserialize<Companies>(jsonString);
            return companies;
        }
    }
}
