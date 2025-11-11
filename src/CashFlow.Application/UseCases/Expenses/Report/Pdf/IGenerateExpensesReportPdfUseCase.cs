namespace CashFlow.Application.UseCases.Expenses.Report.Pdf
{
    public interface IGenerateExpensesReportPdfUseCase
    {
        public Task<byte[]> Execute(DateOnly month);
    }
}
