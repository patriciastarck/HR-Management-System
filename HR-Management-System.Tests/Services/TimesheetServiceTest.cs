using HR_Management_System.Application.Dtos;
using HR_Management_System.Application.Services;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Models;
using Moq;
using NUnit.Framework;

namespace HR_Management_System.Tests.Services;

[TestFixture]
public class TimesheetServiceTest
{
    private Mock<ITimesheetRepository> _repositoryMock;
    private TimesheetService _service;

    [SetUp]
    public void Setup()
    {
        // ARRANGE GLOBAL: Preparando o ambiente antes de cada teste
        _repositoryMock = new Mock<ITimesheetRepository>();
        _service = new TimesheetService(_repositoryMock.Object);
    }

    [Test]
    public async Task RecordTimeAsync_DeveFalhar_QuandoDataJaExistir()
    {
        // 1. ARRANGE (Organizar)
        // Definimos uma data e um ID de funcionário
        var dataTeste = new DateOnly(2024, 5, 20);
        var dto = new TimesheetRequestDto { EmployeeId = 1, Date = dataTeste };

        // Simulamos que o repositório JÁ POSSUI esse registro no banco
        var listaComRegistroExistente = new List<Timesheet>
        {
            new Timesheet { EmployeeId = 1, Date = dataTeste }
        };

        _repositoryMock.Setup(r => r.GetByEmployeeIdAsync(1))
                       .ReturnsAsync(listaComRegistroExistente);

        // 2. ACT (Agir)
        // Chamamos o método que contém a Regra de Negócio
        var resultado = await _service.RecordTimeAsync(dto);

        // 3. ASSERT (Verificar/Afirmar)
        // Validamos se a regra de "bloqueio" funcionou
        Assert.Multiple(() =>
        {
            Assert.That(resultado.Success, Is.False);
            Assert.That(resultado.Message, Is.EqualTo("Já existe um registro de ponto para este funcionário nesta data."));

            // Verificação crucial: O método AddAsync nunca deve ter sido chamado!
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Timesheet>()), Times.Never);
        });
    }
}