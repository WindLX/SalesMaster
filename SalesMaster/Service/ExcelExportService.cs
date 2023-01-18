using SalesMaster.Model;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace SalesMaster.Service
{
    public class ExcelExportService : IExportService
    {
        private static string sourcePath = @"..\..\Data\ExcelTemplate.xlsx";

        private string CopyTemplate(string fileName, string path)
        {
            string targetPath = Path.Combine(path, $"{fileName}.xlsx");
            FileInfo file = new FileInfo(sourcePath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (file.Exists)
                file.CopyTo(targetPath, true);

            return targetPath;
        }

        public void ExportFile(SalesList salesList, string path)
        {
            string filePath = CopyTemplate($"销货清单{salesList.TimeID}", path);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new ExcelPackage(filePath);
            ExcelWorksheet sheet = package.Workbook.Worksheets["Sheet1"];

            sheet.Cells["C2"].Value = salesList.Consignee;
            sheet.Cells["C3"].Value = salesList.SaleTime;
            sheet.Cells["E6"].Value = $"{salesList.SumPrice.ToString("0.00")} 元";

            for (int i = 0; i < salesList.Count; i++)
            {
                if (i != salesList.Count - 1)
                {
                    sheet.InsertRow(i + 6, 1);
                    sheet.Row(i + 6).Height = 30;
                    sheet.Cells[i + 6, 1, i + 6, 6].Style.Font.Size = 12;
                    sheet.Cells[i + 6, 1, i + 6, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[i + 6, 1, i + 6, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    sheet.Cells[i + 6, 1, i + 6, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[i + 6, 1].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                    sheet.Cells[i + 6, 6].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                }
                sheet.Cells[i + 5, 1].Value = salesList[i].ID;
                sheet.Cells[i + 5, 2].Value = salesList[i].CommodityName;
                sheet.Cells[i + 5, 3].Value = salesList[i].Unit;
                sheet.Cells[i + 5, 4].Value = salesList[i].Quantity;
                sheet.Cells[i + 5, 5].Value = salesList[i].UnitPrice.ToString("0.00");
                sheet.Cells[i + 5, 6].Value = salesList[i].TotalPrice.ToString("0.00");
            }

            package.Save();
        }
    }
}
