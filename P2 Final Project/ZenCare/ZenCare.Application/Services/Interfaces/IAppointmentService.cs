using ZenCare.Application.DTOs;

namespace ZenCare.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<AppointmentResponseDto?> GetByIdAsync(Guid id);
    Task<AppointmentResponseDto> CreateAsync(AppointmentCreateDto dto);
    Task<AppointmentResponseDto> UpdateAsync(AppointmentUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<AppointmentResponseDto> ChangeStatusAsync(ChangeAppointmentStatusDto dto);
    Task<IEnumerable<AppointmentResponseDto>> GetByDateAsync(DateTime date);
    Task<IEnumerable<AppointmentResponseDto>> GetBySpecialistAsync(Guid specialistId);
    Task<IEnumerable<AppointmentResponseDto>> GetByPatientAsync(Guid patientId);
}