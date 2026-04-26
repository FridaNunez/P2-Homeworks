using ZenCare.Application.DTOs;

namespace ZenCare.Application.Services.Interfaces;

public interface ISpecialistService
{
    Task<IEnumerable<SpecialistResponseDto>> GetAllAsync();
    Task<SpecialistResponseDto?> GetByIdAsync(Guid id);
    Task<SpecialistResponseDto> CreateAsync(SpecialistCreateDto dto);
    Task<SpecialistResponseDto> UpdateAsync(SpecialistUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<AppointmentResponseDto>> GetScheduleAsync(
        Guid specialistId, DateTime start, DateTime end);
}