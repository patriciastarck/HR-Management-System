using Microsoft.AspNetCore.Mvc;
using HR_Management_System.Application.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HR_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
  private readonly IEmployeeService _employeeService;

  public ReportController(IEmployeeService employeeService)
  {
    _employeeService = employeeService;
  }

  [HttpGet("employees/pdf")]
  public async Task<IActionResult> GetEmployeesPdf()
  {
    QuestPDF.Settings.License = LicenseType.Community;

    var employees = await _employeeService.GetAllAsync();

    var pdf = Document.Create(container =>
    {
      container.Page(page =>
          {
          page.Size(PageSizes.A4);
          page.Margin(2, Unit.Centimetre);
          page.DefaultTextStyle(x => x.FontSize(11));

          page.Header().Column(col =>
              {
              col.Item().Text("HR Management System")
                      .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
              col.Item().Text("Relatório de Funcionários")
                      .FontSize(14).FontColor(Colors.Grey.Darken1);
              col.Item().Text($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}")
                      .FontSize(9).FontColor(Colors.Grey.Medium);
              col.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Blue.Darken2);
            });

          page.Content().PaddingTop(20).Column(col =>
              {
              col.Item().Background(Colors.Blue.Lighten4).Padding(10)
                      .Text($"Total de Funcionários: {employees.Count()}")
                      .Bold().FontSize(12);

              col.Item().PaddingTop(15).Table(t =>
                  {
                  t.ColumnsDefinition(columns =>
                      {
                      columns.ConstantColumn(40);
                      columns.RelativeColumn(3);
                      columns.RelativeColumn(2);
                    });

                  t.Header(header =>
                      {
                      t.Cell().Background(Colors.Blue.Darken2).Padding(8)
                              .Text("ID").Bold().FontColor(Colors.White);
                      t.Cell().Background(Colors.Blue.Darken2).Padding(8)
                              .Text("Nome").Bold().FontColor(Colors.White);
                      t.Cell().Background(Colors.Blue.Darken2).Padding(8)
                              .Text("CPF").Bold().FontColor(Colors.White);
                    });

                  var isAlternate = false;
                  foreach (var emp in employees)
                  {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;
                    isAlternate = !isAlternate;

                    t.Cell().Background(bgColor).Padding(6).Text(emp.Id.ToString());
                    t.Cell().Background(bgColor).Padding(6).Text(emp.Name ?? "-");
                    t.Cell().Background(bgColor).Padding(6).Text(emp.Cpf ?? "-");
                  }
                });
            });

          page.Footer().AlignCenter().Text(text =>
              {
              text.Span("Página ");
              text.CurrentPageNumber();
              text.Span(" de ");
              text.TotalPages();
            });
        });
    });

    var pdfBytes = pdf.GeneratePdf();
    return File(pdfBytes, "application/pdf", $"funcionarios_{DateTime.Now:yyyyMMdd}.pdf");
  }
}