using CashFlow.Application.UseCases.Expenses.Report.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Report.Pdf
{
    public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {

        private const string CURRENCY_SYMBOL = "R$";
        private readonly IExpensesReadOnlyRepository _repository;

        public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
        {
            _repository = repository;

            GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);

            if(expenses.Count == 0)
            {
                return [];
            }

            var document = CreateDocument(month);

            return [];
        }

        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();

            DateTime.Today.ToString("");

            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = "Henrick Nogueira";

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }

    }
}
