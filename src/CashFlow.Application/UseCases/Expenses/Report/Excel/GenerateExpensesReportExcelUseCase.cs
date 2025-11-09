using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        public Task<byte[]> Execute(DateOnly month)
        {
            var workbook = new XLWorkbook();

            workbook.Author = "Henrick Nogueira";
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
        }
    }
}
