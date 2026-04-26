using ZenCare.Application.DTOs;

namespace ZenCare.Application.Services.Interfaces;

public interface ITherapyService
{
    Task<IEnumerable<TherapyResponseDto>> GetAllAsync();
    Task<TherapyResponseDto?> GetByIdAsync(Guid id);
    Task<TherapyResponseDto> CreateAsync(TherapyCreateDto dto);
    Task<TherapyResponseDto> UpdateAsync(TherapyUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<TherapyResponseDto>> GetByCategoryAsync(string category);
}