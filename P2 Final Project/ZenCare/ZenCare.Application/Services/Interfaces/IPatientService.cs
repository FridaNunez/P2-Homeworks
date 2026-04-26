using ZenCare.Application.DTOs;

namespace ZenCare.Application.Services.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetAllAsync();
    Task<PatientResponseDto?> GetByIdAsync(Guid id);
    Task<PatientResponseDto> CreateAsync(PatientCreateDto dto);
    Task<PatientResponseDto> UpdateAsync(PatientUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<AppointmentResponseDto>> GetAppointmentHistoryAsync(Guid patientId);
}