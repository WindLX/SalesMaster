using SalesMaster.Model;

namespace SalesMaster.Service
{
    interface ICompanyService
    {
        Companies GetCompanies();
        void AddCompany(string newCompany);
        void DeleteCompany(string targetCompany);
        void ChangeCompany(string targetCompany, string newCompany);
    }
}
