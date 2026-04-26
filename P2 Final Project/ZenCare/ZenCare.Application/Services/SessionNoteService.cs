using ZenCare.Application.DTOs;
using ZenCare.Application.Interfaces;
using ZenCare.Application.Services.Interfaces;
using ZenCare.Domain.Entities;

namespace ZenCare.Application.Services;

public class SessionNoteService : ISessionNoteService
{
    private readonly IUnitOfWork _uow;

    public SessionNoteService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<SessionNoteResponseDto>> GetByAppointmentAsync(Guid appointmentId)
    {
        var notes = await _uow.SessionNotes.GetByAppointmentAsync(appointmentId);
        return notes.Select(MapToResponse);
    }

    public async Task<SessionNoteResponseDto> CreateAsync(SessionNoteCreateDto dto)
    {
        var appointment = await _uow.Appointments.GetByIdAsync(dto.AppointmentId)
            ?? throw new KeyNotFoundException("Appointment not found.");

        if (appointment.Status != AppointmentStatus.Completed)
            throw new InvalidOperationException(
                "Notes can only be added to completed appointments.");

        var note = new SessionNote(dto.AppointmentId, dto.Content,
                                   dto.RecordedBy, dto.Recommendations ?? string.Empty);

        await _uow.SessionNotes.AddAsync(note);
        await _uow.SaveChangesAsync();
        return MapToResponse(note);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var note = await _uow.SessionNotes.GetByIdAsync(id);
        if (note == null) return false;
        _uow.SessionNotes.Delete(note);
        await _uow.SaveChangesAsync();
        return true;
    }

    private static SessionNoteResponseDto MapToResponse(SessionNote n) => new()
    {
        Id = n.Id,
        AppointmentId = n.AppointmentId,
        Content = n.Content,
        RecordedBy = n.RecordedBy,
        Recommendations = n.Recommendations,
        CreatedAt = n.CreatedAt
    };
}