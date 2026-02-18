namespace HR_Management_System.Application.Dtos;

public class EmployeeRequestDto
{
    // Removi Salary, Email, IDs de departamento, etc.
    // O ID você normalmente não envia no Create, mas se precisar para um Update, ele fica aqui.

    public string Name { get; set; } = null!;
    public string Cpf { get; set; } = null!;
}