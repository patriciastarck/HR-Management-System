using FluentValidation;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Validators;

public class TimesheetRequestDtoValidator : AbstractValidator<TimesheetRequestDto>
{
    public TimesheetRequestDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("O ID do funcionário deve ser válido.");

        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Não é possível registrar ponto em datas futuras.");

        RuleFor(x => x.EntryTime)
            .NotEmpty().WithMessage("O horário de entrada é obrigatório.");

        RuleFor(x => x.ExitTime)
            .NotEmpty().WithMessage("O horário de saída é obrigatório.")
            .GreaterThan(x => x.EntryTime)
            .WithMessage("O horário de saída deve ser posterior ao horário de entrada.")
            .When(x => x.EntryTime.HasValue);
    }
}