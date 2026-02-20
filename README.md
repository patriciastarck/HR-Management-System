Este é o guia completo para o HR Management System, um sistema de gestão de Recursos Humanos desenvolvido em .NET 10 com foco em uma arquitetura limpa, utilizando PostgreSQL e validações avançadas.


## Descrição do Projeto
O HR Management System é uma API robusta projetada para automatizar processos de RH, incluindo o gerenciamento de funcionários, departamentos, cargos, registro de ponto (timesheet), solicitações de benefícios/férias e treinamentos.

### Principais Funcionalidades
* Gestão de Funcionários: Cadastro completo com validação de CPF único.

* Timesheet (Folha de Ponto): Registro de entrada e saída com cálculo automático de horas e bloqueio de registros duplicados no mesmo dia.

* Gestão de Treinamentos: Matrícula de funcionários em capacitações e acompanhamento de participação.

* Relatórios em PDF: Geração dinâmica de listagem de funcionários utilizando a biblioteca QuestPDF.
* Solicitações: Gerenciamento de pedidos (férias, licenças) integrados ao perfil do colaborador.


## Tecnologias Utilizadas
* Backend: .NET 10 (C#).

* Banco de Dados: PostgreSQL com Entity Framework Core.

* Validação: FluentValidation para regras de negócio complexas.

* Documentação: Swagger (OpenAPI).

* Testes: NUnit e Moq para Testes de Unidade e Mocking.

* Relatórios: QuestPDF.

## Arquitetura do Sistema
O projeto segue o padrão de Camadas:
1. Domain: Contém as entidades (ex: Employee, Department) e o contexto do banco (RhContext).

2. Application: Contém os DTOs para transferência de dados, serviços com a lógica de negócio e validadores.

3. Infrastructure: Implementação dos repositórios e acesso a dados.

4. Web/API: Controllers que expõem os endpoints REST.


## Configuração e Execução
### Pré-requisitos
* SDK do .NET 10.
* Instância do PostgreSQL ativa.
### Passo a Passo
1. Configurar a String de Conexão:
No arquivo appsettings.json ou diretamente no RhContext.cs, ajuste as credenciais:
Nota: Por padrão, o sistema busca Host=localhost;Database=rh;Username=postgres;Password=4435.
2. Executar Migrations:
Crie as tabelas no banco de dados com os comandos:
Bash
dotnet ef database update
3. Rodar a Aplicação:
Bash
dotnet run
Acesse o Swagger em: https://localhost:PORTA/swagger.

## Suíte de Testes
O projeto possui alta cobertura de testes para garantir a integridade das regras:
* Service Tests: Validam a lógica de criação, atualização e exclusão.

* Validator Tests: Garantem que logins tenham no mínimo 4 caracteres e senhas no mínimo 6.

* Controller Tests: Validam os retornos HTTP (Ok, NotFound, CreatedAtAction).

Para rodar os testes:
Bash
dotnet test

## Estrutura de Endpoints (Resumo)
MétodoEndpointDescriçãoGET/api/EmployeesLista todos os funcionários.POST/api/TimesheetsRegistra ponto diário.GET/api/Report/employees/pdfGera relatório de funcionários em PDF.POST/api/Training/EnrollMatricula funcionário em treinamento.
