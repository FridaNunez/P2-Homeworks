using ZenCare.Application.DTOs;

namespace ZenCare.Application.Services.Interfaces;

public interface ISessionNoteService
{
    Task<IEnumerable<SessionNoteResponseDto>> GetByAppointmentAsync(Guid appointmentId);
    Task<SessionNoteResponseDto> CreateAsync(SessionNoteCreateDto dto);
    Task<bool> DeleteAsync(Guid id);
}