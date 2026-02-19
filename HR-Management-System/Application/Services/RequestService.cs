using HR_Management_System.Models;
using HR_Management_System.Infrastructure.Repositories;
using HR_Management_System.Application.Dtos;

namespace HR_Management_System.Application.Services;

public class RequestService : IRequestService
{
  private readonly IRequestRepository _repository;

  public RequestService(IRequestRepository repository)
  {
    _repository = repository;
  }

  public async Task<RequestResponseDto> CreateAsync(RequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var request = new Request
    {
      StartDate = dto.StartDate,
      EndDate = dto.EndDate,
      Status = dto.Status,
      Type = dto.Type,
      EmployeeId = dto.EmployeeId
    };

    await _repository.AddAsync(request);
    await _repository.SaveChangesAsync();

    return MapToResponseDto(request);
  }

  public async Task<RequestResponseDto?> GetByIdAsync(int id)
  {
    var request = await _repository.GetByIdAsync(id);
    return request is null ? null : MapToResponseDto(request);
  }

  public async Task<IEnumerable<RequestResponseDto>> GetAllAsync()
  {
    var requests = await _repository.GetAllAsync();
    return requests.Select(MapToResponseDto);
  }

  public async Task<RequestResponseDto?> UpdateAsync(int id, RequestDto dto)
  {
    if (dto is null) throw new ArgumentNullException(nameof(dto));

    var updatedData = new Request
    {
      StartDate = dto.StartDate,
      EndDate = dto.EndDate,
      Status = dto.Status,
      Type = dto.Type
    };

    var request = await _repository.UpdateAsync(id, updatedData);

    if (request is null) return null;

    await _repository.SaveChangesAsync();

    return MapToResponseDto(request);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    return await _repository.DeleteAsync(id);
  }

  private static RequestResponseDto MapToResponseDto(Request request)
  {
    return new RequestResponseDto
    {
      Id = request.Id,
      StartDate = request.StartDate,
      EndDate = request.EndDate,
      Status = request.Status,
      Type = request.Type,
      EmployeeId = request.EmployeeId
    };
  }
}