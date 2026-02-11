using HR_Management_System.Models;
using HR_Management_System.Repositories;

namespace HR_Management_System.Services;

public class SystemUserService
{
    private readonly ISystemUserRepository _repository;

    public SystemUserService(ISystemUserRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Creates a new Systemuser after business validation.
    /// </summary>
    public async Task<Systemuser> CreateAsync(Systemuser user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        // Validate login business rules before persisting
        await ValidateLogin(user.Login);

        // Persist using repository
        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Retrieves a user by id.
    /// </summary>
    public Task<Systemuser?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    /// <summary>
    /// Validates login according to business rules:
    /// - required (not null/empty/whitespace)
    /// - minimum length 4
    /// - no spaces
    /// - unique (uses repository.LoginExists)
    /// Throws exceptions with clear messages when validation fails.
    /// </summary>
    private async Task ValidateLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("O login não pode estar vazio nem conter espaços.", nameof(login));
        }

        if (login.Length < 4)
        {
            throw new ArgumentException("O login deve ter no mínimo, 4 caracteres.", nameof(login));
        }

        if (login.Contains(' '))
        {
            throw new ArgumentException("O login não pode ter espaço.", nameof(login));
        }

        // Check uniqueness via repository
        bool exists;
        try
        {
            exists = await _repository.LoginExists(login);
        }
        catch (Exception ex)
        {
            // Fail fast with a clear message if repository check cannot be performed
            throw new InvalidOperationException("Unable to verify login uniqueness: repository check failed.", ex);
        }

        if (exists)
        {
            throw new InvalidOperationException($"Login '{login}' is already in use. Choose a different login.");
        }
    }
}